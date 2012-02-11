using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateAllTheThingsBackend.Controller;
using RateAllTheThingsBackend.Integration;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Tests.Controller
{
    [TestClass]
    public class BarCodeControllerTests
    {
        [Fake] private IBarCodes barCodes;
        [Fake] private IApiSearchService apiSearchService;
        [UnderTest] private BarCodeController barCodeController;

        [TestInitialize]
        public void Setup()
        {
            Fake.InitializeFixture(this);
        }

        [TestMethod]
        public void Should_be_able_to_get_a_barcode()
        {            
            barCodeController.Get(0, 0);

            A.CallTo(() => barCodes.Get(0,0)).MustHaveHappened();
        }

        [TestMethod]
        public void should_not_to_api_lookup_if_barcode_is_old()
        {
            var barcode = new BarCode() {New = false};
            A.CallTo(() => this.barCodes.Get("", "", 0)).Returns(barcode);

            barCodeController.Get("", "", 0);

            A.CallTo(() => this.apiSearchService.Search("","")).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_call_api_search_service_if_barcode_is_new()
        {
            var barcode = new BarCode() { New = true };
            A.CallTo(() => this.barCodes.Get("", "", 0)).Returns(null);
            A.CallTo(() => this.barCodes.Create("", "", 0)).Returns(barcode);
            A.CallTo(() => this.apiSearchService.Search("", "")).Returns(Enumerable.Empty<ApiSearchHit>());

            barCodeController.Get("", "", 0);

            A.CallTo(() => apiSearchService.Search("","")).MustHaveHappened();
        }

        [TestMethod]
        public void Should_pick_first_item_from_search_hits_and_use_to_update_barcode()
        {
            var barcode = new BarCode() { New = true };
            var searchHit = new ApiSearchHit() {Manufacturer = "lol", Name = "baz"};
            A.CallTo(() => this.barCodes.Get("", "", 0)).Returns(null);
            A.CallTo(() => this.barCodes.Create("", "", 0)).Returns(barcode);
            A.CallTo(() => this.apiSearchService.Search("", "")).Returns(new []{searchHit});

            barCodeController.Get("", "", 0);

            A.CallTo(() => this.barCodes.Update(A<BarCode>.That.Matches(x => x.Name=="baz" && x.Manufacturer== "lol"), 0)).MustHaveHappened();
        }

        [TestMethod]
        public void Should_not_update_if_input_barcode_has_the_wrong_format_is_the_same()
        {
            var updateBarCode = new BarCode() {Format = "EAN_13", Id=1};
            var repoBarCode = new BarCode() {Format = "UPC", Id=1};

            A.CallTo(() => this.barCodes.Get(1, 0)).Returns(repoBarCode);

            barCodeController.Update(updateBarCode, 0);

            A.CallTo(() => this.barCodes.Update(A<BarCode>._,A<long>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_not_update_if_input_barcode_has_the_wrong_code_is_the_same()
        {
            var updateBarCode = new BarCode() { Code = "1234567890", Id = 1 };
            var repoBarCode = new BarCode() { Code = "0987654321", Id = 1 };

            A.CallTo(() => this.barCodes.Get(1, 0)).Returns(repoBarCode);

            barCodeController.Update(updateBarCode, 0);

            A.CallTo(() => this.barCodes.Update(A<BarCode>._, A<long>._)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_be_allowed_to_update_barcode_if_format_and_code_is_the_same()
        {
            var updateBarCode = new BarCode() { Format = "EAN_13", Code = "1234567890", Id = 1, Name="foo", Manufacturer = "bar"};
            var repoBarCode = new BarCode() { Format = "EAN_13", Code = "1234567890", Id = 1 };

            A.CallTo(() => this.barCodes.Get(1, 0)).Returns(repoBarCode);

            barCodeController.Update(updateBarCode, 0);

            A.CallTo(() => this.barCodes.Update(A<BarCode>.That.Matches(x => x.Name == "foo" && x.Manufacturer=="bar"), A<long>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Should_not_be_possible_to_rate_a_barcode_that_doesnt_exist()
        {
            A.CallTo(() => this.barCodes.Exists(10)).Returns(false);

            try
            {
                barCodeController.Rate(10, 1, 0);
            }
            catch (ArgumentException)
            {
                return;
            }
            Assert.Fail();
        }
        
        [TestMethod]
        public void Should_not_be_possible_to_rate_a_barcode_lower_than_one()
        {
            A.CallTo(() => this.barCodes.Exists(10)).Returns(true);

            barCodeController.Rate(10, 0, 0);

            A.CallTo(() => this.barCodes.Rate(10,0,0)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_not_be_possible_to_rate_a_barcode_higher_than_six()
        {
            A.CallTo(() => this.barCodes.Exists(10)).Returns(true);

            barCodeController.Rate(10, 7, 0);

            A.CallTo(() => this.barCodes.Rate(10, 0, 0)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_not_be_possible_to_rate_a_barcode_more_than_once()
        {
            A.CallTo(() => this.barCodes.Exists(10)).Returns(true);
            A.CallTo(() => this.barCodes.HasRated(0, 10)).Returns(true);

            barCodeController.Rate(10, 5, 0);

            A.CallTo(() => this.barCodes.Rate(10, 0, 0)).MustNotHaveHappened();
        }

        [TestMethod]
        public void Should_be_possible_to_rate_barcode()
        {
            A.CallTo(() => this.barCodes.Exists(10)).Returns(true);
            A.CallTo(() => this.barCodes.HasRated(0, 10)).Returns(false);

            barCodeController.Rate(10, 5, 0);

            A.CallTo(() => this.barCodes.Rate(10, 5, 0)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Should_return_barcode_from_repo()
        {
            var expectedBarcode = new BarCode();

            A.CallTo(() => this.barCodes.Exists(10)).Returns(true);
            A.CallTo(() => this.barCodes.Get(10, 0)).Returns(expectedBarcode);

            var barcode = barCodeController.Rate(10, 5, 0);

            Assert.AreEqual(barcode, expectedBarcode);
        }
    }
}

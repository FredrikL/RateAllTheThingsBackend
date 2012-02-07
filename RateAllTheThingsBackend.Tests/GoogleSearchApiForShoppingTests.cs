using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateAllTheThingsBackend.Integration;

namespace RateAllTheThingsBackend.Tests
{
    [TestClass]
    public class GoogleSearchApiForShoppingTests
    {
        [TestMethod]
        public void Test()
        {
            var api = new GoogleSearchApiForShopping();

            var x = api.Search("", "9781905211487");

            Assert.AreEqual(1, x.Count());
        }
    }
}
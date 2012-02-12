using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateAllTheThingsBackend.Controller;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Tests.Controller
{
    [TestClass]
    public class UserControllerTest
    {        
        [UnderTest] private UserController userController;
        [Fake] private IUsers users;

        [TestInitialize]
        public void Setup()
        {
            Fake.InitializeFixture(this);
        }

        [TestMethod]
        public void Should_be_able_to_update_user()
        {
            var user = A.Dummy<User>();

            this.userController.Update(user);

            A.CallTo(() => this.users.Update(user)).MustHaveHappened();
        }
    }
}
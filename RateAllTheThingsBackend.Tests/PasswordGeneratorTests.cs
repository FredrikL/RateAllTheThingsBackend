using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateAllTheThingsBackend.Core;

namespace RateAllTheThingsBackend.Tests
{
    [TestClass]
    public class PasswordGeneratorTests
    {
        [TestMethod]
        public void ShouldGenerateAPassword()
        {
            PasswordGenerator gen = new PasswordGenerator();

            string generate = gen.Generate();
            Debug.WriteLine(generate);
            Assert.AreNotEqual(generate, string.Empty);
        }

        [TestMethod]
        public void PasswordLengthShouldBeSeventyChars()
        {
            PasswordGenerator gen = new PasswordGenerator();

            Assert.AreEqual(70, gen.Generate().Length);
        }
    }
}

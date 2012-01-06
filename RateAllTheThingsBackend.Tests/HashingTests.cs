using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RateAllTheThingsBackend.Core;

namespace RateAllTheThingsBackend.Tests
{
    [TestClass]
    public class HashingTests
    {
        [TestMethod]
        public void ShouldHashInputtedData()
        {
            Hashing hasing = new Hashing();
            var data = "FOOBAR";

            Assert.AreEqual("2E28E57E85DFBC3622830D4FAD1961D9660344B4D3980A751A52270C4B0F83E7844A2A99B2917115051A204DE5AC81F4C4843D78A12983196DDBDD793AAA9369", hasing.Hash(data));
        }
    }
}

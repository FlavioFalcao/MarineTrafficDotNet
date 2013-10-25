using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MarineTrafficDotNet;

namespace MarineTrafficDotNet.Test
{
    [TestClass]
    public class MarineTrafficTest
    {
        [TestMethod]
        public void GetImage()
        {
            string imageUrl = MarineTraffic.GetVesselPhoto("9597214");
            Assert.AreEqual(imageUrl, "http://services.marinetraffic.com/photos/show/902094");
        }
    }
}

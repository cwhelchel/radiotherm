using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioThermLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLib.Tests
{
    [TestClass()]
    public class MarvellDiscoveryTests
    {
        [TestMethod()]
        public void MarvellDiscoveryTest()
        {
            using (var uut = new MarvellDiscovery(IPAddress.Parse("192.168.11.2"), 5000))
            {
                uut.Discover();
                Assert.AreEqual(1, uut.DiscoveredDevices.Count);
                Assert.IsTrue(uut.DiscoveredDevices[0].Equals(IPAddress.Parse("192.168.11.156")));
            }
        }
    }
}
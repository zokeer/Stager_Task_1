using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LukeSkywalker.IPNetwork;

namespace Task_1.Models.Tests
{
    [TestClass()]
    public class SubnetTests
    {
        [TestMethod()]
        public void Subnet_FixedParams_CreatedInstance()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            Assert.AreEqual(subnet.Id, "id");
            Assert.AreEqual(IPNetwork.Parse("0.0.0.0/24"), subnet.Network);
        }

        [TestMethod()]
        public void GetHashCode_SomeString_GivesIdHashCode()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            Assert.AreEqual(subnet.Id.GetHashCode(), subnet.GetHashCode());
        }

        #region EqualsTests
        [TestMethod()]
        public void Equals_EqualSubnets_Success()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            var other_subnet = new Subnet("id", "0.0.0.0/24");
            Assert.AreEqual(subnet, other_subnet);
        }

        [TestMethod()]
        public void Equals_DifferentIds_Fail()
        {
            var subnet = new Subnet("id1", "0.0.0.0/24");
            var other_subnet = new Subnet("id2", "0.0.0.0/24");
            Assert.AreNotEqual(subnet, other_subnet);
        }

        [TestMethod()]
        public void Equals_DifferentNetworks_Fail()
        {
            var subnet = new Subnet("id", "5.0.0.0/24");
            var other_subnet = new Subnet("id", "0.0.0.0/30");
            Assert.AreNotEqual(subnet, other_subnet);
        }
        #endregion
    }
}
using LukeSkywalker.IPNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task_1.Models.Tests
{
    [TestClass]
    public class SubnetTests
    {
        [TestMethod]
        public void Subnet_FixedParams_CreatedInstance()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            Assert.AreEqual(subnet.Id, "id");
            Assert.AreEqual(IPNetwork.Parse("0.0.0.0/24"), subnet.Network);
        }

        [TestMethod]
        public void GetHashCode_SomeString_GivesIdHashCode()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            Assert.AreEqual(subnet.Id.GetHashCode(), subnet.GetHashCode());
        }

        #region EqualsTests
        [TestMethod]
        public void Equals_EqualSubnets_Success()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            var other_subnet = new Subnet("id", "0.0.0.0/24");
            Assert.AreEqual(subnet, other_subnet);
        }

        [TestMethod]
        public void Equals_DifferentIds_Fail()
        {
            var subnet = new Subnet("id1", "0.0.0.0/24");
            var other_subnet = new Subnet("id2", "0.0.0.0/24");
            Assert.AreNotEqual(subnet, other_subnet);
        }

        [TestMethod]
        public void Equals_DifferentNetworks_Fail()
        {
            var subnet = new Subnet("id", "5.0.0.0/24");
            var other_subnet = new Subnet("id", "0.0.0.0/30");
            Assert.AreNotEqual(subnet, other_subnet);
        }
        #endregion

        #region isCoveringTests
        [TestMethod]
        public void IsCovering_LargeSoversSmall_Success()
        {
            var large = new Subnet("large", "10.0.0.0/23");
            var small = new Subnet("small", "10.0.1.0/24");
            Assert.IsTrue(large.IsCovering(small));
        }

        [TestMethod]
        public void IsCovering_SmallCoveringLarge_Fail()
        {
            var large = new Subnet("large", "10.0.0.0/23");
            var small = new Subnet("small", "10.0.1.0/24");
            Assert.IsFalse(small.IsCovering(large));
        }

        [TestMethod]
        public void IsCovering_SameSubnets_Success()
        {
            var large = new Subnet("large", "10.0.0.0/23");
            Assert.IsTrue(large.IsCovering(large));
        }

        [TestMethod]
        public void IsCovering_TransitiveCover_Success()
        { 
            var large = new Subnet("large", "10.0.0.0/23");
            var small = new Subnet("small", "10.0.0.0/24");
            var smallest = new Subnet("smallest", "10.0.0.0/30");
            Assert.IsTrue(large.IsCovering(small));
            Assert.IsTrue(small.IsCovering(smallest));
            Assert.IsTrue(large.IsCovering(smallest));
        }
        #endregion
    }
}
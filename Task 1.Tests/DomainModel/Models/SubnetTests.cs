using System;
using DomainModel.Models;
using LukeSkywalker.IPNetwork;
using NUnit.Framework;

namespace Task_1.Models.Tests
{
    [Parallelizable]
    [TestFixture]
    public class SubnetTests
    {
        [Test]
        public void Subnet_FixedParams_CreatedInstance()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");

            Assert.AreEqual(subnet.Id, "id");
            Assert.AreEqual(IPNetwork.Parse("0.0.0.0/24"), subnet.Network);
        }

        [Test]
        public void Subnet_NullId_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Subnet(null, "0.0.0.0/24"));
        }

        [Test]
        public void Subnet_NullSubnet_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Subnet("fddd", null));
        }

        [Test]
        public void GetHashCode_SomeString_GivesIdHashCode()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");

            Assert.AreEqual(subnet.Id.GetHashCode(), subnet.GetHashCode());
        }

        #region EqualsTests
        [Test]
        public void Equals_EqualSubnets_Success()
        {
            var subnet = new Subnet("id", "0.0.0.0/24");
            var other_subnet = new Subnet("id", "0.0.0.0/24");

            Assert.AreEqual(subnet, other_subnet);
        }

        [Test]
        public void Equals_DifferentIds_Fail()
        {
            var subnet = new Subnet("id1", "0.0.0.0/24");
            var other_subnet = new Subnet("id2", "0.0.0.0/24");

            Assert.AreNotEqual(subnet, other_subnet);
        }

        [Test]
        public void Equals_DifferentNetworks_Fail()
        {
            var subnet = new Subnet("id", "5.0.0.0/24");
            var other_subnet = new Subnet("id", "0.0.0.0/30");

            Assert.AreNotEqual(subnet, other_subnet);
        }

        [Test]
        public void Equals_NotASubnet_Fail()
        {
            var subnet = new Subnet("id", "5.0.0.0/24");

            Assert.AreNotEqual(subnet, "notASubnet");
        }
        #endregion

        #region isCoveringTests
        [Test]
        public void IsCovering_LargeSoversSmall_Success()
        {
            var large = new Subnet("large", "10.0.0.0/23");
            var small = new Subnet("small", "10.0.1.0/24");

            Assert.IsTrue(large.IsCovering(small));
        }

        [Test]
        public void IsCovering_SmallCoveringLarge_Fail()
        {
            var large = new Subnet("large", "10.0.0.0/23");
            var small = new Subnet("small", "10.0.1.0/24");

            Assert.IsFalse(small.IsCovering(large));
        }

        [Test]
        public void IsCovering_SameSubnets_Success()
        {
            var large = new Subnet("large", "10.0.0.0/23");

            Assert.IsTrue(large.IsCovering(large));
        }

        [Test]
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
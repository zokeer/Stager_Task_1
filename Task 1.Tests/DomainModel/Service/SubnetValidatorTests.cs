using DomainModel.Service;
using NUnit.Framework;

namespace Task_1.DomainModel.Service.Tests
{
    [TestFixture]
    public class SubnetValidatorTests
    {
        #region IsValidAddressTests
        [Test]
        public void IsValidAddress_RightAddress_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidAddress("192.168.168.0"));
        }

        [Test]
        public void IsValidAddress_ZeroAddress_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidAddress("0.0.0.0"));
        }

        [Test]
        public void IsValidAddress_RightMaskedAddress_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidAddress("0.0.0.0/24"));
        }

        [Test]
        public void IsValidAddress_NotAnAddress_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidAddress("192.168."));
        }

        [Test]
        public void IsValidAddress_WrongMaskedAddress_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidAddress("192.168./24"));
        }

        [Test]
        public void IsValidAddress_NegativeAddress_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidAddress("-192.-168./24"));
        }
        #endregion
        #region IsValidMaskTests
        [Test]
        public void IsValidMask_RightMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/24"));
        }

        [Test]
        public void IsValidMask_ZeroMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/0"));
        }
        
        [Test]
        public void IsValidMask_NegativeMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/-2"));
        }

        [Test]
        public void IsValidMask_AddressNotAStartForMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.12/24"));
        }

        [Test]
        public void IsValidMask_LargeMask_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/33"));
        }

        [Test]
        public void IsValidMask_MoreThanTwoDigitMask_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/016"));
        }

        [Test]
        public void IsValidMask_EmptyMask_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/"));
        }
        #endregion
    }
}
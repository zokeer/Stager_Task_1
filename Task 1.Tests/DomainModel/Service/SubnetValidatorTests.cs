using DomainModel.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidAddress("192.168.168.0"));
        }

        [Test]
        public void IsValidAddress_ZeroAddress_Success()
        {
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidAddress("0.0.0.0"));
        }

        [Test]
        public void IsValidAddress_RightMaskedAddress_Success()
        {
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidAddress("0.0.0.0/24"));
        }

        [Test]
        public void IsValidAddress_NotAnAddress_Fail()
        {
            NUnit.Framework.Assert.IsFalse(SubnetValidator.IsValidAddress("192.168."));
        }

        [Test]
        public void IsValidAddress_WrongMaskedAddress_Fail()
        {
            NUnit.Framework.Assert.IsFalse(SubnetValidator.IsValidAddress("192.168./24"));
        }

        [Test]
        public void IsValidAddress_NegativeAddress_Fail()
        {
            NUnit.Framework.Assert.IsFalse(SubnetValidator.IsValidAddress("-192.-168./24"));
        }
        #endregion
        #region IsValidMaskTests
        [Test]
        public void IsValidMask_RightMask_Success()
        {
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/24"));
        }

        [Test]
        public void IsValidMask_ZeroMask_Success()
        {
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/0"));
        }
        
        [Test]
        public void IsValidMask_NegativeMask_Success()
        {
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/-2"));
        }

        [Test]
        public void IsValidMask_AddressNotAStartForMask_Success()
        {
            NUnit.Framework.Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.12/24"));
        }

        [Test]
        public void IsValidMask_LargeMask_Fail()
        {
            NUnit.Framework.Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/33"));
        }

        [Test]
        public void IsValidMask_MoreThanTwoDigitMask_Fail()
        {
            NUnit.Framework.Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/016"));
        }

        [Test]
        public void IsValidMask_EmptyMask_Fail()
        {
            NUnit.Framework.Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/"));
        }
        #endregion
    }
}
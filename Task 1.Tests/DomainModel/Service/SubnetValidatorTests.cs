using DomainModel.Models;
using DomainModel.Service;
using NUnit.Framework;

namespace Task_1.DomainModel.Service.Tests
{
    [Parallelizable]
    [TestFixture]
    public class SubnetValidatorTests
    {
        #region IsValidAddressTests
        [Test]
        public void IsValidAddress_RightAddress_Success()
        {
            var result = SubnetValidator.IsValidAddress("192.168.168.0");

            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
            Assert.AreEqual(result.Field, SubnetField.Address);
        }

        [Test]
        public void IsValidAddress_ZeroAddress_Success()
        {
            var result = SubnetValidator.IsValidAddress("0.0.0.0");

            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
            Assert.AreEqual(result.Field, SubnetField.Address);
        }

        [Test]
        public void IsValidAddress_RightMaskedAddress_Success()
        {
            var result = SubnetValidator.IsValidAddress("0.0.0.0/24");

            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
            Assert.AreEqual(result.Field, SubnetField.Address);

        }

        [Test]
        public void IsValidAddress_NotAnAddress_Fail()
        {
            var result = SubnetValidator.IsValidAddress("192.168.");
            
            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Address);
        }

        [Test]
        public void IsValidAddress_WrongMaskedAddress_Fail()
        {
            var result = SubnetValidator.IsValidAddress("192.168./24");

            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Address);
        }

        [Test]
        public void IsValidAddress_NegativeAddress_Fail()
        {
            var result = SubnetValidator.IsValidAddress("-192.-168./24");

            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Address);
        }
        #endregion
        #region IsValidMaskTests
        [Test]
        public void IsValidMask_RightMask_Success()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.0/24");

            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }

        [Test]
        public void IsValidMask_ZeroMask_Success()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.0/0");
            
            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }
        
        [Test]
        public void IsValidMask_NegativeMask_Fail()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.0/-2");

            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }
        
        [Test]
        public void IsValidMask_AddressNotAStartForMask_Success()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.12/24");

            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }

        [Test]
        public void IsValidMask_LargeMask_Fail()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.0/33");

            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }

        [Test]
        public void IsValidMask_MoreThanTwoDigitMask_Fail()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.0/016");

            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }

        [Test]
        public void IsValidMask_EmptyMask_Fail()
        {
            var result = SubnetValidator.IsValidMask("192.168.168.0/");

            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
            Assert.AreEqual(result.Field, SubnetField.Mask);
        }
        #endregion
    }
}
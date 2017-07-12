using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1.Models.Tests
{
    [TestClass()]
    public class SubnetValidatorTests
    {
        IRepository test_repository = new FileRepository("test.txt");
        #region IsValidAddressTests
        [TestMethod()]
        public void IsValidAddress_RightAddress_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidAddress("192.168.168.0"));
        }

        [TestMethod()]
        public void IsValidAddress_ZeroAddress_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidAddress("0.0.0.0"));
        }

        [TestMethod()]
        public void IsValidAddress_RightMaskedAddress_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidAddress("0.0.0.0/24"));
        }

        [TestMethod()]
        public void IsValidAddress_NotAnAddress_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidAddress("192.168."));
        }

        [TestMethod()]
        public void IsValidAddress_WrongMaskedAddress_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidAddress("192.168./24"));
        }

        [TestMethod()]
        public void IsValidAddress_NegativeAddress_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidAddress("-192.-168./24"));
        }
        #endregion
        #region IsValidMaskTests
        [TestMethod()]
        public void IsValidMask_RightMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/24"));
        }

        [TestMethod()]
        public void IsValidMask_ZeroMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/0"));
        }
        
        [TestMethod()]
        public void IsValidMask_NegativeMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.0/-2"));
        }

        [TestMethod()]
        public void IsValidMask_AddressNotAStartForMask_Success()
        {
            Assert.IsTrue(SubnetValidator.IsValidMask("192.168.168.12/24"));
        }

        [TestMethod()]
        public void IsValidMask_LargeMask_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/33"));
        }

        [TestMethod()]
        public void IsValidMask_MoreThanTwoDigitMask_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/016"));
        }

        [TestMethod()]
        public void IsValidMask_EmptyMask_Fail()
        {
            Assert.IsFalse(SubnetValidator.IsValidMask("192.168.168.0/"));
        }
        #endregion
        #region IsValidIdTests
        [TestMethod()]
        public void isValidId_RightId_Success()
        {
            Assert.IsTrue(SubnetValidator.isValidId(test_repository, "NotExistingID"));
        }

        [TestMethod()]
        public void isValidId_ExistingId_Fail()
        {
            Assert.IsFalse(SubnetValidator.isValidId(test_repository, "Первая First"));
        }

        [TestMethod()]
        public void isValidId_EmptyId_Fail()
        {
            Assert.IsFalse(SubnetValidator.isValidId(test_repository, ""));
        }

        [TestMethod()]
        public void isValidId_LargeId_Fail()
        {
            Assert.IsFalse(SubnetValidator.isValidId(test_repository, new string('*', 256)));
        }
        #endregion
    }
}
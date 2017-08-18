using NUnit.Framework;

namespace DomainModel.Models.Tests
{
    [Parallelizable]
    [TestFixture]
    public class ValidationLogTests
    {
        [Test]
        public void ValidationLog_ValidParams_Success()
        {
            var result = new ValidationLog(SubnetField.Id, LogInfo.NoErrors);

            Assert.AreEqual(result.Field, SubnetField.Id);
            Assert.AreEqual(result.LogInfo, LogInfo.NoErrors);
        }

        [Test]
        public void ToString_IdNoErrors_Success()
        {
            var result = new ValidationLog(SubnetField.Id, LogInfo.NoErrors);

            Assert.AreEqual("Идентификатор в порядке.", result.ToString());
        }

        [Test]
        public void ToString_IdNotUnique_Success()
        {
            var result = new ValidationLog(SubnetField.Id, LogInfo.NotUnique);

            Assert.AreEqual("Идентификатор не уникален.", result.ToString());
        }

        [Test]
        public void ToString_IdNotExists_Success()
        {
            var result = new ValidationLog(SubnetField.Id, LogInfo.NotExists);

            Assert.AreEqual("Идентификатор не существует.", result.ToString());
        }

        [Test]
        public void ToString_IdInvalid_Success()
        {
            var result = new ValidationLog(SubnetField.Id, LogInfo.Invalid);

            Assert.AreEqual("Идентификатор содержит ошибку.", result.ToString());
        }

        [Test]
        public void ToString_AddressInvalid_Success()
        {
            var result = new ValidationLog(SubnetField.Address, LogInfo.Invalid);

            Assert.AreEqual("Адрес содержит ошибку.", result.ToString());
        }

        [Test]
        public void ToString_MaskInvalid_Success()
        {
            var result = new ValidationLog(SubnetField.Mask, LogInfo.Invalid);

            Assert.AreEqual("Маска содержит ошибку.", result.ToString());
        }

        [Test]
        public void ToString_EverythingIsFine_Success()
        {
            var result = new ValidationLog(SubnetField.Everything, LogInfo.NoErrors);

            Assert.AreEqual("Всё в порядке.", result.ToString());
        }
    }
}
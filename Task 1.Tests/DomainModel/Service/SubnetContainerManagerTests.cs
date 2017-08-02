using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;
using DomainModel.Repository;
using DomainModel.Service;
using Moq;
using NUnit.Framework;

namespace Task_1.DomainModel.Service.Tests
{
    [TestFixture]
    public class SubnetContainerManagerTests
    {
        #region GetTests
        [Test]
        public void GetTest_SameCollections()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            var test_list = new List<Subnet>
            {
                new Subnet("4", "192.168.168.0/30"),
                new Subnet("123123sometext@!%@^&!#", "192.168.168.0/26"),
                new Subnet("2-Вторая", "192.168.168.0/24"),
                new Subnet("Первая First", "192.168.168.0/24")
            };

            mock.Setup(m => m.Get()).Returns(test_list);

            foreach(var test_subnet in test_list)
            {
                Assert.IsTrue(subnet_container_manager.Get().Contains(test_subnet));
            }
        }

        [Test]
        public void GetTest_CollectionsOfDifferentCount()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            var test_list = new List<Subnet>
            {
                new Subnet("4", "192.168.168.0/30"),
                new Subnet("123123sometext@!%@^&!#", "192.168.168.0/26"),
                new Subnet("2-Вторая", "192.168.168.0/24"),
                new Subnet("Первая First", "192.168.168.0/24")
            };

            mock.Setup(m => m.Get()).Returns(test_list.Skip(2).ToList());

            Assert.AreNotEqual(test_list.Count, subnet_container_manager.Get().Count);
        }

        [Test]
        public void GetTest_CollectionsOfDifferentElementsId()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            var test_list = new List<Subnet>
            {
                new Subnet("4", "192.168.168.0/30"),
                new Subnet("123123sometext@!%@^&!#", "192.168.168.0/26"),
                new Subnet("2-Вторая", "192.168.168.0/24"),
                new Subnet("Первая First", "192.168.168.0/24")
            };

            var mock_list = new List<Subnet>
            {
                new Subnet("41", "192.168.168.0/30"),
                new Subnet("1", "192.168.168.0/26"),
                new Subnet("2", "192.168.168.0/24"),
                new Subnet("Первая", "192.168.168.0/24")
            };

            mock.Setup(m => m.Get()).Returns(mock_list);

            foreach (var test_subnet in test_list)
            {
                Assert.IsFalse(subnet_container_manager.Get().Contains(test_subnet));
            }
        }

        [Test]
        public void GetTest_CollectionsOfDifferentElementsNetworks()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            var test_list = new List<Subnet>
            {
                new Subnet("4", "192.168.168.0/30"),
                new Subnet("123123sometext@!%@^&!#", "192.168.168.0/26"),
                new Subnet("2-Вторая", "192.168.168.0/24"),
                new Subnet("Первая First", "192.168.168.0/24")
            };

            var mock_list = new List<Subnet>
            {
                new Subnet("4", "5.168.168.0/30"),
                new Subnet("123123sometext@!%@^&!#", "5.168.168.0/26"),
                new Subnet("2-Вторая", "5.168.168.0/24"),
                new Subnet("Первая First", "5.168.168.0/24")
            };

            mock.Setup(m => m.Get()).Returns(mock_list);

            foreach (var test_subnet in test_list)
            {
                Assert.IsFalse(subnet_container_manager.Get().Contains(test_subnet));
            }
        }
        #endregion
        #region CreateTests
        [Test]
        public void Create_ValidSubnet_RepositoryMethodCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet>());
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));

            subnet_container_manager.Create("new_ID", "192.168.168.0/24");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Create_ExistingIdSubnet_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> {new Subnet("new_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));

            subnet_container_manager.Create("new_ID", "192.168.168.0/24");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Create_WrongAddressSubnet_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));

            var result = subnet_container_manager.Create("new_ID", "192asdas.168dasd/24");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(result.Field, SubnetField.Address);
            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
        }

        [Test]
        public void Create_WrongMaskSubnet_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));

            var result = subnet_container_manager.Create("new_ID", "192.168.168.0/50");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(result.Field, SubnetField.Mask);
            Assert.AreEqual(result.LogInfo, LogInfo.Invalid);
        }

        [Test]
        public void Create_LongId_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("new_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));

            subnet_container_manager.Create(new string('*', 256), "192.168.168.0/50");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        #endregion
        #region DeleteTests
        [Test]
        public void Delete_ValidId_RepositoryMethodCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("new_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Delete(It.IsAny<string>()));

            subnet_container_manager.Delete("new_ID");
            mock.Verify(m => m.Delete(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Delete_LongId_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Delete(It.IsAny<string>()));

            subnet_container_manager.Delete(new string('*', 256));
            mock.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Delete_EmptyId_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Delete(It.IsAny<string>()));

            subnet_container_manager.Delete("");
            mock.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
        }
        #endregion
        #region EditTests
        [Test]
        public void Edit_ValidIds_RepositoryMethodCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            subnet_container_manager.Edit("old_ID", "new_ID", "10.0.0.0/30");
            mock.Verify(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Edit_IDsAreSame_RepositoryMethodCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            subnet_container_manager.Edit("old_ID", "old_ID", "10.0.0.0/30");
            mock.Verify(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Edit_NotExistingOldId_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var result = subnet_container_manager.Edit("invalid", "new_ID", "10.0.0.0/30");
            mock.Verify(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(result.Field, SubnetField.Id);
            Assert.AreEqual(result.LogInfo, LogInfo.NotExists);
        }

        [Test]
        public void Edit_InvalidNewId_RepositoryMethodNotCalled()
        {
            var mock = new Mock<IRepository>();
            var subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet> { new Subnet("old_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            subnet_container_manager.Edit("invalid", new string('*', 256), "10.0.0.0/30");
            mock.Verify(m => m.Edit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        #endregion
    }
}

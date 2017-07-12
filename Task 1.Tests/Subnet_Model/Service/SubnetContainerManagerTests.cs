using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Task_1.Tests.Subnet_Model.Service
{
    [TestClass]
    public class SubnetContainerManagerTests
    {
        #region GetTests
        [TestMethod()]
        public void GetTest_SameCollections()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
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

        [TestMethod()]
        public void GetTest_CollectionsOfDifferentCount()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
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

        [TestMethod()]
        public void GetTest_CollectionsOfDifferentElementsId()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
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

        [TestMethod()]
        public void GetTest_CollectionsOfDifferentElementsNetworks()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
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
        [TestMethod()]
        public void Create_ValidSubnet_RepositoryMethodCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet>());
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));
            subnet_container_manager.Create("new_ID", "192.168.168.0/24");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void Create_ExistingIdSubnet_RepositoryMethodNotCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet>() {new Subnet("new_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));
            subnet_container_manager.Create("new_ID", "192.168.168.0/24");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void Create_WrongAddressSubnet_RepositoryMethodNotCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet>() { new Subnet("new_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));
            subnet_container_manager.Create("new_ID", "192.168/24");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void Create_WrongMaskSubnet_RepositoryMethodNotCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Get()).Returns(new List<Subnet>() { new Subnet("new_ID", "192.168.168.0/24") });
            mock.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()));
            subnet_container_manager.Create("new_ID", "192.168.168.0/50");
            mock.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        #endregion
        #region DeleteTests
        [TestMethod()]
        public void Delete_ValidId_RepositoryMethodCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Delete(It.IsAny<string>()));
            subnet_container_manager.Delete("new_ID");
            mock.Verify(m => m.Delete(It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void Delete_LongId_RepositoryMethodNotCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Delete(It.IsAny<string>()));
            subnet_container_manager.Delete(new string('*', 256));
            mock.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void Delete_EmptyId_RepositoryMethodNotCalled()
        {
            Mock<IRepository> mock = new Mock<IRepository>();
            SubnetContainerManager subnet_container_manager = new SubnetContainerManager(mock.Object);
            mock.Setup(m => m.Delete(It.IsAny<string>()));
            subnet_container_manager.Delete("");
            mock.Verify(m => m.Delete(It.IsAny<string>()), Times.Never);
        }
        #endregion
    }
}

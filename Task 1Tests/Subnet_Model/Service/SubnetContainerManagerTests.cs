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
    public class SubnetContainerManagerTests
    {
        SubnetContainerManager subnet_container_manager = new SubnetContainerManager(new FileRepository("test.txt"));
        [TestMethod()]
        public void GetTest()
        {
            var test_list = new List<Subnet>
            {
                new Subnet("4", "192.168.168.0/30"),
                new Subnet("123123sometext@!%@^&!#", "192.168.168.0/26"),
                new Subnet("2-Вторая", "192.168.168.0/24"),
                new Subnet("Первая First", "192.168.168.0/24"),
            };

            for (var i = 0; i<= test_list.Count; i++)
            {
                Assert.AreEqual(test_list[i].Id, subnet_container_manager.Get().ElementAt(i).Id);
                Assert.AreEqual(test_list[i].Network, subnet_container_manager.Get().ElementAt(i).Network);
            }
        }

        //[TestMethod()]
        //public void CreateTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DeleteTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void EditTest()
        //{
        //    Assert.Fail();
        //}
    }
}
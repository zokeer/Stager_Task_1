using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task_1.Subnet_Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_1.Models;

namespace Task_1.Subnet_Model.Service.Tests
{
    [TestClass()]
    public class SubnetCoverageManagerTests
    {
        [TestMethod()]
        public void GetCoverage_OneCoversAnother_Success()
        {
            var large = new Subnet("large", "10.0.0.0/24");
            var small = new Subnet("small", "10.0.0.0/30");
            var result = SubnetCoverageManager.GetCoverage(new List<Subnet>() { large, small });
            var expected = new Dictionary<Subnet, List<Subnet>>()
            {
                {small, new List<Subnet>() { small } },
                {large, new List<Subnet>() {large, small } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }

        }

        [TestMethod()]
        public void GetCoverage_OneCoversAnotherTwo_Success()
        {
            var large = new Subnet("large", "10.0.0.0/24");
            var small = new Subnet("small", "10.0.0.0/30");
            var small_2 = new Subnet("small2", "10.0.0.128/30");
            var result = SubnetCoverageManager.GetCoverage(new List<Subnet>() { large, small, small_2 });
            var expected = new Dictionary<Subnet, List<Subnet>>()
            {
                {small_2, new List<Subnet>() { small_2 } },
                {small, new List<Subnet>() { small } },
                {large, new List<Subnet>() {large, small, small_2 } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }

        }

        [TestMethod()]
        public void GetCoverage_TwoCoverages_Success()
        {
            var large_1 = new Subnet("large1", "10.0.0.0/24");
            var small_1 = new Subnet("small1", "10.0.0.0/30");
            var large_2 = new Subnet("large2", "198.0.0.0/24");
            var small_2 = new Subnet("small2", "198.0.0.0/30");
            var result = SubnetCoverageManager.GetCoverage(new List<Subnet>() { large_1, small_1, large_2, small_2 });
            var expected = new Dictionary<Subnet, List<Subnet>>()
            {
                {small_1, new List<Subnet>() { small_1 } },
                {small_2, new List<Subnet>() { small_2 } },
                {large_1, new List<Subnet>() { large_1, small_1 } },
                {large_2, new List<Subnet>() { large_2, small_2 } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }
        }

        [TestMethod()]
        public void GetCoverage_TransitiveCoverage_OneCoversAll()
        {
            var large = new Subnet("large", "10.0.0.0/24");
            var small = new Subnet("small", "10.0.0.0/28");
            var smallest = new Subnet("smallest", "10.0.0.0/30");
            var result = SubnetCoverageManager.GetCoverage(new List<Subnet>() { large, small, smallest });
            var expected = new Dictionary<Subnet, List<Subnet>>()
            {
                {small, new List<Subnet>() { small, smallest } },
                {smallest, new List<Subnet>() { smallest } },
                {large, new List<Subnet>() {large, small, smallest } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }
        }

        [TestMethod()]
        public void GetCoverage_SameNetwork_ChoseBiggerAtRandom()
        {
            var large_1 = new Subnet("large_1", "10.0.0.0/24");
            var large_2 = new Subnet("large_2", "10.0.0.0/24");
            var result = SubnetCoverageManager.GetCoverage(new List<Subnet>() { large_1, large_2 });
            var expected = new Dictionary<Subnet, List<Subnet>>()
            {
                {large_1, new List<Subnet>() { large_1, large_2 } },
                {large_2, new List<Subnet>() { large_2 } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }
        }
    }
}
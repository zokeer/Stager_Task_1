﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task_1.Models;

namespace Task_1.DomainModel.Service.Tests
{
    [TestClass]
    public class SubnetCoverageManagerTests
    {
        [TestMethod]
        public void GetMinimalCoverage_OneCoversAnother_Success()
        {
            var large = new Subnet("large", "10.0.0.0/24");
            var small = new Subnet("small", "10.0.0.0/30");
            var result = SubnetCoverageManager.GetMinimalCoverage(new List<Subnet> { large, small });
            var expected = new Dictionary<Subnet, List<Subnet>>
            {
                {large, new List<Subnet> {large, small } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }

        }

        [TestMethod]
        public void GetMinimalCoverage_OneCoversAnotherTwo_Success()
        {
            var large = new Subnet("large", "10.0.0.0/24");
            var small = new Subnet("small", "10.0.0.0/30");
            var small_2 = new Subnet("small2", "10.0.0.128/30");
            var result = SubnetCoverageManager.GetMinimalCoverage(new List<Subnet> { large, small, small_2 });
            var expected = new Dictionary<Subnet, List<Subnet>>
            {
                {large, new List<Subnet> {large, small, small_2 } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }

        }

        [TestMethod]
        public void GetMinimalCoverage_TwoCoverages_Success()
        {
            var large_1 = new Subnet("large1", "10.0.0.0/24");
            var small_1 = new Subnet("small1", "10.0.0.0/30");
            var large_2 = new Subnet("large2", "198.0.0.0/24");
            var small_2 = new Subnet("small2", "198.0.0.0/30");
            var result = SubnetCoverageManager.GetMinimalCoverage(new List<Subnet> { large_1, small_1, large_2, small_2 });
            var expected = new Dictionary<Subnet, List<Subnet>>
            {
                {large_1, new List<Subnet> { large_1, small_1 } },
                {large_2, new List<Subnet> { large_2, small_2 } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }
        }

        [TestMethod]
        public void GetMinimalCoverage_TransitiveCoverage_OneCoversAll()
        {
            var large = new Subnet("large", "10.0.0.0/24");
            var small = new Subnet("small", "10.0.0.0/28");
            var smallest = new Subnet("smallest", "10.0.0.0/30");
            var result = SubnetCoverageManager.GetMinimalCoverage(new List<Subnet> { large, small, smallest });
            var expected = new Dictionary<Subnet, List<Subnet>>
            {
                {large, new List<Subnet> {large, small, smallest } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }
        }

        [TestMethod]
        public void GetMinimalCoverage_SameNetwork_CoverBoth()
        {
            var large_1 = new Subnet("large_1", "10.0.0.0/24");
            var large_2 = new Subnet("large_2", "10.0.0.0/24");
            var result = SubnetCoverageManager.GetMinimalCoverage(new List<Subnet> { large_1, large_2 });
            var expected = new Dictionary<Subnet, List<Subnet>>
            {
                {large_1, new List<Subnet> { large_1, large_2 } }
            };
            foreach (var key in expected.Keys)
            {
                CollectionAssert.AreEquivalent(expected[key], result[key]);
            }
        }
    }
}
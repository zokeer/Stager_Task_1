using DomainModel.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DomainModel.Repository.Tests
{
    [TestFixture]
    public class DBRepositoryTests
    {
        private string _right_connection_string;

        [Test]
        public void DBRepository()
        {
            Assert.DoesNotThrow(() => new DBRepository(_right_connection_string));
        }

        [Test]
        public void Create()
        {
            Assert.Fail();
        }

        [Test]
        public void Delete()
        {
            Assert.Fail();
        }

        [Test]
        public void Edit()
        {
            Assert.Fail();
        }

        [Test]
        public void Get()
        {
            Assert.Fail();
        }
    }
}
using DomainModel.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Models;
using NUnit.Framework;

namespace DomainModel.Repository.Tests
{
    [TestFixture]
    public class DBRepositoryTests
    {
        private readonly string _rightConnectionString;
        private readonly string _wrongConnectionString;

        public DBRepositoryTests()
        {
            _rightConnectionString = @"Data Source=TESLA\DEV1;Initial Catalog=Study;
                                    Integrated Security=True;Integrated Security=True;
                                    Pooling=False;";
            _wrongConnectionString = "wrong";
            const string sqlExpression = @"if not exists (select * from sysobjects where name='Test' and xtype='U')
                                        create table Test (
                                            id nvarchar(255) not null,
                                            network nvarchar(32) not null
                                        )";

            using (var connection = new SqlConnection(_rightConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlExpression, connection))
                    command.ExecuteNonQuery();
            }
        }

        #region ConstructorTests
        [Test]
        public void DBRepository_RightConnectionString_Success()
        {
            Assert.DoesNotThrow(() => new DBRepository(_rightConnectionString));
        }

        [Test]
        public void DBRepository_WrongConnectionString_Fail()
        {
            Assert.Throws<ArgumentException>(() => new DBRepository(_wrongConnectionString));
        }
#endregion
        [Test]
        public void Create_ValidData_Success()
        {
            RecreateTestTable();
            var id = "new_id";
            var raw_subnet = "192.10.10.0/22";
            var repo = new DBRepository(_rightConnectionString);
            var expected = new List<Subnet>()
            {
                new Subnet(id, raw_subnet)
            };
            repo.Create(id, raw_subnet);
            TestRepositoryContent(repo, expected);

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

        private bool TestRepositoryContent(IRepository repository, List<Subnet> expected)
        {
            var subnets = repository.Get();
            CollectionAssert.AreEquivalent(expected, subnets);
        }
        private void RecreateTestTable()
        {
            const string sqlExpression = @"drop table Test;
                                        create table Test (
                                            id nvarchar(255) not null,
                                            network nvarchar(32) not null
                                        )";

            using (var connection = new SqlConnection(_rightConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlExpression, connection))
                    command.ExecuteNonQuery();
            }
        }
    }
}
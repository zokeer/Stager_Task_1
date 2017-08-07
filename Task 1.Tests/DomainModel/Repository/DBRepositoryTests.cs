using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DomainModel.Models;
using NUnit.Framework;

namespace DomainModel.Repository.Tests
{
    [TestFixture]
    public class DBRepositoryTests
    {
        private readonly string _rightConnectionString;
        private readonly string _wrongConnectionString;
        private readonly string _tableName;

        public DBRepositoryTests()
        {
            _rightConnectionString = @"Data Source=TESLA\DEV1;Initial Catalog=Study;
                                    Integrated Security=True;Integrated Security=True;
                                    Pooling=False;";
            _wrongConnectionString = "wrong";
            _tableName = "Test";
            var sql_expression = $@"if not exists (select * from sysobjects where name='{_tableName}' and xtype='U')
                                        create table {_tableName} (
                                            id nvarchar(255) not null,
                                            network nvarchar(32) not null
                                        )";

            using (var connection = new SqlConnection(_rightConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql_expression, connection))
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
        public void DBRepository_WrongConnectionString_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new DBRepository(_wrongConnectionString));
        }

        [Test]
        public void DBRepository_NullTableName_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new DBRepository(_rightConnectionString, null));
        }

        [Test]
        public void DBRepository_NullConnectionString_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new DBRepository(null, "fjdij"));
        }
        #endregion
        #region CreateTests
        [Test]
        public void Create_ValidData_Success()
        {
            RecreateTestTable();
            var id = "new_id";
            var raw_subnet = "192.10.10.0/22";
            var repo = new DBRepository(_rightConnectionString, _tableName);
            var expected = new List<Subnet>
            {
                new Subnet(id, raw_subnet)
            };
            repo.Create(id, raw_subnet);
            TestRepositoryContent(repo, expected);

        }

        [Test]
        public void Create_NullId_ThrowsException()
        {
            var repo = new DBRepository(_rightConnectionString, _tableName);
            Assert.Throws<ArgumentNullException>(() => repo.Create(null, "doesn't matter what"));
        }

        [Test]
        public void Create_NullSubnet_ThrowsException()
        {
            var repo = new DBRepository(_rightConnectionString, _tableName);
            Assert.Throws<ArgumentNullException>(() => repo.Create("some_id", null));
        }
        #endregion
        #region DeleteTests
        [Test]
        public void Delete_ValidData_Success()
        {
            RecreateTestTable();
            var id = "new_id";
            var raw_subnet = "192.10.10.0/22";
            var repo = new DBRepository(_rightConnectionString, _tableName);
            repo.Create(id, raw_subnet);
            repo.Delete(id);
            TestRepositoryContent(repo, new List<Subnet>());

        }

        [Test]
        public void Delete_NullId_ThrowsException()
        {
            var repo = new DBRepository(_rightConnectionString, _tableName);
            Assert.Throws<ArgumentNullException>(() => repo.Delete(null));
        }
        #endregion
        #region  EditTests
        [Test]
        public void Edit_ValidData_Success()
        {
            RecreateTestTable();
            var old_id = "old_id";
            var raw_subnet = "192.10.10.0/22";
            var repo = new DBRepository(_rightConnectionString, _tableName);
            repo.Create(old_id, raw_subnet);
            var new_id = "new_id";
            var expected = new List<Subnet>
            {
                new Subnet(new_id, raw_subnet)
            };

            repo.Edit(old_id, new_id, raw_subnet);
            TestRepositoryContent(repo, expected);

        }

        [Test]
        public void Edit_NullOldId_ThrowsException()
        {
            var repo = new DBRepository(_rightConnectionString, _tableName);
            Assert.Throws<ArgumentNullException>(() => repo.Edit(null, "doesn't matter what", "doesn't matter what"));
        }

        [Test]
        public void Edit_NullNewId_ThrowsException()
        {
            var repo = new DBRepository(_rightConnectionString, _tableName);
            Assert.Throws<ArgumentNullException>(() => repo.Edit("doesn't matter what", null, "doesn't matter what"));
        }

        [Test]
        public void Edit_NullSubnet_ThrowsException()
        {
            var repo = new DBRepository(_rightConnectionString, _tableName);
            Assert.Throws<ArgumentNullException>(() => repo.Edit("some_id", "some_new_id", null));
        }

        #endregion
        private void TestRepositoryContent(IRepository repository, List<Subnet> expected)
        {
            var subnets = repository.Get();
            CollectionAssert.AreEquivalent(expected, subnets);
        }
        private void RecreateTestTable()
        {
            var sql_expression = $@"drop table {_tableName};
                                        create table {_tableName} (
                                            id nvarchar(255) not null,
                                            network nvarchar(32) not null
                                        )";

            using (var connection = new SqlConnection(_rightConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql_expression, connection))
                    command.ExecuteNonQuery();
            }
        }
    }
}
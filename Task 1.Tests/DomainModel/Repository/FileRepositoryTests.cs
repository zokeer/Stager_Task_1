using System.Collections.Generic;
using System.IO;
using System.Linq;
using DomainModel.Models;
using DomainModel.Repository;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Task_1.DomainModel.Repository.Tests
{
    [TestFixture]
    public class FileRepositoryTests
    {
        private readonly string _rigthFilePath = Path.GetDirectoryName(typeof(FileRepositoryTests).Assembly.Location) + "\\rigth_test.txt";
        private readonly string _notExistingFile = "not_existing_file_test.txt";
        private readonly string _wrongDataFile = Path.GetDirectoryName(typeof(FileRepositoryTests).Assembly.Location) + "\\wrong_data_test.txt";
        private readonly string _defaultData = "[\"1,192.168.0.0/23\", \"2,192.168.0.0/24\"]";
        #region ConstuctorTests
        [Test]
        public void FileRepository_RightFile_NoExceptions()
        {
            Assert.DoesNotThrow(() => new FileRepository(_rigthFilePath));
        }
        
        [Test]
        public void FileRepository_NotExistingFile_Exception()
        {
            Assert.Throws(typeof(FileNotFoundException),
                () => new FileRepository(_notExistingFile));
        }

        [Test]
        public void FileRepository_WrongDataFile_Exception()
        {
            Assert.Throws(typeof(JsonSerializationException),
                () => new FileRepository(_wrongDataFile));
        }
        #endregion
        #region GetDataTests
        [Test]
        public void GetData_RigthData_Success()
        {
            var expected_list = new List<Subnet>
            {
                new Subnet("1", "192.168.0.0/23"),
                new Subnet("2", "192.168.0.0/24")
            };
            var actual_list = new FileRepository(_rigthFilePath).Get();

            CollectionAssert.AreEquivalent(expected_list, actual_list);
        }

        #endregion
        #region EditingFileTests
        [Test]
        public void Create_RigthData_SubnetsUpdated()
        {
            File.WriteAllText(_rigthFilePath, _defaultData);

            var test_repository = new FileRepository(_rigthFilePath);
            var expected_list = test_repository.Get();

            test_repository.Create("new", "10.0.0.0/30");
            var new_list = test_repository.Get();
            CollectionAssert.AreEquivalent(expected_list, new_list);

            File.WriteAllText(_rigthFilePath, _defaultData);
        }

        [Test]
        public void Delete_RigthData_SubnetsUpdated()
        {
            File.WriteAllText(_rigthFilePath, _defaultData);

            var test_repository = new FileRepository(_rigthFilePath);
            var expected_list = test_repository.Get();

            test_repository.Delete("1");
            expected_list = expected_list.Where(subnet => subnet.Id != "1").ToList();
            var new_list = test_repository.Get();
            CollectionAssert.AreEquivalent(expected_list, new_list);

            File.WriteAllText(_rigthFilePath, _defaultData);
        }
        #endregion
    }
}

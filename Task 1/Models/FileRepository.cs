using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Task_1.Models
{
    public class FileRepository : IRepository
    {
        public readonly string repository_path;

        public FileRepository(string repository_path)
        {
            this.repository_path = repository_path;
        }

        public void Create(string data)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(string id, string data)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Subnet> GetData()
        {
            //var data = File.ReadAllText(repository_path);
            //return JsonConvert.DeserializeObject<IEnumerable<Subnet>>(data);
            return new List<Subnet>() { new Subnet("23,22.22.22.22/12"), new Subnet("1,1.1.1.1/1"), new Subnet("2,2.2.2.2/2") };
        }
    }
}
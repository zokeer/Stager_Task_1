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
        private readonly string repository_path;

        public FileRepository(string repository_path)
        {
            this.repository_path = repository_path;
        }

        public void Create(string id, string raw_subnet)
        {
            var subnets = GetData();
            subnets.Add(new Subnet(id, raw_subnet));
            File.WriteAllText(repository_path,
                JsonConvert.SerializeObject(
                    subnets.Select(subnet => $"{subnet.Id},{subnet.Network.Network}/{subnet.Network.Cidr}")
                    )
                );

        }

        public void Delete(string id)
        {
            var subnets = GetData();
            subnets.Remove(subnets.Find(subnet => subnet.Id == id));
            File.WriteAllText(repository_path,
                JsonConvert.SerializeObject(
                    subnets.Select(subnet => $"{subnet.Id},{subnet.Network.Network}/{subnet.Network.Cidr}"))
                    );
        }

        public void Edit(string id, string raw_subnet)
        {
            Delete(id);
            Create(id, raw_subnet);
        }

        public List<Subnet> GetData()
        {
            var data = File.ReadAllText(repository_path);
            return JsonConvert.DeserializeObject<IEnumerable<string>>(data)
                .Select(raw_data => new Subnet(raw_data.Split(',')[0], raw_data.Split(',')[1])).ToList();
        }
    }
}
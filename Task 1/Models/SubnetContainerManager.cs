using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Task_1.Models
{
    public class SubnetContainerManager
    {
        private IRepository _repository;
        private SubnetContainer _subnetContainer;

        public SubnetContainerManager(IRepository repository)
        {
            _repository = repository;
            _subnetContainer = new SubnetContainer();
            _subnetContainer.Subnets = repository.GetData();
        }

        public IEnumerable<Subnet> Get()
        {
            return _subnetContainer.Subnets;
        }

        public void Create(string raw_subnet)
        {
            var new_subnet = new Subnet(raw_subnet);
            foreach (var subnet in _subnetContainer.Subnets)
            {
                if (subnet.Id == new_subnet.Id)
                    throw new InvalidOperationException();
                throw new NotImplementedException();
            }
        }

        public void Delete(string id)
        {
            try
            {
                _subnetContainer.Subnets.ToList<Subnet>().Remove(_subnetContainer.Subnets.Single(subnet => subnet.Id == id));
                _repository.Delete(id);
            }
            catch(Exception)
            { return; };
        }

        public void Edit(string id, string raw_subnet)
        {
            throw new NotImplementedException();
        }
    }
}
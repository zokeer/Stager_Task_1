using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Create(string id, string raw_subnet)
        {
            var new_subnet = new Subnet(id, raw_subnet);
            if (SubnetValidator.IsValidAddress(raw_subnet) 
                && SubnetValidator.IsValidMask(raw_subnet) 
                && !SubnetValidator.ContainsId(_subnetContainer, id))
            {
                _subnetContainer.Subnets.Add(new_subnet);
                _repository.Create(id, raw_subnet);
            }
        }

        public void Delete(string id)
        {
            _subnetContainer.Subnets = _subnetContainer.Subnets.Where(subnet => subnet.Id != id).ToList();
            _repository.Delete(id);
        }

        public void Edit(string id, string raw_subnet)
        {
            Delete(id);
            Create(id, raw_subnet);
        }
    }
}
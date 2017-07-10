using System.Net;
using LukeSkywalker.IPNetwork;

namespace Task_1.Models
{
    public class Subnet
    {
        public string Id { get; }
        public IPNetwork Network { get; }

        public Subnet(string id, string raw_subnet)
        {
            Id = id;
            Network = IPNetwork.Parse(raw_subnet);
        }
    }
}
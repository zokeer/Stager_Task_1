using System.Net;
using LukeSkywalker.IPNetwork;

namespace Task_1.Models
{
    /// <summary>
    /// Класс представления подсетей.
    /// </summary>
    public class Subnet
    {
        public string Id { get; }
        public IPNetwork Network { get; }

        public Subnet(string id, string raw_subnet)
        {
            Id = id;
            Network = IPNetwork.Parse(raw_subnet);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (!(other is Subnet))
                return false;
            Subnet other_subnet = (Subnet)other;
            return Id == other_subnet.Id && Network == other_subnet.Network;
        }
    }
}
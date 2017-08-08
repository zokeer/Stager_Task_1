using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;

namespace Task_1.ASMX
{
    public static class SubnetSerializer
    {
        public static SeriablizableSubnet SerializeSubnet(Subnet subnet)
        {
            return new SeriablizableSubnet
            {
                Id = subnet.Id,
                Address = subnet.Network.Network.ToString(),
                Mask = subnet.Network.Cidr.ToString()
            };

        }

        public static SeriablizableSubnet[] SerializeList(List<Subnet> subnets)
        {
            return subnets
                .Select(SerializeSubnet)
                .ToArray();
        }

        public static Dictionary<SeriablizableSubnet, SeriablizableSubnet[]> SerializeDictionary(
            Dictionary<Subnet, List<Subnet>> dictionary)
        {
            return dictionary.ToDictionary(pair => SerializeSubnet(pair.Key), pair => SerializeList(pair.Value));
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;

namespace Task_1.ASMX
{
    /// <summary>
    /// Класс предоставляет методы для сериализации Subnet в различном виде:
    /// экземпляр класса Subnet, список экземпляров класса Subnet, словарь, устроенный по принципу
    /// ключ - Subnet, значение - список Subnet.
    /// </summary>
    public static class SubnetSerializer
    {
        /// <summary>
        /// Сериализует экземпляр класса Subnet.
        /// </summary>
        /// <param name="subnet">Экземпляр класса Subnet</param>
        /// <returns>
        /// Сериализованное представление переданного экземпляра
        /// в виде нового экземпляра класса SerializableSubnet
        /// </returns>
        public static SeriablizableSubnet SerializeSubnet(Subnet subnet)
        {
            return new SeriablizableSubnet
            {
                Id = subnet.Id,
                Address = subnet.Network.Network.ToString(),
                Mask = subnet.Network.Cidr.ToString()
            };
        }

        /// <summary>
        /// Сериализует список экземпляров класса Subnet.
        /// </summary>
        /// <param name="subnets">Список экземпляров класса Subnet.</param>
        /// <returns>
        /// Сериализованное представление списка в виде 
        /// массива новых экземпляров класса SerializableSubnet.
        /// </returns>
        public static SeriablizableSubnet[] SerializeList(List<Subnet> subnets)
        {
            return subnets
                .Select(SerializeSubnet)
                .ToArray();
        }

        /// <summary>
        /// Сериализует словарь вида: ключ - Subnet, значение - Список Subnet.
        /// </summary>
        /// <param name="dictionary">Словарь вида: ключ - Subnet, значение - Список Subnet.</param>
        /// <returns>
        /// Сериализованное представление словаря в виде нового словаря, где
        /// ключ - SerializableSubnet, значение - массив SerializableSubnet.
        /// </returns>
        public static Dictionary<SeriablizableSubnet, SeriablizableSubnet[]> SerializeDictionary(
            Dictionary<Subnet, List<Subnet>> dictionary)
        {
            return dictionary.ToDictionary(pair => SerializeSubnet(pair.Key), pair => SerializeList(pair.Value));
        }
    }
}
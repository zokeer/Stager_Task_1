using System;
using LukeSkywalker.IPNetwork;

namespace Task_1.Models
{
    /// <summary>
    /// Класс представления подсетей.
    /// </summary>
    public class Subnet : IComparable
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

        /// <summary>
        /// Проверяет, покрывает ли эта подсеть данную.
        /// Передаёт ответственность методу Contains у класса IPNetwork.
        /// </summary>
        /// <param name="candidate_subnet">Предположительно меньшая подсеть.</param>
        /// <returns>True/False: Покрывает ли.</returns>
        public bool IsCovering(Subnet candidate_subnet)
        {
            return IPNetwork.Contains(Network, candidate_subnet.Network);
        }

        /// <summary>
        /// Сравнение определено для сортировки. Если одна сеть покрывает другую, то она считается "больше".
        /// Если никакая из сетей не покрывает другую, считаем их "несравнимыми".
        /// </summary>
        /// <param name="obj">Другая сеть.</param>
        /// <returns>Возвращаемое значение согласовано с требованиями IComparable</returns>
        public int CompareTo(object obj)
        {
            Subnet another = (Subnet)obj;
            if (IsCovering(another))
                return 1;
            if (another.IsCovering(this))
                return -1;
            return 0;
        }
    }
}
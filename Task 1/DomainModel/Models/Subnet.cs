using System;
using LukeSkywalker.IPNetwork;
using static DomainModel.Service.SubnetValidator;

namespace DomainModel.Models
{
    /// <summary>
    /// Класс представления подсетей.
    /// </summary>
    public class Subnet : IComparable
    {
        public string Id { get; }
        public IPNetwork Network { get; }

        private readonly string _errorMessage = @"{0} и {1} не корректные данные для создания Subnet.
                                                   Идентификатор должен быть длины не более 255 символов,
                                                   а подсеть соотвествовать требованиям IPv4 сети.";

        /// <summary>
        /// Конструктор создания Subnet. Перед созданием экзепляра проверяет ID на соотвествование
        /// требованиям о длине (не более 255 символов) и на соотвествие подсети требованиям IPv4 сети.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="raw_subnet">Подсеть с маской</param>
        public Subnet(string id, string raw_subnet)
        {
            raw_subnet = raw_subnet.Trim();
            id = id.Trim();
            if (!IsValidArguments(id, raw_subnet))
                throw new ArgumentException(string.Format(_errorMessage, id, raw_subnet));
            Id = id;
            Network = IPNetwork.Parse(raw_subnet);
        }
        
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Subnet равна другой Subnet если у них одинаковый ID и Маскированный Адрес Сети.
        /// </summary>
        /// <param name="other">Другая Сеть.</param>
        /// <returns>True/False: Равны ли подсети.</returns>
        public override bool Equals(object other)
        {
            var other_subnet = other as Subnet;
            if (other_subnet == null)
                return false;
            return Id == other_subnet.Id && Network == other_subnet.Network;
        }

        /// <summary>
        /// Проверяет, покрывает ли эта подсеть данную.
        /// Передаёт ответственность методу Contains у класса IPNetwork.
        /// </summary>
        /// <param name="candidate_subnet">Предположительно меньшая подсеть.</param>
        /// <returns>True/False: Покрывает ли эта подсеть данную.</returns>
        public bool IsCovering(Subnet candidate_subnet)
        {
            return IPNetwork.Contains(Network, candidate_subnet.Network);
        }

        /// <summary>
        /// Если одна сеть покрывает другую, то она считается "больше".
        /// Если никакая из сетей не покрывает другую, они "несравнимы".
        /// </summary>
        /// <param name="obj">Другая сеть.</param>
        /// <returns>Возвращаемое значение согласовано с требованиями IComparable</returns>
        public int CompareTo(object obj)
        {
            var another = (Subnet)obj;
            if (IsCovering(another))
                return 1;
            if (another.IsCovering(this))
                return -1;
            return 0;
        }
        
        private static bool IsValidArguments(string id, string raw_subnet)
        {
            return IsValidMask(raw_subnet).LogInfo == LogInfo.NoErrors &&
                   IsValidAddress(raw_subnet).LogInfo == LogInfo.NoErrors &&
                   IsValidId(id).LogInfo == LogInfo.NoErrors;
        }
    }
}
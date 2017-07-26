using System;
using LukeSkywalker.IPNetwork;
using DomainModel.Repository;

namespace DomainModel.Service
{
    /// <summary>
    /// Статический класс, хранящий методы для валидации данных, поступающих от пользователя.
    /// </summary>
    public static class SubnetValidator
    {
        /// <summary>
        /// Проверяет адрес подсети на соответствие виду d[dd].d[dd].d[dd].d[dd]/d[d].
        /// Проверку осущесвляет передачей ответсвенности методу Parse класса IPNetwork.
        /// </summary>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns>True/False: является ли адрес верным.</returns>
        public static bool IsValidAddress(string raw_subnet)
        {
            try
            {
                var address = raw_subnet.Split('/')[0];
                var possible_address = IPNetwork.Parse(address);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
    }
        /// <summary>
        /// Проверяет маску на соответствие виду d[d].
        /// Проверку осущесвляет передачей ответсвенности методу Parse класса IPNetwork.
        /// </summary>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns>True/False: является ли адрес верным.</returns>
        public static bool IsValidMask(string raw_subnet)
        {
            try
            {
                var mask = raw_subnet.Split('/')[1];
                if (string.IsNullOrEmpty(mask) || mask.Length > 2)
                    throw new Exception();
                var possible_subnet = IPNetwork.Parse(raw_subnet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет ID на уникальность в наборе, также на соответствение требованиям о длине [0..255].
        /// </summary>
        /// <param name="repository">Репозиторий подсетей.</param>
        /// <param name="id">ID, который нужно проверить.</param>
        /// <returns>True/False: является ли ID верным.</returns>
        public static bool IsValidId(IRepository repository, string id)
        {
            return !repository.Get().Exists(subnet => subnet.Id == id)
                && id.Length <= 255
                && !string.IsNullOrEmpty(id);
        }

        /// <summary>
        /// Проверяет соответствение требованиям о длине [0..255].
        /// </summary>
        /// <param name="id">ID, который нужно проверить.</param>
        /// <returns>True/False: является ли ID верным.</returns>
        public static bool IsValidId(string id)
        {
            return id.Length <= 255
                && !string.IsNullOrEmpty(id);
        }
    }
}
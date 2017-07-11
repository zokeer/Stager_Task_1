using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LukeSkywalker.IPNetwork;

namespace Task_1.Models
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
                if (mask.Length < 0 && mask.Length > 2)
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
        /// <param name="subnet_container">Контейнер подсетей.</param>
        /// <param name="id">ID, который нужно проверить.</param>
        /// <returns>True/False: является ли ID верным.</returns>
        public static bool isValidId(IRepository repository, string id)
        {
            return !repository.Get().Exists(subnet => subnet.Id == id)
                && id.Length <= 255
                && !string.IsNullOrEmpty(id);
        }
    }
}
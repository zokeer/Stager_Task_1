using System;
using System.Net;
using DomainModel.Models;
using LukeSkywalker.IPNetwork;
using DomainModel.Repository;
using Microsoft.Ajax.Utilities;

namespace DomainModel.Service
{
    /// <summary>
    /// Статический класс, хранящий методы для валидации данных, поступающих от пользователя.
    /// </summary>
    public static class SubnetValidator
    {
        /// <summary>
        /// Проверяет адрес подсети на соответствие виду d[dd].d[dd].d[dd].d[dd].
        /// Проверку осущесвляет передачей ответсвенности методу Parse класса IPAddress.
        /// </summary>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns>Информация о прохождении операции.</returns>
        public static ValidationLog IsValidAddress(string raw_subnet)
        {
            try
            {
                var address = raw_subnet.Split('/')[0];
                var possible_address = IPAddress.Parse(address);
                return new ValidationLog(SubnetField.Address, LogInfo.NoErrors);
            }
            catch (Exception)
            {
                return new ValidationLog(SubnetField.Address, LogInfo.Invalid);
            }
    }
        /// <summary>
        /// Проверяет маску на соответствие виду d[d].
        /// Проверку осущесвляет передачей ответсвенности методу Parse класса IPNetwork.
        /// </summary>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns>Информация о прохождении операции.</returns>
        public static ValidationLog IsValidMask(string raw_subnet)
        {
            try
            {
                var mask = raw_subnet.Split('/')[1];
                if (string.IsNullOrEmpty(mask) || mask.Length > 2)
                    throw new Exception();
                var possible_subnet = IPNetwork.Parse(raw_subnet);
                return new ValidationLog(SubnetField.Mask, LogInfo.NoErrors);
            }
            catch (Exception)
            {
                return new ValidationLog(SubnetField.Mask, LogInfo.Invalid);
            }
        }

        /// <summary>
        /// Проверяет ID на уникальность в наборе, также на соответствение требованиям о длине [0..255].
        /// </summary>
        /// <param name="repository">Репозиторий подсетей.</param>
        /// <param name="id">ID, который нужно проверить.</param>
        /// <returns>Информация о прохождении операции.</returns>
        public static ValidationLog IsValidId(IRepository repository, string id)
        {
            var log = IsValidId(id);
            if (log.LogInfo != LogInfo.NoErrors)
                return log;
            return repository.Get().Exists(subnet => subnet.Id == id) ?
                new ValidationLog(SubnetField.Id, LogInfo.NotUnique) :
                new ValidationLog(SubnetField.Id, LogInfo.NotExists);
        }

        /// <summary>
        /// Проверяет соответствение требованиям о длине [0..255].
        /// </summary>
        /// <param name="id">ID, который нужно проверить.</param>
        /// <returns>Информация о прохождении операции.</returns>
        public static ValidationLog IsValidId(string id)
        {
            if (id.Length <= 255 && !string.IsNullOrEmpty(id))
                return new ValidationLog(SubnetField.Id, LogInfo.NoErrors);
            return new ValidationLog(SubnetField.Id, LogInfo.Invalid);
        }
    }
}
using System;
using System.Net;
using DomainModel.Models;
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
            if (raw_subnet.IsNullOrWhiteSpace())
                return new ValidationLog(SubnetField.Address, LogInfo.Invalid);

            IPAddress test_address;
            var raw_address = raw_subnet.Split('/')[0];
            if (string.IsNullOrEmpty(raw_address) || raw_address.Length > 15)
                return new ValidationLog(SubnetField.Address, LogInfo.Invalid);

            return IPAddress.TryParse(raw_address, out test_address) ? 
                new ValidationLog(SubnetField.Address, LogInfo.NoErrors) : 
                new ValidationLog(SubnetField.Address, LogInfo.Invalid);
        }

        /// <summary>
        /// Проверяет маску на соответствие виду d[d].
        /// Проверку осущесвляет передачей ответсвенности методу Parse класса IPNetwork.
        /// </summary>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns>Информация о прохождении операции.</returns>
        public static ValidationLog IsValidMask(string raw_subnet)
        {
            if (raw_subnet.IsNullOrWhiteSpace())
                return new ValidationLog(SubnetField.Mask, LogInfo.Invalid);

            var mask = raw_subnet.Split('/')[1];
            if (string.IsNullOrEmpty(mask) || mask.Length > 2)
                return new ValidationLog(SubnetField.Mask, LogInfo.Invalid);

            int int_mask;
            if (!int.TryParse(mask, out int_mask))
                return new ValidationLog(SubnetField.Mask, LogInfo.Invalid);

            if (int_mask < 0 || int_mask > 32)
                return new ValidationLog(SubnetField.Mask, LogInfo.Invalid);
            return new ValidationLog(SubnetField.Mask, LogInfo.NoErrors);
        }

        /// <summary>
        /// Проверяет ID на уникальность в наборе, также на соответствение требованиям о длине [0..255].
        /// </summary>
        /// <param name="repository">Репозиторий подсетей.</param>
        /// <param name="id">ID, который нужно проверить.</param>
        /// <returns>Информация о прохождении операции.</returns>
        public static ValidationLog IsValidId(IRepository repository, string id)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository), "Не может быть null.");

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
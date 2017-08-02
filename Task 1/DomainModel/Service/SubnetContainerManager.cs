using System;
using DomainModel.Models;
using DomainModel.Repository;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace DomainModel.Service
{
    /// <summary>
    /// Сервис для работы с контейнером подсетей SubnetContainer.
    /// </summary>
    public class SubnetContainerManager
    {
        /// <summary>
        /// Репозиторий, реализующий методы работы с хранилищем данных.
        /// </summary>
        private readonly IRepository _repository;

        /// <summary>
        /// Конструктор инициализирует репозиторий, контейнер подсетей и наполняет его данными из репозитория.
        /// </summary>
        /// <param name="repository">Репозиторий, реализующий методы работы с хранилищем данных.</param>
        public SubnetContainerManager(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository), "Не может быть null.");
            _repository = repository;
        }

        /// <summary>
        /// Возвращает список подсетей из контейнера.
        /// </summary>
        /// <returns>Список подсетей.</returns>
        public List<Subnet> Get()
        {
            return _repository.Get();
        }

        /// <summary>
        /// Проверяет данные от пользователя на валидность. Если данные верны, то добавляет новую подсеть
        /// в контейнер подсетей и вызывает метод добавления подсети у репозитория.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns> Информация об успехе операции создания.</returns>
        public ValidationLog Create(string id, string raw_subnet)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");

            var id_log = SubnetValidator.IsValidId(_repository, id);
            if (id_log.LogInfo != LogInfo.NotExists)
                return id_log;
            var address_log = SubnetValidator.IsValidAddress(raw_subnet);
            if (address_log.LogInfo != LogInfo.NoErrors)
                return address_log;
            var mask_log = SubnetValidator.IsValidMask(raw_subnet);
            if (mask_log.LogInfo != LogInfo.NoErrors)
                return mask_log;
            _repository.Create(id, raw_subnet);
            return new ValidationLog(SubnetField.Everything, LogInfo.NoErrors);

        }

        /// <summary>
        /// Удаляет подсеть из контейнера подсетей и вызывает метод удаления подсети у репозитория.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить.</param>
        /// <returns> Информация об успехе операции удаления.</returns>
        public ValidationLog Delete(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Не может быть null.");

            var id_log = SubnetValidator.IsValidId(_repository, id);
            if (id_log.LogInfo != LogInfo.NotUnique)
                return id_log;
            _repository.Delete(id);
            return new ValidationLog(SubnetField.Everything, LogInfo.NoErrors);
        }

        /// <summary>
        /// Изменяет сеть по old_id на сеть (new_id, raw_subnet).
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить.</param>
        /// <param name="new_id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns> Информация об успехе операции изменения.</returns>
        public ValidationLog Edit(string old_id, string new_id, string raw_subnet)
        {
            if (old_id == null)
                throw new ArgumentNullException(nameof(old_id), "Не может быть null.");
            if (new_id == null)
                throw new ArgumentNullException(nameof(new_id), "Не может быть null.");
            if (raw_subnet == null)
                throw new ArgumentNullException(nameof(raw_subnet), "Не может быть null.");
            //Старый идентификатор должен существовать.
            var old_id_log = SubnetValidator.IsValidId(_repository, old_id);
            if (old_id_log.LogInfo != LogInfo.NotUnique)
                return old_id_log;
            //Новый идентификатор либо равен старому, либо уникален.
            var new_id_log = SubnetValidator.IsValidId(_repository, new_id);
            if (new_id_log.LogInfo != LogInfo.NotExists && new_id != old_id)
                return new_id_log;
            //Адрес должен быть верен.
            var address_log = SubnetValidator.IsValidAddress(raw_subnet);
            if (address_log.LogInfo != LogInfo.NoErrors)
                return address_log;
            //Маска должна быть верна.
            var mask_log = SubnetValidator.IsValidMask(raw_subnet);
            if (mask_log.LogInfo != LogInfo.NoErrors)
                return mask_log;

            _repository.Edit(old_id, new_id, raw_subnet);
            return new ValidationLog(SubnetField.Everything, LogInfo.NoErrors);
        }
    }
}
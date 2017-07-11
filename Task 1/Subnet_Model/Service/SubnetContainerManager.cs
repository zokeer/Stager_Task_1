using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_1.Models
{
    /// <summary>
    /// Сервис для работы с контейнером подсетей SubnetContainer.
    /// </summary>
    public class SubnetContainerManager
    {
        /// <summary>
        /// Репозиторий, реализующий методы работы с хранилищем данных.
        /// </summary>
        private IRepository _repository;
        /// <summary>
        /// Контейнер подсетей. Основное виртуальное хранилище данных.
        /// </summary>
        private SubnetContainer _subnetContainer;

        /// <summary>
        /// Конструктор инициализирует репозиторий, контейнер подсетей и наполняет его данными из репозитория.
        /// </summary>
        /// <param name="repository">Репозиторий, реализующий методы работы с хранилищем данных.</param>
        public SubnetContainerManager(IRepository repository)
        {
            _repository = repository;
            _subnetContainer = new SubnetContainer();
            _subnetContainer.Subnets = repository.GetData();
        }

        /// <summary>
        /// Возвращает список подсетей из контейнера.
        /// </summary>
        /// <returns>Список подсетей.</returns>
        public IEnumerable<Subnet> Get()
        {
            return _subnetContainer.Subnets;
        }

        /// <summary>
        /// Проверяет данные от пользователя на валидность. Если данные верны, то добавляет новую подсеть
        /// в контейнер подсетей и вызывает метод добавления подсети у репозитория.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        public void Create(string id, string raw_subnet)
        {
            if (SubnetValidator.IsValidAddress(raw_subnet) 
                && SubnetValidator.IsValidMask(raw_subnet) 
                && SubnetValidator.isValidId(_subnetContainer, id))
            {
                var new_subnet = new Subnet(id, raw_subnet);
                _subnetContainer.Subnets.Add(new_subnet);
                _repository.Create(id, raw_subnet);
            }
        }

        /// <summary>
        /// Удаляет подсеть из контейнера подсетей и вызывает метод удаления подсети у репозитория.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить.</param>
        public void Delete(string id)
        {
            _subnetContainer.Subnets = _subnetContainer.Subnets.Where(subnet => subnet.Id != id).ToList();
            _repository.Delete(id);
        }

        /// <summary>
        /// "Ленивый" Edit: вызывает удаление старой подсети и добавление новой.
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить</param>
        /// <param name="new_id">ID новой подсети</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        public void Edit(string old_id, string new_id, string raw_subnet)
        {
            Delete(old_id);
            Create(new_id, raw_subnet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Services;
using DomainModel.Models;
using DomainModel.Repository;
using DomainModel.Service;
using Microsoft.Ajax.Utilities;

namespace Task_1.ASMX
{
    /// <summary>
    /// Класс Веб-Сервиса, описывающий работу с сервисом подсетей.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SubnetContainerWebService : WebService
    {
        /// <summary>
        /// Сервис подсетей, хранит список подсетей и методы работы с ними.
        /// </summary>
        private readonly SubnetContainerManager _subnetContainerManager;

        /// <summary>
        /// Конструктор инициализирует сервис подсетей и репозиторий.
        /// </summary>
        public SubnetContainerWebService()
        {
            _subnetContainerManager = new SubnetContainerManager(
                new DBRepository(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()
                )
            );
        }

        /// <summary>
        /// Получает у сервиса подсетей полный список подсетей и преобразует в сериализуемый класс.
        /// </summary>
        /// <returns>Сериализованное представление массива Subnet.</returns>
        [WebMethod(Description = "Получить массив подсетей.")]
        public SeriablizableSubnet[] Get()
        {
            return SubnetSerializer.SerializeList(_subnetContainerManager.Get());
        }

        /// <summary>
        /// Вызывает у сервиса подсетей создание новой подсети.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns> Информация об успехе операции создания.</returns>
        [WebMethod(Description = "Добавить Подсеть.")]
        public ValidationLog Create(string id, string raw_subnet)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), "Идентификатор новой подсети не может быть null.");
            if (raw_subnet.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(raw_subnet), @"Аргумент должен быть маскированным
                                                                    адресом подсети, но был получен null.");

            return _subnetContainerManager.Create(id, raw_subnet);
        }

        /// <summary>
        /// Вызывает у сервиса подсетей удаление подсети с указанным идентификатором.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить.</param>
        /// <returns> Информация об успехе операции удаления.</returns>
        [WebMethod(Description = "Удалить Подсеть.")]
        public ValidationLog Delete(string id)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), @"Идентификатор подсети, которую нужно удалить не может быть null.
                                                              Выберите существующий идентификатор подсети.");

            return _subnetContainerManager.Delete(id);
        }

        /// <summary>
        /// Вызывает у сервиса подсетей изменение подсети с указанным идентификатором.
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить.</param>
        /// <param name="new_id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns> Информация об успехе операции изменения.</returns>
        [WebMethod(Description = "Изменить Подсеть.")]
        public ValidationLog Edit(string old_id, string new_id, string raw_subnet)
        {
            if (old_id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(old_id), @"Идентификатор подсети, которую нужно изменить
                                                                не может быть null.
                                                                Выберите существующий идентификатор подсети.");
            if (new_id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(new_id), @"Новый идентификатор подсети не может быть null.
                                                                 Это либо новый уникальный идентификатор,
                                                                 либо тот, который изменяется.");
            if (raw_subnet.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(raw_subnet), @"Аргумент должен быть маскированным
                                                                    адресом подсети, но был получен null.");

            return _subnetContainerManager.Edit(old_id, new_id, raw_subnet);
        }

        /// <summary>
        /// Вызывает у SubnetCoverageManager метод создания минимального покрытия подсетей,
        /// которые предоставляет сервис подсетей.
        /// </summary>
        /// <returns>
        /// Словарь минимального покрытия, устроенный по принципу:
        /// ключ - покрывающая подсеть
        /// значение - список подсетей, которые покрывает подсеть-ключ.
        /// </returns>
        [WebMethod(Description = "Получить минимальное покрытие подсетей.")]
        public Dictionary<SeriablizableSubnet, SeriablizableSubnet[]> GetCoverage()
        {
            return SubnetSerializer.SerializeDictionary(
                SubnetCoverageManager.GetMinimalCoverage(
                    _subnetContainerManager.Get()
                )
            );

        }
    }
}


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using DomainModel.Models;
using DomainModel.Repository;
using DomainModel.Service;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Task_1.ASMX
{
    /// <summary>
    /// Класс Веб-Сервиса, описывающий работу с сервисом подсетей.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class SubnetContainerWebService : WebService
    {
        /// <summary>
        /// Сервис подсетей, хранит список подсетей и методы работы с ними.
        /// </summary>
        private readonly SubnetContainerManager _subnetContainerManager;

        /// <summary>
        /// Функция преобразования строковых представлений полей
        /// экземпляра класса Subnet в привычный вид: ddd.ddd.ddd.ddd/d[d].
        /// </summary>
        private readonly Func<string, string, string> _normalizeSubnetName;

        /// <summary>
        /// Конструктор инициализирует сервис подсетей и репозиторий.
        /// </summary>
        public SubnetContainerWebService()
        {
            _normalizeSubnetName = (address, mask) => $"{address}/{mask}";
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
        [WebMethod(Description = "Получить Список Подсетей."), ScriptMethod(UseHttpGet = true)]
        public List<Subnet> Get()
        { 
            return _subnetContainerManager.Get();
        }

        /// <summary>
        /// Вызывает у сервиса подсетей создание новой подсети.
        /// </summary>
        /// <param name="id">ID новой подсети.</param>
        /// <param name="address">Строковое представление адреса подсети.</param>
        /// <param name="mask">Маска подсети.</param>
        /// <returns> Информация об успехе операции создания.</returns>
        [WebMethod(Description = "Добавить Подсеть.")]
        [ScriptMethod]
        public string Create(string id, string address, string mask)
        {
            if (mask == null)
                throw new ArgumentNullException(nameof(mask), "Маска подсети должна быть числом от 0 до 32, но был получен null.");
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), "Идентификатор новой подсети не может быть null.");
            if (address.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(address), @"Аргумент должен быть маскированным
                                                                    адресом подсети, но был получен null.");

            return _subnetContainerManager.Create(id, _normalizeSubnetName(address, mask)).ToString();

        }

        /// <summary>
        /// Вызывает у сервиса подсетей удаление подсети с указанным идентификатором.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить.</param>
        /// <returns> Информация об успехе операции удаления.</returns>
        [WebMethod(Description = "Удалить Подсеть.")]
        [ScriptMethod]
        public string Delete(string id)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), @"Идентификатор подсети, которую нужно удалить не может быть null.
                                                              Выберите существующий идентификатор подсети.");

            return _subnetContainerManager.Delete(id).ToString();
        }

        /// <summary>
        /// Вызывает у сервиса подсетей изменение подсети с указанным идентификатором.
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить.</param>
        /// <param name="new_id">ID новой подсети.</param>
        /// <param name="raw_subnet">Строковое представление подсети.</param>
        /// <returns> Информация об успехе операции изменения.</returns>
        [WebMethod(Description = "Изменить Подсеть.")]
        [ScriptMethod]
        public string Edit(string old_id, string new_id, string raw_subnet)
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

            return _subnetContainerManager.Edit(old_id, new_id, raw_subnet).ToString();
        }

        ///// <summary>
        ///// Вызывает у SubnetCoverageManager метод создания минимального покрытия подсетей,
        ///// которые предоставляет сервис подсетей.
        ///// </summary>
        ///// <returns>
        ///// Словарь минимального покрытия, устроенный по принципу:
        ///// ключ - покрывающая подсеть
        ///// значение - список подсетей, которые покрывает подсеть-ключ.
        ///// </returns>
        //[WebMethod(Description = "Получить минимальное покрытие подсетей.")]
        //public Dictionary<Subnet, List<Subnet>> GetCoverage()
        //{
        //    return SubnetCoverageManager.GetMinimalCoverage(
        //        _subnetContainerManager.Get()
        //    );
        //}
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using DomainModel.Models;
using DomainModel.Repository;
using DomainModel.Service;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Task_1.Controllers
{
    public class SubnetApiController : ApiController
    {
        /// <summary>
        /// Функция преобразования строковых представлений полей
        /// экземпляра класса Subnet в привычный вид: ddd.ddd.ddd.ddd/d[d].
        /// </summary>
        private readonly Func<string, string, string> _normalizeSubnetName;

        /// <summary>
        /// Сервис для работы с контейнером экземпляров класса Subnet.
        /// </summary>
        private readonly SubnetContainerManager _subnetContainerManager;

        /// <summary>
        /// Конструктор инициализириует функцию преобразования и сервис.
        /// </summary>
        public SubnetApiController(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository), @"Аргумент должен быть экземпляром класса,
                                                                    реализующем интерфейс IRepository,
                                                                    но был получен null.");

            _normalizeSubnetName = (address, mask) => $"{address}/{mask}";
            _subnetContainerManager = new SubnetContainerManager(repository);
        }
        
        public string Get()
        {
            return JsonConvert.SerializeObject(_subnetContainerManager.Get());
        }


        public ValidationLog CreateSubnet(string id, string address, string mask)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), "Идентификатор новой подсети не может быть null.");
            if (address.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(address), @"Аргумент должен быть 
                                                                    адресом подсети, но был получен null.");
            if (mask.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(mask), @"Аргумент должен быть 
                                                                    маской подсети, но был получен null.");

            return _subnetContainerManager.Create(id, _normalizeSubnetName(address, mask));

        }
        
        public ValidationLog DeleteSubnet(string id)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), @"Идентификатор подсети, которую нужно удалить не может быть null.
                                                              Выберите существующий идентификатор подсети.");
            return _subnetContainerManager.Delete(id);
        }

        public ValidationLog EditSubnet(string old_id, string new_id, string address, string mask)
        {
            if (old_id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(old_id), @"Идентификатор подсети, которую нужно изменить
                                                                не может быть null.
                                                                Выберите существующий идентификатор подсети.");
            if (new_id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(new_id), @"Новый идентификатор подсети не может быть null.
                                                                 Это либо новый уникальный идентификатор,
                                                                 либо тот, который изменяется.");
            if (address.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(address), @"Аргумент должен быть 
                                                                    адресом подсети, но был получен null.");
            if (mask.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(mask), @"Аргумент должен быть 
                                                                    маской подсети, но был получен null.");

            return _subnetContainerManager.Edit(old_id, new_id, _normalizeSubnetName(address, mask));
            
        }

        public Dictionary<Subnet, List<Subnet>> GetCoverage()
        {
            return SubnetCoverageManager.GetMinimalCoverage(_subnetContainerManager.Get());
        }
    }
}
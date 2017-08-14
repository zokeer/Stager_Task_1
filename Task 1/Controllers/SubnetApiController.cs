using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DomainModel.Repository;
using DomainModel.Service;
using Newtonsoft.Json;

namespace Task_1.Controllers
{
    public class SubnetApiController : ApiController
    {
        /// <summary>
        /// Сервис для работы с контейнером экземпляров класса Subnet.
        /// </summary>
        private readonly SubnetContainerManager _subnetContainerManager;

        /// <summary>
        /// Функция преобразования строковых представлений полей
        /// экземпляра класса Subnet в привычный вид: ddd.ddd.ddd.ddd/d[d].
        /// </summary>
        private readonly Func<string, string, string> _normalizeSubnetName;

        /// <summary>
        /// Конструктор инициализириует функцию преобразования и сервис.
        /// </summary>
        public SubnetApiController(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository), @"Репозиторий должен быть экземпляром класса,
                                                                    реализующем IRepository, но был получен null.");
            _normalizeSubnetName = (address, mask) => $"{address}/{mask}";
            _subnetContainerManager = new SubnetContainerManager(repository);
        }

        /// <summary>
        /// Функция получает все Subnet, которые предоставляет сервис.
        /// </summary>
        /// <returns>Json-представление маскированных подсетей и дополнительные поля для DataTables.</returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<object> Get()
        {
            var subnets = _subnetContainerManager.Get();
            IEnumerable<object> masked_subnets = subnets.Select((subnet, index) => new
            {
                subnet.Id,
                MaskedAddress = _normalizeSubnetName(subnet.Network.Network.ToString(),
                    subnet.Network.Cidr.ToString()),
                DT_RowId = subnet.Id

            });
            return masked_subnets;
        }

        /// <summary>
        /// Передаёт ответственность за создание новой подсети сервису.
        /// </summary>
        /// <param name="json_data">
        /// Данные из формы формата:
        /// {
        ///     "id": value,
        ///     "address": value,
        ///     "mask": value
        ///}
        /// Из данных будет создан новый экземпляр класса Subnet.
        /// </param>
        /// <returns>Информация о прохождении операции в формате JSON.</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public string CreateSubnet([FromBody] string json_data)
        {
            if (string.IsNullOrEmpty(json_data))
                throw new ArgumentException(@"Данные из формы должны представлять
                                            json-сериализованный объект,
                                            но был получен null.", nameof(json_data));

            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_data);
            return _subnetContainerManager.Create(data["id"], _normalizeSubnetName(data["address"], data["mask"]))
                .ToString();
        }


        /// <summary>
        /// Передаёт ответственность за удаление подсети сервису.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить</param>
        /// <returns>Информация о прохождении операции в формате JSON.</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public string DeleteSubnet([FromBody] string id)
        {
            return _subnetContainerManager.Delete(id).ToString();
        }

        /// <summary>
        /// Передаёт ответственность за изменение подсети сервису.
        /// </summary>
        /// <param name="json_data">
        /// Данные из формы формата:
        ///{
        ///    old_id: value,
        ///    new_id: value,
        ///    address: value,
        ///    mask: value
        ///}
        /// По переданным данным будет изменён соответствующий экземпляр Subnet из контейнера.
        /// </param>
        /// <returns>Информация о прохождении операции в формате JSON.</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.HttpGet]
        public string EditSubnet([FromBody] string json_data)
        {
            if (string.IsNullOrEmpty(json_data))
                throw new ArgumentException(@"Данные из формы должны представлять
                                            json-сериализованный объект,
                                            но был получен null.", nameof(json_data));

            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_data);
            return _subnetContainerManager.Edit(data["old_id"], data["new_id"],
                _normalizeSubnetName(data["address"], data["mask"])).ToString();
        }

        /// <summary>
        /// Получает минимальное покрытие подсетей из класса-сервиса.
        /// </summary>
        /// <returns>
        /// Json-представление маскированных подсетей минимального покрытия
        /// и дополнительные поля для DataTables.
        /// </returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<object> GetCoverage()
        {
            var coverage = SubnetCoverageManager.GetMinimalCoverage(_subnetContainerManager.Get());
            var json_acceptable_list = new List<object>();

            foreach (var key in coverage.Keys)
            {
                json_acceptable_list.Add(new
                {
                    KeyId = key.Id,
                    KeyMaskedAddress = _normalizeSubnetName(key.Network.Network.ToString(),
                        key.Network.Cidr.ToString()),
                    Children = coverage[key].Select(subnet => subnet.Id)
                });
            }
            return json_acceptable_list;
        }
    }
}
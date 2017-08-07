using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DomainModel.Models;
using DomainModel.Repository;
using DomainModel.Service;
using Microsoft.Ajax.Utilities;
using Task_1.ASMX;

namespace Task_1.Controllers
{
    /// <summary>
    /// Контроллер для работы с сервисом подсетей.
    /// </summary>
    public class SubnetContainerController : Controller
    {
        /// <summary>
        /// Сервис для работы с контейнером экземпляров класса Subnet.
        /// </summary>
        private readonly SubnetContainerWebService _subnetContainerWebService;

        /// <summary>
        /// Функция преобразования строковых представлений полей
        /// экземпляра класса Subnet в привычный вид: ddd.ddd.ddd.ddd/d[d].
        /// </summary>
        private readonly Func<string, string, string> _normalizeSubnetName;
        
        /// <summary>
        /// Конструктор инициализириует функцию преобразования и сервис.
        /// </summary>
        public SubnetContainerController()
        {
            _normalizeSubnetName = (address, mask) => $"{address}/{mask}";
            _subnetContainerWebService = new SubnetContainerWebService();
        }        

        /// <summary>
        /// Метод отображает базовый вид страницы.
        /// </summary>
        /// <returns>Базовый вид страницы.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Функция получает все Subnet, которые предоставляет сервис.
        /// </summary>
        /// <returns>Json-представление маскированных подсетей и дополнительные поля для DataTables.</returns>
        [HttpGet]
        public ActionResult Get()
        {
            var seriablizable_subnets =_subnetContainerWebService.Get();
            var subnets = TransformAsmxToModel(seriablizable_subnets);
            IEnumerable<object> masked_subnets = subnets.Select((subnet, index) => new
            {
                subnet.Id,
                MaskedAddress = _normalizeSubnetName(subnet.Network.Network.ToString(),
                    subnet.Network.Cidr.ToString()),
                DT_RowId = subnet.Id

            });
            var masked_subnets_array = masked_subnets.ToArray();
            return Json(new { data = masked_subnets_array,
                recordsTotal = masked_subnets_array.Length,
                recordsFiltered = masked_subnets_array.Length
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Передаёт ответственность за создание новой подсети сервису.
        /// </summary>
        /// <param name="id">ID новой подсети</param>
        /// <param name="address">Адрес новой подсети</param>
        /// <param name="mask">Маска новой подсети</param>
        /// <returns>Информация о прохождении операции в формате JSON.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSubnet(string id, string address, string mask)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), "Идентификатор новой подсети не может быть null.");
            if (address.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(address), @"Аргумент должен быть 
                                                                    адресом подсети, но был получен null.");
            if (mask.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(mask), @"Аргумент должен быть 
                                                                    маской подсети, но был получен null.");

            var log = _subnetContainerWebService.Create(id, _normalizeSubnetName(address, mask));
            return CreateJsonResult(log);
        }

        /// <summary>
        /// Передаёт ответственность за удаление подсети сервису.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить</param>
        /// <returns>Информация о прохождении операции в формате JSON.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSubnet(string id)
        {
            if (id.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(id), @"Идентификатор подсети, которую нужно удалить не может быть null.
                                                              Выберите существующий идентификатор подсети.");
            var log = _subnetContainerWebService.Delete(id);
            return CreateJsonResult(log);
        }

        /// <summary>
        /// Передаёт ответственность за изменение подсети сервису.
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить.</param>
        /// <param name="new_id">ID новой подсети.</param>
        /// <param name="address">Адрес новой подсети.</param>
        /// <param name="mask">Маска новой подсети.</param>
        /// <returns>Информация о прохождении операции в формате JSON.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubnet(string old_id, string new_id, string address, string mask)
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

            var log = _subnetContainerWebService.Edit(old_id, new_id, _normalizeSubnetName(address, mask));
            return CreateJsonResult(log);
        }

        ///// <summary>
        ///// Получает минимальное покрытие подсетей из класса-сервиса.
        ///// </summary>
        ///// <returns>
        ///// Json-представление маскированных подсетей минимального покрытия
        ///// и дополнительные поля для DataTables.
        ///// </returns>
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetCoverage()
        //{
        //    var coverage = SubnetCoverageManager.GetMinimalCoverage(_subnetContainerManager.Get());
        //    var json_acceptable_list = new List<object>();

        //    foreach (var key in coverage.Keys)
        //    {
        //        json_acceptable_list.Add(new
        //        {
        //            KeyId = key.Id,
        //            KeyMaskedAddress = _normalizeSubnetName(key.Network.Network.ToString(),
        //                key.Network.Cidr.ToString()),
        //            Children = coverage[key].Select(subnet => subnet.Id)
        //        });                
        //    }

        //    return Json(new
        //    {
        //        data = json_acceptable_list,
        //        recordsTotal = json_acceptable_list.Count,
        //        recordsFiltered = json_acceptable_list.Count
        //    }, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Преобразует экземпляр класса ValidationLog в JSON.
        /// </summary>
        /// <param name="log">Информация, которую нужно преобразовать.</param>
        /// <returns>Json-представление переданной информации.</returns>
        private JsonResult CreateJsonResult(ValidationLog log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), @"Аргумент, представляющий логирование выполненных операций не может быть null.
                                                                Передайте экземпляр класса ValidationLog.");

            return Json(log.ToString(), JsonRequestBehavior.AllowGet);
        }

        private List<Subnet> TransformAsmxToModel(SeriablizableSubnet[] asmx_subnets)
        {
            return asmx_subnets.Select(subnet => new Subnet(subnet.Id,
                _normalizeSubnetName(subnet.Address, subnet.Mask))).ToList();
        }
    }
}
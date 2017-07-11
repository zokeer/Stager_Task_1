using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Task_1.Models;

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
        private SubnetContainerManager _subnetContainerManager;

        /// <summary>
        /// Функция преобразования строковых предсталений полей экземпляра класса Subnet в привычный вид: ddd.ddd.ddd.ddd/d[d].
        /// </summary>
        private Func<string, string, string> _normalizeSubnetName;
        
        /// <summary>
        /// Конструктор инициализириует функцию преобразования и сервис.
        /// TODO: реализовать DI, не хардкодить путь.
        /// </summary>
        public SubnetContainerController()
        {
            var repository = new FileRepository(HostingEnvironment.ApplicationPhysicalPath + "test.txt");
            _normalizeSubnetName = (id, mask) => $"{id}/{mask}";
            _subnetContainerManager = new SubnetContainerManager(repository);
        }        

        /// <summary>
        /// Метод отображает базовый вид страницы.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Функция получает все Subnet, которые предоставляет сервис.
        /// </summary>
        /// <returns>Json-представление маскированных подсетей и дополнительные поля для DataTables</returns>
        [HttpGet]
        public ActionResult Get()
        {
            var subnets =_subnetContainerManager.Get();
            IEnumerable<object> masked_subnets = subnets.Select((subnet, index) => new
            {
                subnet.Id,
                MaskedAddress = _normalizeSubnetName(subnet.Network.Network.ToString(), subnet.Network.Cidr.ToString()),
                DT_RowId = subnet.Id

            });
            var subnets_amount = masked_subnets.ToArray().Length;
            return Json(new { data = masked_subnets,
                recordsTotal = subnets_amount,
                recordsFiltered = subnets_amount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Передаёт ответственность за создание новой подсети сервису.
        /// </summary>
        /// <param name="id">ID новой подсети</param>
        /// <param name="address">Адрес новой подсети</param>
        /// <param name="mask">Маска новой подсети</param>
        /// <returns>Json-представление маскированных подсетей и дополнительные поля для DataTables</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSubnet(string id, string address, string mask)
        {
            _subnetContainerManager.Create(id, address + '/' + mask);
            return Get();
        }

        /// <summary>
        /// Передаёт ответственность за удаление подсети сервису.
        /// </summary>
        /// <param name="id">ID сети, которую надо удалить</param>
        /// <returns>Json-представление маскированных подсетей и дополнительные поля для DataTables</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSubnet(string id)
        {
            _subnetContainerManager.Delete(id);
            return Get();
        }

        /// <summary>
        /// Передаёт ответственность за изменение подсети сервису.
        /// </summary>
        /// <param name="old_id">ID той сети, чьи параметры нужно изменить</param>
        /// <param name="new_id">ID новой подсети</param>
        /// <param name="address">Адрес новой подсети</param>
        /// <param name="mask">Маска новой подсети</param>
        /// <returns>Json-представление маскированных подсетей и дополнительные поля для DataTables</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubnet(string old_id, string new_id, string address, string mask)
        {
            _subnetContainerManager.Edit(old_id, new_id, address + '/' + mask);
            return Get();
        }
    }
}
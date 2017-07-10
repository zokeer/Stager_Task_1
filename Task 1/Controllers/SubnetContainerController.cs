using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Task_1.Models;

namespace Task_1.Controllers
{
    public class SubnetContainerController : Controller
    {
        private SubnetContainerManager _subnetContainerManager;
        private Func<string, string, string> _normalizeSubnetName;
        
        public SubnetContainerController()
        {
            var repository = new FileRepository(@"C:\Users\Дмитрий\Source\Repos\Stager_Task_1\Task 1\test.txt");
            _normalizeSubnetName = (id, mask) => $"{id}/{mask}";
            _subnetContainerManager = new SubnetContainerManager(repository);
        }        

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSubnet(string id, string address, string mask)
        {
            _subnetContainerManager.Create(id, address + '/' + mask);
            return Get();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSubnet(string id)
        {
            _subnetContainerManager.Delete(id);
            return Get();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubnet(string id, string address, string mask)
        {
            _subnetContainerManager.Edit(id, address + '/' + mask);
            return Get();
        }
    }
}
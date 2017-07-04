using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var repository = new FileRepository("placeholder");
            _normalizeSubnetName = (id, mask) => string.Format("{0}/{1}", id, mask);
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
            IEnumerable<object> masked_subnets = subnets.Select(subnet => new
            {
                Id = subnet.Id,
                MaskedAddress = _normalizeSubnetName(subnet.Address.ToString(), subnet.Mask.ToString())
            });

            return Json(new { data = masked_subnets }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSubnet(string id, string address, string mask)
        {
            _subnetContainerManager.Create(id + ',' + address + '/' + mask);
            return this.Get();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSubnet(string id)
        {
            _subnetContainerManager.Delete(id);
            return this.Get();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubnet(string id, string address, string mask)
        {
            _subnetContainerManager.Create(id + ',' + address + '/' + mask);
            return this.Get();
        }
    }
}
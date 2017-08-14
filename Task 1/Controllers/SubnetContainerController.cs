using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DomainModel.Models;
using Microsoft.Ajax.Utilities;
using Task_1.ASMX;

namespace Task_1.Controllers
{
    /// <summary>
    /// Контроллер для работы с веб-сервисом подсетей.
    /// </summary>
    public class SubnetContainerController : Controller
    {
        /// <summary>
        /// Метод отображает базовый вид страницы.
        /// </summary>
        /// <returns>Базовый вид страницы.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
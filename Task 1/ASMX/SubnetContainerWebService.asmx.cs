using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Autofac;
using System.Web.Services;
using DomainModel.Models;
using DomainModel.Repository;
using DomainModel.Service;
using WebGrease.Configuration;

namespace Task_1.ASMX
{
    /// <summary>
    /// Класс Веб-Сервиса, описывающий работу с Сервисом Подсетей.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SubnetContainerWebService : WebService
    {
        private readonly SubnetContainerManager _subnetContainerManager;

        public SubnetContainerWebService()
        {
            _subnetContainerManager = new SubnetContainerManager(
                new DBRepository(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()
                    )
                );
        }

        /// <summary>
        /// Получает у Сервиса подсетей полный список подсетей и преобразует в сериализуемый класс.
        /// </summary>
        /// <returns>Сериализованное представление массива Subnet.</returns>
        [WebMethod(Description = "Получить массив подсетей.")]
        public SeriablizableSubnet[] Get()
        {
            var subnets = _subnetContainerManager.Get();
            return subnets
                .Select(subnet => new SeriablizableSubnet
                {
                    Id = subnet.Id,
                    Address = subnet.Network.Network.ToString(),
                    Mask = subnet.Network.Cidr.ToString()
                })
                .ToArray();
        }

        [WebMethod(Description = "Добавить Подсеть.")]
        public ValidationLog Create(string id, string raw_subnet)
        {
            return _subnetContainerManager.Create(id, raw_subnet);
        }

        [WebMethod(Description = "Удалить Подсеть.")]
        public ValidationLog Delete(string id)
        {
            return _subnetContainerManager.Delete(id);
        }

        [WebMethod(Description = "Изменить Подсеть.")]
        public ValidationLog Edit(string old_id, string new_id, string raw_subnet)
        {
            return _subnetContainerManager.Edit(old_id, new_id, raw_subnet);
        }

    }
}

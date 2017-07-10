using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LukeSkywalker.IPNetwork;

namespace Task_1.Models
{
    public static class SubnetValidator
    {
        public static bool IsValidAddress(string raw_subnet)
        {
            try
            {
                var address = raw_subnet.Split('/')[0];
                var possible_address = IPNetwork.Parse(address);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
    }

        public static bool IsValidMask(string raw_subnet)
        {
            try
            {
                var mask = raw_subnet.Split('/')[1];
                if (mask.Length < 0 && mask.Length > 2)
                    throw new Exception();
                var possible_subnet = IPNetwork.Parse(raw_subnet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ContainsId(SubnetContainer subnet_container, string id)
        {
            return subnet_container.Subnets.Exists(subnet => subnet.Id == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task_1.Models;

namespace Task_1.Subnet_Model.Service
{
    public static class SubnetCoverageManager
    {
        private static Dictionary<Subnet, List<Subnet>> GetAllCovers(List<Subnet> sorted_subnet_container)
        {
            var coverage_dict = new Dictionary<Subnet, List<Subnet>>();
            foreach (var subnet in sorted_subnet_container)
                foreach (var other_subnet in sorted_subnet_container.Skip(sorted_subnet_container.IndexOf(subnet)))
                {
                    if (subnet.IsCovering(other_subnet))
                        if (coverage_dict.ContainsKey(subnet))
                        {
                            if (!coverage_dict[subnet].Contains(other_subnet))
                                coverage_dict[subnet].Add(other_subnet);
                        }
                        else
                            coverage_dict.Add(subnet, new List<Subnet>() { other_subnet });
                    else
                    {
                        if (coverage_dict.ContainsKey(other_subnet))
                        {
                            if (!coverage_dict[other_subnet].Contains(other_subnet))
                                coverage_dict[other_subnet].Add(other_subnet);
                        }
                        else
                            coverage_dict.Add(other_subnet, new List<Subnet>() { other_subnet });
                    }
                }
            return coverage_dict;
        }

        public static Dictionary<Subnet, List<Subnet>> GetCoverage(List<Subnet> subnet_container)
        {
            subnet_container.Sort();
            subnet_container.Reverse();
            return GetAllCovers(subnet_container);
        }

        //public static Dictionary<Subnet, List<Subnet>> GetMinimalCoverage(List<Subnet> subnet_container)
        //{
        //    var coverages = GetCoverage(subnet_container);
        //    var used_subnets = new List<Subnet>();
        //}
    }
}
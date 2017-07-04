using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace Task_1.Models
{
    public class Subnet
    {
        public string Id { get; set; }
        public IPAddress Address { get; set; }
        public IPAddress Mask { get; set; }

        public Subnet(string subnet_string)
        {
            var splitted_tuple = subnet_string.Split(',');
            Id = splitted_tuple[0];
            Address = IPAddress.Parse(splitted_tuple[1].Split('/')[0]);
            Mask = IPAddress.Parse(splitted_tuple[1].Split('/')[1]);
        }
    }
}
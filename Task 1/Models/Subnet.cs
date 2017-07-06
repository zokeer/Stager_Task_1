using System.Net;

namespace Task_1.Models
{
    public class Subnet
    {
        public string Id { get; }
        public IPAddress Address { get; }
        public IPAddress Mask { get; }

        public Subnet(string raw_subnet)
        {
            var splitted_token = raw_subnet.Split(',');
            Id = splitted_token[0];
            Address = IPAddress.Parse(splitted_token[1].Split('/')[0]);
            Mask = IPAddress.Parse(splitted_token[1].Split('/')[1]);
        }
    }
}
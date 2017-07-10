using System.Collections.Generic;

namespace Task_1.Models
{
    public interface IRepository
    {
        List<Subnet> GetData();
        void Create(string id, string raw_subnet);
        void Edit(string id, string raw_subnet);
        void Delete(string id);
    }
}

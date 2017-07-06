using System.Collections.Generic;

namespace Task_1.Models
{
    public interface IRepository
    {
        List<Subnet> GetData();
        void Create(string data);
        void Edit(string id, string data);
        void Delete(string id);
    }
}

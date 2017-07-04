using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1.Models
{
    public interface IRepository
    {
        IEnumerable<Subnet> GetData();
        void Create(string data);
        void Edit(string id, string data);
        void Delete(string id);
    }
}

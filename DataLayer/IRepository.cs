using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IRepository<T> :IDisposable
        where T:class
    {
        void add(T item);
        void addByIndex(T item, int Index);
        void deleteByIndex(T item, int Index);
        T GetPerson(int ID);
        IEnumerable<T> GetPersonList();
    }
}

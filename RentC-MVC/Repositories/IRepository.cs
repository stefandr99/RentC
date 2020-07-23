using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Util;

namespace RentC_MVC.Repositories
{
    public interface IRepository<T> where T : class
    {
        int register(T obj, DbConnection db);
        bool remove(int id, DbConnection db);
        List<T> list(int orderBy, string ascendent, DbConnection db);
        bool update(T obj, DbConnection db);
    }
}

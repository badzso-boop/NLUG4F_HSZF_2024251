using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Model
{
    public interface IProductDataProvider : IRepository<Product>
    {
        event EventHandler<ProductPersonEventArgs> ProductBelowCriticalLevel;
    }

    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T? GetById(int id);
        List<T> GetAll();
    }
}

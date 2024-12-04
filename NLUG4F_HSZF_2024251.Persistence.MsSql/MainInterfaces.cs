using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Model
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T? GetById(int id);
        List<T> GetAll();
    }

    public interface IJsonRead
    {
        HashSet<Person> People { get; set; }
        HashSet<Product> Products { get; set; }
        Fridge Fridge { get; set; }
        Pantry Pantry { get; set; }

        void SeedDatabase();
    }

    public interface IDataProvider
    {
        JsonRead JsonRead { get; set; }
        ProductDataProvider ProductDataProvider { get; set; }
        PersonDataProvider PersonDataProvider { get; set; }
        FridgeDataProvider FridgeDataProvider { get; set; }
        PantryDataProvider PantryDataProvider { get; set; }
    }
}

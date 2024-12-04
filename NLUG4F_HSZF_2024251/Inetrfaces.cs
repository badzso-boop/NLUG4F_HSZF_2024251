using NLUG4F_HSZF_2024251.Model;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251
{
    public interface IDataProvider<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }

    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IInputCollector
    {
        IRepository<Product> productData { get; set; }
        IRepository<Person> personData { get; set; }
        IRepository<Fridge> fridgeData { get; set; }
        IRepository<Pantry> pantryData { get; set; }
        Product CollectProductData();
        Person CollectPersonData();
        Pantry CollectPantryData();
        Fridge CollectFridgeData();
        decimal GetDecimalInput(string prompt);
        List<decimal> GetDecimalInputs(string prompt);
        int GetIntInput(string prompt);
        bool GetBooleanInput(string prompt);
        List<int> GetMultipleIdsInput(string prompt, IEnumerable<Product> products);
        List<int> GetMultipleIdsInputWithDecimal(string prompt, IEnumerable<Product> products);
        void UnifiedAdd(string entityType);
        void UnifiedUpdate(string entityType);
        void UnifiedDelete(string entityType);
    }
}

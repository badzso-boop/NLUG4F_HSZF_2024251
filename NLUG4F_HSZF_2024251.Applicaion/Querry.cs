using NLUG4F_HSZF_2024251.Model;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class Querry : IProductRepository
    {
        private readonly IProductDataProvider _productDataProvider;
        private readonly IRepository<Person> _personDataProvider;
        private readonly IRepository<Fridge> _fridgeDataProvider;
        private readonly IRepository<Pantry> _pantryDataProvider;
        public event EventHandler<LowStockProductListEventArgs> ProductsBelowCriticalLevel;
        public event EventHandler<LowStockProductListEventArgs> NotifyAllHouseholdMembers;
        public Querry(IProductDataProvider productDataProvider, IRepository<Person> PersonDataProvider, IRepository<Fridge> FridgeDataProvider, IRepository<Pantry> PantryDataProvider)
        {
            _productDataProvider = productDataProvider;
            _personDataProvider = PersonDataProvider;
            _fridgeDataProvider = FridgeDataProvider;
            _pantryDataProvider = PantryDataProvider;
        }

        public List<Product> GetAllStockProduct()
        {
            var fridge = _fridgeDataProvider.GetById(1);
            var pantry = _pantryDataProvider.GetById(1);
            List<Product> allProducts = new List<Product>();

            if (fridge != null || pantry != null)
            {
                var fridgeProducts = fridge?.Products?.ToList() ?? new List<Product>();
                var pantryProducts = pantry?.Products?.ToList() ?? new List<Product>();

                int maxCapacity = fridge.Capacity + pantry.Capacity;
                decimal CriticalCapacityThreshold = (decimal)maxCapacity * (decimal)0.1;

                allProducts.AddRange(fridgeProducts);
                allProducts.AddRange(pantryProducts);

                decimal totalQuantity = allProducts.Sum(product => product.Quantity);
                decimal remainingCapacity = maxCapacity - totalQuantity;

                if (remainingCapacity < CriticalCapacityThreshold)
                {
                    NotifyAllHouseholdMembers?.Invoke(this, new LowStockProductListEventArgs(allProducts));
                }
            }

            return allProducts;
        }

        public List<Product> GetLowStockItems()
        {
            var lowStockProducts = _productDataProvider.GetAll()?.Where(p => p.Quantity <= p.CriticalLevel).ToList() ?? new List<Product>();

            if (lowStockProducts.Count != 0)
            {
                ProductsBelowCriticalLevel?.Invoke(this, new LowStockProductListEventArgs(lowStockProducts));
            }

            return lowStockProducts;
        }

        public List<Product> GetExpiringSoon()
        {
            var soon = DateTime.Now.AddDays(30);
            var expiringProducts = _productDataProvider.GetAll()?.Where(p => p.BestBefore <= soon).ToList() ?? new List<Product>();

            return expiringProducts;
        }

        public bool ExportToTxt()
        {
            List<Product> productsInStock = _productDataProvider.GetAll()?.Where(p => p.Quantity > 0).ToList() ?? new List<Product>();

            if (productsInStock.Count == 0)
            {
                return false;
            }

            string currentDate = DateTime.Now.ToString("ddMMyyyy");
            string timestamp = DateTime.Now.ToString("HHmmss");

            string folderPath = Path.Combine(Environment.CurrentDirectory, currentDate);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"HouseholdRegisterExport_{timestamp}.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("List of products with stock information:");
                writer.WriteLine("--------------------------------------------------");
                foreach (var product in productsInStock)
                {
                    writer.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Store in fridge: {product.StoreInFridge}");
                }
            }

            return true;
        }
    }
}

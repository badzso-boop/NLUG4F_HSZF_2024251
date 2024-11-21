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
        private readonly HouseHoldDbContext _context;
        private ProductDataProvider _productDataProvider;
        private PersonDataProvider _personDataProvider;
        private FridgeDataProvider _fridgeDataProvider;
        private PantryDataProvider _pantryDataProvider;
        public event EventHandler<LowStockProductListEventArgs> ProductsBelowCriticalLevel;
        public event EventHandler<LowStockProductListEventArgs> NotifyAllHouseholdMembers;
        public Querry(HouseHoldDbContext context, ProductDataProvider productDataProvider, PersonDataProvider personDataProvider, FridgeDataProvider fridgeDataProvider, PantryDataProvider pantryDataProvider)
        {
            _context = context;
            _productDataProvider = productDataProvider;
            _personDataProvider = personDataProvider;
            _fridgeDataProvider = fridgeDataProvider;
            _pantryDataProvider = pantryDataProvider;
        }

        static void Kiir(List<Product> products)
        {
            Console.Clear();
            Console.WriteLine($"Number of products: {products.Count}");
            Console.WriteLine("Current products:");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                Console.WriteLine("----------------------");
            }
        }

        public void GetAllStockProduct()
        {
            var fridge = _fridgeDataProvider.GetById(1);
            var pantry = _pantryDataProvider.GetById(1);

            var fridgeProducts = fridge.Products?.ToList() ?? new List<Product>();
            var pantryProducts = pantry.Products?.ToList() ?? new List<Product>();

            int maxCapacity = fridge.Capacity + pantry.Capacity;
            decimal CriticalCapacityThreshold = (decimal)maxCapacity * (decimal)0.1;

            List<Product> allProducts = new List<Product>(fridgeProducts);
            allProducts.AddRange(pantryProducts);

            decimal totalQuantity = allProducts.Sum(product => product.Quantity);
            decimal remainingCapacity = maxCapacity - totalQuantity;

            Console.WriteLine($"Fennmaradó kapacitás: {remainingCapacity}");

            Kiir(allProducts);

            if (remainingCapacity < CriticalCapacityThreshold)
            {
                NotifyAllHouseholdMembers?.Invoke(this, new LowStockProductListEventArgs(allProducts));
            }
        }

        public void GetLowStockItems()
        {
            var lowStockProducts = _productDataProvider.GetAll()?.Where(p => p.Quantity <= p.CriticalLevel).ToList() ?? new List<Product>();

            if (lowStockProducts.Count == 0)
            {
                Console.WriteLine("No products are currently at or below the critical stock level.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"Number of products: {lowStockProducts.Count}");
                Console.WriteLine("Current products:");
                ProductsBelowCriticalLevel?.Invoke(this, new LowStockProductListEventArgs(lowStockProducts));
            }
        }

        public void GetExpiringSoon()
        {
            var soon = DateTime.Now.AddDays(30);
            var expiringProducts = _productDataProvider.GetAll()?.Where(p => p.BestBefore <= soon).ToList() ?? new List<Product>();

            if (expiringProducts.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("No products are expiring soon.");
            }
            else
            {
                Kiir(expiringProducts);
            }
        }

        public void ExportToTxt()
        {
            List<Product> productsInStock = _productDataProvider.GetAll()?.Where(p => p.Quantity > 0).ToList() ?? new List<Product>();

            if (productsInStock.Count == 0)
            {
                Console.WriteLine("Nincs elérhető termék raktáron az exportáláshoz.");
                return;
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
                writer.WriteLine("Termékek listája raktárkészlettel:");
                writer.WriteLine("--------------------------------------------------");
                foreach (var product in productsInStock)
                {
                    writer.WriteLine($"Termék ID: {product.Id}, Név: {product.Name}, Mennyiség: {product.Quantity}, Store in fridge: {product.StoreInFridge}");
                }
            }

            Console.WriteLine($"Exportálás sikeresen megtörtént: {filePath}");
        }
    }
}

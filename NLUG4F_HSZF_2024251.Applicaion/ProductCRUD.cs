using Microsoft.EntityFrameworkCore;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class ProductCRUD : CRUDActions<Product>
    {
        HouseHoldDbContext context { get; set; }
        ProductDataProvider productData { get; set; }
        PersonDataProvider personData { get; set; }
        public ProductCRUD(HouseHoldDbContext houseHoldDbContext, ProductDataProvider productData, PersonDataProvider personData)
        {
            this.context = houseHoldDbContext;
            this.productData = productData;
            this.personData = personData;
        }

        static void Kiir(List<Product> products)
        {
            Console.WriteLine($"Number of products: {products.Count}");
            Console.WriteLine("Current products:");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                Console.WriteLine("----------------------");
            }
        }

        public List<Product> WriteAll()
        {
            return productData.GetAll();
        }

        public void WriteOne(int productId)
        {
            try
            {
                Product productToWrite = productData.GetById(productId);
                Console.WriteLine($"Id: {productToWrite.Id}, Name: {productToWrite.Name}, Quantity: {productToWrite.Quantity}, Critical Level: {productToWrite.CriticalLevel}, Best Before: {productToWrite.BestBefore}");
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine($"Product with ID {productId} not found: {ex.Message}");
            }
        }

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("Adding a new product...");

            Console.Write("Enter the product name: ");
            string name = Console.ReadLine() ?? string.Empty;

            decimal quantity;
            while (true)
            {
                Console.Write("Enter the quantity (decimal number): ");
                string quantityInput = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(quantityInput, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal number for quantity.");
                }
            }

            decimal criticalLevel;
            while (true)
            {
                Console.Write("Enter the critical level (decimal number): ");
                string criticalLevelInput = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(criticalLevelInput, NumberStyles.Any, CultureInfo.InvariantCulture, out criticalLevel))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal number for critical level.");
                }
            }

            Console.Write("Store in fridge? (yes/no): ");
            string storeInFridgeInput = Console.ReadLine()?.ToLower();
            bool storeInFridge = storeInFridgeInput == "yes" || storeInFridgeInput == "y";


            Product newProduct = new Product
            {
                Name = name,
                Quantity = quantity,
                CriticalLevel = criticalLevel,
                BestBefore = DateTime.Now.AddDays(30),
                StoreInFridge = storeInFridge
            };

            try
            {
                if (storeInFridge)
                {
                    var fridgeCTX = context.Fridge;
                    var fridge = fridgeCTX.Include(f => f.Products).FirstOrDefault();

                    if (fridge != null)
                    {
                        decimal currentFridgeQuantity = fridge.Products.Sum(p => p.Quantity);

                        if (currentFridgeQuantity + newProduct.Quantity <= fridge.Capacity)
                        {
                            fridge.Products.Add(newProduct);
                            Console.WriteLine("Product added to fridge!");
                            productData.Add(newProduct);
                            Console.WriteLine("Product added successfully!");
                        }
                        else
                        {
                            throw new InvalidProductDataException("Not enough capacity in the fridge to store this product.");
                        }
                    }
                    else
                    {
                        throw new InvalidProductDataException("No fridge found to store the product.");
                    }
                }
                else
                {
                    var pantryCTX = context.Pantry;
                    var pantry = pantryCTX.Include(p => p.Products).FirstOrDefault();

                    if (pantry != null) 
                    {
                        decimal currentPantryQuantity = pantry.Products.Sum(p => p.Quantity);

                        if (currentPantryQuantity + newProduct.Quantity <= pantry.Capacity)
                        {
                            pantry.Products.Add(newProduct);
                            Console.WriteLine("Product added to pantry");
                            productData.Add(newProduct);
                            Console.WriteLine("Product added successfully!");
                        }
                        else
                        {
                            throw new InvalidProductDataException("Not enough capacity in the pantry to store this product.");
                        }
                    }
                    else
                    {
                        throw new InvalidProductDataException("No pantry found to store the product!");
                    }
                }
            }
            catch (InvalidProductDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine("Fetching all products...");

            var products = productData.GetAll();
            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Kiir(products);

            Product productToUpdate = new Product();
            while (true)
            {
                Console.Write("Enter the ID of the product you want to update: ");

                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                try
                {
                    productToUpdate = productData.GetById(productId);
                    break;
                }
                catch (ProductNotFoundException ex)
                {
                    Console.WriteLine($"Product with ID {productId} not found: {ex.Message}");
                }
            }

            Console.Write($"Enter new name for {productToUpdate.Name} (or press Enter to keep it the same): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) productToUpdate.Name = newName;

            decimal newQuantity;
            while (true)
            {
                Console.Write($"Enter new quantity for {productToUpdate.Quantity} (decimal number): ");
                string quantityInput = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(quantityInput, NumberStyles.Any, CultureInfo.InvariantCulture, out newQuantity))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal number for quantity.");
                }
            }
            productToUpdate.Quantity = newQuantity;

            decimal newCriticalLevel;
            while (true)
            {
                Console.Write($"Enter new critical level for {productToUpdate.CriticalLevel} (decimal number): ");
                string criticalLevelInput = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(criticalLevelInput, NumberStyles.Any, CultureInfo.InvariantCulture, out newCriticalLevel))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal number for critical level.");
                }
            }
            productToUpdate.CriticalLevel = newCriticalLevel;

            try
            {
                if (productToUpdate.StoreInFridge)
                {
                    var fridgeCTX = context.Fridge;
                    var fridge = fridgeCTX.Include(f => f.Products).FirstOrDefault();

                    if (fridge != null)
                    {
                        decimal currentFridgeQuantity = fridge.Products.Sum(p => p.Quantity);

                        if (currentFridgeQuantity + productToUpdate.Quantity <= fridge.Capacity)
                        {
                            productData.Update(productToUpdate);
                            Console.WriteLine("Product updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Not enough capacity in the fridge to store this product.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No fridge found to store the product.");
                    }
                }
                else
                {
                    var pantryCTX = context.Pantry;
                    var pantry = pantryCTX.Include(p => p.Products).FirstOrDefault();

                    if (pantry != null)
                    {
                        decimal currentPantryQuantity = pantry.Products.Sum(p => p.Quantity);

                        if (currentPantryQuantity + productToUpdate.Quantity <= pantry.Capacity)
                        {
                            productData.Update(productToUpdate);
                            Console.WriteLine("Product updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Not enough capacity in the pantry to store this product.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No pantry found to store the product!");
                    }
                }
            }
            catch (InvalidProductDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("Fetching all products...");

            var products = productData.GetAll();
            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Kiir(products);

            while (true)
            {
                Console.Write("Enter the ID of the product you want to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int productId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                try
                {
                    var product = productData.GetById(productId);
                    if (product != null)
                    {
                        var fridgeCTX = context.Fridge;
                        var fridge = fridgeCTX.Include(f => f.Products).FirstOrDefault();
                        if (fridge != null && fridge.Products.Contains(product))
                        {
                            fridge.Products.Remove(product);
                            Console.WriteLine("Product removed from fridge.");
                        }

                        productData.Delete(productId);
                        Console.WriteLine("Product deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }
                    break;
                }
                catch (ProductNotFoundException ex)
                {
                    Console.WriteLine($"Product with ID {productId} not found: {ex.Message}");
                }
            }
        }
        
    }
}

using Microsoft.EntityFrameworkCore;
using NLUG4F_HSZF_2024251.Model;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251
{
    public class InputCollector
    {
        public PersonDataProvider personData;
        public ProductDataProvider productData;
        public FridgeDataProvider fridgeData;
        public PantryDataProvider pantryData;
        public HouseHoldDbContext context;

        public InputCollector(HouseHoldDbContext houseHoldDbContext, PersonDataProvider InputPersonData, ProductDataProvider InputProductData, FridgeDataProvider InputFridgeData, PantryDataProvider InputPantryData)
        {
            this.context = houseHoldDbContext;
            personData = InputPersonData;
            productData = InputProductData;
            fridgeData = InputFridgeData;
            pantryData = InputPantryData;
        }

        public Product CollectProductData()
        {
            Console.Clear();
            Console.WriteLine("Adding a new product...");

            Console.Write("Enter the product name: ");
            string name = Console.ReadLine() ?? string.Empty;

            decimal quantity = GetDecimalInput("Enter the quantity (decimal number): ");
            decimal criticalLevel = GetDecimalInput("Enter the critical level (decimal number): ");
            bool storeInFridge = GetBooleanInput("Store in fridge? (yes/no): ");

            return new Product
            {
                Name = name,
                Quantity = quantity,
                CriticalLevel = criticalLevel,
                BestBefore = DateTime.Now.AddDays(30),
                StoreInFridge = storeInFridge
            };
        }

        public Person CollectPersonData()
        {
            Console.Clear();
            Console.WriteLine("Adding a new person...");

            Console.Write("Enter the person name: ");
            string name = Console.ReadLine() ?? string.Empty;

            bool responsibleForPurchase = GetBooleanInput("Responsible for purchase? (yes/no): ");
            var favoriteProductIds = GetMultipleIdsInput("Select favorite product IDs (-1 to finish): ", productData.GetAll());

            return new Person(name, responsibleForPurchase, favoriteProductIds);
        }

        public Pantry CollectPantryData()
        {
            Console.Clear();
            Console.WriteLine("Adding a new pantry...");

            int capacity = GetIntInput("Enter the pantry capacity: ");
            var productIds = GetMultipleIdsInput("Select product IDs to add to the pantry (-1 to finish): ", productData.GetAll());
            var selectedProducts = productData.GetAll().Where(p => productIds.Contains(p.Id)).ToList();

            return new Pantry(capacity, selectedProducts);
        }

        public Fridge CollectFridgeData()
        {
            Console.Clear();
            Console.WriteLine("Adding a new fridge...");

            int capacity = GetIntInput("Enter the fridge capacity: ");
            var productIds = GetMultipleIdsInput("Select product IDs to add to the fridge (-1 to finish): ", productData.GetAll());
            var selectedProducts = productData.GetAll().Where(p => productIds.Contains(p.Id)).ToList();

            return new Fridge(capacity, selectedProducts);
        }



        // Helper methods
        private decimal GetDecimalInput(string prompt)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    return value;
                Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            }
        }

        private int GetIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                    return value;
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }

        private bool GetBooleanInput(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.ToLower();
            return input == "yes" || input == "y";
        }

        private List<int> GetMultipleIdsInput(string prompt, IEnumerable<Product> products)
        {
            Console.WriteLine(prompt);
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}");
            }

            List<int> ids = new List<int>();
            int input;
            do
            {
                input = GetIntInput("Product ID (-1 to finish): ");
                if (input == -1) break;
                if (products.Any(p => p.Id == input))
                    ids.Add(input);
                else
                    Console.WriteLine("Invalid ID, please try again.");
            } while (true);

            return ids;
        }

        public void PrintAll<T>(List<T> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine($"--- {typeof(T).Name} ---");
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);

                    //Console.WriteLine($"Típus: {value?.GetType().Name}");

                    if (value is System.Collections.IEnumerable && value.GetType() != typeof(string))
                    {
                        Console.WriteLine($"{property.Name}:");
                        if (value is IEnumerable<int> intList)
                        {
                            // Ha int típusú lista, akkor foreach ciklussal kiírjuk
                            foreach (var subItem in intList)
                            {
                                Console.WriteLine($"- {subItem}");
                            }
                        }
                        else
                        {
                            // Ha nem int típusú lista, rekurzívan meghívjuk a PrintAll-t
                            var itemType = value.GetType().GetGenericArguments().FirstOrDefault();
                            if (itemType != null)
                            {
                                var printMethod = this.GetType().GetMethod("PrintAll").MakeGenericMethod(itemType);
                                printMethod.Invoke(this, new object[] { value });
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine($"{property.Name}: {value}");
                    }
                }
                Console.WriteLine("----------------------");
            }
        }

        // Universal Methods
        public void UnifiedAdd(string entityType)
        {
            switch (entityType.ToLower())
            {
                case "product":
                    Product product = CollectProductData();
                    try
                    {
                        if (product != null)
                        {
                            if (product.StoreInFridge)
                            {
                                var fridgeCTX = context.Fridge;
                                var fridgeToAdd = fridgeCTX.Include(f => f.Products).FirstOrDefault();

                                if (fridgeToAdd != null)
                                {
                                    decimal currentFridgeQuantity = fridgeToAdd.Products.Sum(p => p.Quantity);

                                    if (currentFridgeQuantity + product.Quantity <= fridgeToAdd.Capacity)
                                    {
                                        fridgeToAdd.Products.Add(product);
                                        Console.WriteLine("Product added to fridge!");
                                        productData.Add(product);
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
                                var pantryToAdd = pantryCTX.Include(p => p.Products).FirstOrDefault();

                                if (pantryToAdd != null)
                                {
                                    decimal currentPantryQuantity = pantryToAdd.Products.Sum(p => p.Quantity);

                                    if (currentPantryQuantity + product.Quantity <= pantryToAdd.Capacity)
                                    {
                                        pantryToAdd.Products.Add(product);
                                        Console.WriteLine("Product added to pantry!");
                                        productData.Add(product);
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
                        else
                        {
                            Console.WriteLine("Failed to collect product data.");
                        }
                    }
                    catch (InvalidProductDataException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "person":
                    Person person = CollectPersonData();
                    if (person != null)
                    {
                        personData.Add(person);
                        Console.WriteLine("Person added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to collect person data.");
                    }
                    break;

                case "pantry":
                    Pantry pantry = CollectPantryData();
                    if (pantry != null)
                    {
                        pantryData.Add(pantry);
                        Console.WriteLine("Pantry added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to collect pantry data.");
                    }
                    break;

                case "fridge":
                    Fridge fridge = CollectFridgeData();
                    if (fridge != null)
                    {
                        fridgeData.Add(fridge);
                        Console.WriteLine("Fridge added successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to collect fridge data.");
                    }
                    break;

                default:
                    Console.WriteLine("Unknown entity type.");
                    break;
            }
        }

        public void UnifiedUpdate(string entityType)
        {
            switch (entityType.ToLower())
            {
                case "product":
                    Console.Clear();
                    Console.WriteLine("Updating a product...");
                    var products = productData.GetAll();
                    PrintAll(products);

                    Console.Write("Enter the ID of the product you want to update: ");
                    int productId = GetIntInput("Product ID: ");
                    Product productToUpdate = productData.GetById(productId);

                    // Get updated product data
                    Console.Write($"Enter new name for {productToUpdate.Name} (or press Enter to keep it the same): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName)) productToUpdate.Name = newName;

                    decimal newQuantity = GetDecimalInput("Enter new quantity: ");
                    productToUpdate.Quantity = newQuantity;

                    decimal newCriticalLevel = GetDecimalInput("Enter new critical level: ");
                    productToUpdate.CriticalLevel = newCriticalLevel;

                    bool storeInFridge = GetBooleanInput("Store in fridge? (yes/no): ");
                    productToUpdate.StoreInFridge = storeInFridge;

                    productData.Update(productToUpdate);
                    Console.WriteLine("Product updated successfully!");
                    break;

                case "person":
                    Console.Clear();
                    Console.WriteLine("Updating a person...");
                    var people = personData.GetAll();
                    PrintAll(people);

                    Console.Write("Enter the ID of the person you want to update: ");
                    int personId = GetIntInput("Person ID: ");
                    Person personToUpdate = personData.GetById(personId);

                    // Get updated person data
                    Console.Write($"Enter new name for {personToUpdate.Name} (or press Enter to keep it the same): ");
                    newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName)) personToUpdate.Name = newName;

                    bool responsibleForPurchase = GetBooleanInput("Responsible for purchase? (yes/no): ");
                    personToUpdate.ResponsibleForPurchase = responsibleForPurchase;

                    // Optionally handle favorite products update if necessary
                    personToUpdate.FavoriteProductIds = personToUpdate.FavoriteProductIds; // or any update logic needed

                    personData.Update(personToUpdate);
                    Console.WriteLine("Person updated successfully!");
                    break;

                case "pantry":
                    Console.Clear();
                    Console.WriteLine("Updating a pantry...");
                    var pantries = pantryData.GetAll();
                    PrintAll(pantries);

                    Console.Write("Enter the ID of the pantry you want to update: ");
                    int pantryId = GetIntInput("Pantry ID: ");
                    Pantry pantryToUpdate = pantryData.GetById(pantryId);

                    // Get updated pantry data
                    Console.Write($"Enter the new capacity for pantry: ");
                    int newCapacity = GetIntInput("Capacity: ");
                    pantryToUpdate.Capacity = newCapacity;

                    pantryData.Update(pantryToUpdate);
                    Console.WriteLine("Pantry updated successfully!");
                    break;

                case "fridge":
                    Console.Clear();
                    Console.WriteLine("Updating a fridge...");
                    var fridges = fridgeData.GetAll();
                    PrintAll(fridges);

                    Console.Write("Enter the ID of the fridge you want to update: ");
                    int fridgeId = GetIntInput("Fridge ID: ");
                    Fridge fridgeToUpdate = fridgeData.GetById(fridgeId);

                    // Get updated fridge data
                    Console.Write($"Enter the new capacity for fridge: ");
                    int newFridgeCapacity = GetIntInput("Capacity: ");
                    fridgeToUpdate.Capacity = newFridgeCapacity;

                    fridgeData.Update(fridgeToUpdate);
                    Console.WriteLine("Fridge updated successfully!");
                    break;

                default:
                    Console.WriteLine("Unknown entity type.");
                    break;
            }
        }

        public void UnifiedDelete(string entityType)
        {
            switch(entityType.ToLower())
            {
                case "product":
                    Console.Clear();
                    Console.WriteLine("Deleting a product...");
                    var products = productData.GetAll();
                    PrintAll(products);

                    Console.Write("Enter the ID of the product you want to delete: ");
                    int productId = GetIntInput("Product ID: ");
                    Product productToDelete = productData.GetById(productId);

                    if (productToDelete != null)
                    {
                        var fridgeCTX = context.Fridge;
                        var fridge = fridgeCTX.Include(f => f.Products).FirstOrDefault();
                        if (fridge != null && fridge.Products.Contains(productToDelete))
                        {
                            fridge.Products.Remove(productToDelete);
                            Console.WriteLine("Product removed from fridge.");
                        }
                        productData.Delete(productId);
                        Console.WriteLine("Product deleted successfully!");
                    }
                        
                    break;

                case "person":
                    Console.Clear();
                    Console.WriteLine("Deleting a person...");
                    var people = personData.GetAll();
                    PrintAll(people);

                    Console.Write("Enter the ID of the person you want to delete: ");
                    int personId = GetIntInput("Person ID: ");
                    Person personToDelete = personData.GetById(personId);

                    personData.Delete(personToDelete.Id);
                    Console.WriteLine("Person deleted successfully!");
                    break;

                case "fridge":
                    Console.Clear();
                    Console.WriteLine("Deleting a fridge...");
                    var fridges = fridgeData.GetAll();
                    PrintAll(fridges);

                    Console.Write("Enter the ID of the fridge you want to delete: ");
                    int fridgeId = GetIntInput("Fridge ID: ");
                    Fridge fridgeToDelete = fridgeData.GetById(fridgeId);

                    fridgeData.Delete(fridgeToDelete.Id);
                    Console.WriteLine("Fridge deleted successfully!");
                    break;

                case "pantry":
                    Console.Clear();
                    Console.WriteLine("Deleting a pantry...");
                    var pantries = pantryData.GetAll();
                    PrintAll(pantries);

                    Console.Write("Enter the ID of the pantry you want to delete: ");
                    int pantryId = GetIntInput("Pantry ID: ");
                    Pantry pantryToDelete = pantryData.GetById(pantryId);

                    pantryData.Delete(pantryToDelete.Id);
                    Console.WriteLine("Pantry deleted successfully!");
                    break;
            }
        } 
    }

}

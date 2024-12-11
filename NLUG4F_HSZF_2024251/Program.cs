using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLUG4F_HSZF_2024251.Applicaion;
using NLUG4F_HSZF_2024251.Model;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Globalization;
using System.Reflection;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;
using static NLUG4F_HSZF_2024251.Persistence.MsSql.JsonRead;
using static System.Net.Mime.MediaTypeNames;

namespace NLUG4F_HSZF_2024251
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddDbContext<HouseHoldDbContext>();

            services.AddSingleton<PersonDataProvider>();
            services.AddSingleton<ProductDataProvider>();
            services.AddSingleton<FridgeDataProvider>();
            services.AddSingleton<PantryDataProvider>();

            services.AddSingleton<IRepository<Person>, PersonDataProvider>();
            services.AddSingleton<IRepository<Product>, ProductDataProvider>();
            services.AddSingleton<IRepository<Fridge>, FridgeDataProvider>();
            services.AddSingleton<IRepository<Pantry>, PantryDataProvider>();
            services.AddSingleton<IJsonRead, JsonRead>();
            services.AddSingleton<InputCollector>();
            services.AddSingleton<IInputCollector, InputCollector>();
            services.AddSingleton<Middleman>();
            services.AddSingleton<IMiddle, Middleman>();


            services.AddSingleton<Querry>();
            services.AddSingleton<MakeFood>();
            services.AddSingleton<Shopping>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Querry querrys = serviceProvider.GetRequiredService<Querry>();
            MakeFood makeFood = serviceProvider.GetRequiredService<MakeFood>();
            Shopping shopping = serviceProvider.GetRequiredService<Shopping>();
            InputCollector inputCollector = serviceProvider.GetRequiredService<InputCollector>();
            Middleman middleMan = serviceProvider.GetRequiredService<Middleman>();


            makeFood.ProductBelowCriticalLevel += OnProductBelowCriticalLevel;
            shopping.FavoriteProductRestock += OnFavoriteProductRestock;
            querrys.ProductsBelowCriticalLevel += OnProductsBelowCriticalLevel;
            querrys.NotifyAllHouseholdMembers += OnProductsBelowCriticalLevel;


            DisplayMenu(
                [
                     "Queries",
                      "Cooking Food",
                      "Shopping",
                      "Read data",
                      "Export data",
                      "DB Editing"
                ],
                [
                    () => DisplayMenu(
                        new string[] {
                            "Products with Near Expiry",
                            "Low Stock Products",
                            "Remaining Total Capacity"
                        },
                        new Action[] {
                            () => WriteOut(querrys.GetExpiringSoon(), "No products are expiring soon."),
                            () => WriteOut(querrys.GetLowStockItems(), "No products are currently at or below the critical stock level."),
                            () => WriteOut(querrys.GetAllStockProduct(), "No products on stock.")
                        }
                    ),
                    () => makeFood.Cook(CollectCookData(middleMan), serviceProvider.GetRequiredService<IRepository<Person>>().GetAll()),
                    () => shopping.RestockProducts(CollectShoppingData(middleMan), serviceProvider.GetRequiredService<IRepository<Person>>().GetAll() ),
                    () => { serviceProvider.GetRequiredService<IJsonRead>().SeedDatabase(); },
                    () => { BoolWrite("Exported succesfully","No products to export", querrys.ExportToTxt()); }, 
                    () => DisplayMenu(
                        new string[] {
                            "Products DB",
                            "Person DB",
                            "Fridge DB",
                            "Pantry DB",
                        },
                        new Action[] {
                            () => DisplayMenu(
                                new string[] {
                                    "Add Product",
                                    "Write all product",
                                    "Update specific product",
                                    "Delete specific product"
                                },
                                new Action[] {
                                    () => inputCollector.UnifiedAdd("product"),
                                    () => inputCollector.PrintAll<Product>(serviceProvider.GetRequiredService<IRepository<Product>>().GetAll()),
                                    () => inputCollector.UnifiedUpdate("product"),
                                    () => inputCollector.UnifiedDelete("product")
                                }
                            ),
                            () => DisplayMenu(
                                new string[] {
                                    "Add person",
                                    "Write all people",
                                    "Update specific person",
                                    "Delete specific person"
                                },
                                new Action[] {
                                    () => inputCollector.UnifiedAdd("person"),
                                    () => inputCollector.PrintAll<Person>(serviceProvider.GetRequiredService<IRepository<Person>>().GetAll()),
                                    () => inputCollector.UnifiedUpdate("person"),
                                    () => inputCollector.UnifiedDelete("person")
                                }
                            ),
                            () => DisplayMenu(
                                new string[] {
                                    "Add fridge",
                                    "Write all fridge",
                                    "Update specific fridge",
                                    "Delete specific fridge"
                                },
                                new Action[] {
                                    () => inputCollector.UnifiedAdd("fridge"),
                                    () => inputCollector.PrintAll<Fridge>(serviceProvider.GetRequiredService<IRepository<Fridge>>().GetAll()),
                                    () => inputCollector.UnifiedUpdate("fridge"),
                                    () => inputCollector.UnifiedDelete("fridge")
                                }
                            ),
                            () => DisplayMenu(
                                new string[] {
                                    "Add pantry",
                                    "Write all pantry",
                                    "Update specific pantry",
                                    "Delete specific pantry"
                                },
                                new Action[] {
                                    () => inputCollector.UnifiedAdd("pantry"),
                                    () => inputCollector.PrintAll<Pantry>(serviceProvider.GetRequiredService<IRepository<Pantry>>().GetAll()),
                                    () => inputCollector.UnifiedUpdate("pantry"),
                                    () => inputCollector.UnifiedDelete("fridge")
                                }
                            ),
                        }
                    )
                ]
            );
        }

        private static List<Product> CollectShoppingData(Middleman mM)
        {
            Console.Clear();
            Console.WriteLine("Fetching all products...");
            List<Product> productsToUpdate = new List<Product>();
            var products = mM.getAllProduct();
            var people = mM.getAllPerson();
            WriteOut(products, "No products to show.");

            List<int> ids = products.Select(p => p.Id).ToList();

            Console.WriteLine("Write a number from the product ids to restock:");

            List<int> selectedProductIds = new List<int>();
            List<decimal> selectedQuantities = new List<decimal>();

            int input;
            do
            {
                Console.Write("Product id (-1 to end it): ");
                if (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for product ID.");
                    continue;
                }

                if (input == -1)
                {
                    break;
                }

                if (ids.Contains(input))
                {
                    var product = products.FirstOrDefault(p => p.Id == input);
                    if (product == null) continue;

                    decimal quantity;
                    while (true)
                    {
                        Console.Write($"Enter the quantity you want to add for {product.Name} (current: {product.Quantity}): ");
                        string quantityInput = Console.ReadLine() ?? string.Empty;

                        if (decimal.TryParse(quantityInput, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity))
                        {
                            if (quantity > 0)
                            {
                                selectedProductIds.Add(input);
                                selectedQuantities.Add(quantity);
                                Product productToUpdate = product;
                                productToUpdate.Quantity += quantity;
                                productToUpdate.BestBefore = DateTime.Now.AddDays(30);
                                productsToUpdate.Add(productToUpdate);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a positive quantity to add.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid decimal number for quantity.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid id, please enter a valid product id from the list.");
                }
            } while (true);

            Console.WriteLine("Products restocked:");
            for (int i = 0; i < selectedProductIds.Count; i++)
            {
                var product = products.FirstOrDefault(p => p.Id == selectedProductIds[i]);
                if (product != null)
                {
                    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Updated quantity of the product: {product.Quantity}, Added quantity: {selectedQuantities[i]}");
                }
            }

            return productsToUpdate;
        }

        private static List<Product> CollectCookData(Middleman mM)
        {
            Console.Clear();
            List<Product> productsToUpdate = new List<Product>();
            var products = mM.getAllProduct();
            if (products.Count > 0)
            {
                List<int> ids = products.Select(p => p.Id).ToList();
                WriteOut(products, "No product found!");

                Console.WriteLine("Write a number from the product ids:");

                List<int> selectedProductIds = new List<int>();
                List<decimal> selectedQuantities = new List<decimal>();

                int input;

                do
                {
                    Console.Write("Product id (-1 to end it): ");
                    if (!int.TryParse(Console.ReadLine(), out input))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for product ID.");
                        continue;
                    }

                    if (input == -1)
                    {
                        break;
                    }

                    if (ids.Contains(input))
                    {
                        var product = products.FirstOrDefault(p => p.Id == input);
                        if (product == null) continue;

                        decimal quantity;
                        while (true)
                        {
                            Console.Write($"Enter the quantity you want to use for {product.Name} (available: {product.Quantity}): ");
                            string quantityInput = Console.ReadLine() ?? string.Empty;

                            if (decimal.TryParse(quantityInput, NumberStyles.Any, CultureInfo.InvariantCulture, out quantity))
                            {
                                if (quantity <= product.Quantity)
                                {
                                    selectedProductIds.Add(input);
                                    selectedQuantities.Add(quantity);
                                    Product productToUpdate = product;
                                    productToUpdate.Quantity -= quantity;
                                    productToUpdate.BestBefore = DateTime.Now.AddDays(30);
                                    productsToUpdate.Add(productToUpdate);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($"Insufficient quantity available. Please enter a quantity up to {product.Quantity}.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid decimal number for quantity.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid id, please enter a valid product id from the list.");
                    }

                } while (true);

                Console.WriteLine("Selected products for cooking:");
                for (int i = 0; i < selectedProductIds.Count; i++)
                {
                    var product = products.FirstOrDefault(p => p.Id == selectedProductIds[i]);
                    if (product != null)
                    {
                        Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Updated quantity of the product: {product.Quantity}, Used quantity: {selectedQuantities[i]}");
                    }
                }

                return productsToUpdate;
            }
            else
            {
                Console.WriteLine("No avaliable product to cook from! :(");
                return new List<Product>();
            }
        }

        private static void WriteOut(List<Product> products, string? txt)
        {
            Console.Clear();
            if (products.Count == 0)
            {
                Console.WriteLine(txt);
            }
            else
            {
                Console.WriteLine($"Number of products: {products.Count}");
                Console.WriteLine("Current products:");
                foreach (var product in products)
                {
                    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                    Console.WriteLine("----------------------");
                }
            }
        }

        private static void BoolWrite(string True, string False, bool stmt)
        {
            if (stmt)
            {
                Console.WriteLine(True);
            }
            else
            {
                Console.WriteLine(False);
            }
        }

        private static void OnProductBelowCriticalLevel(object sender, ProductPersonEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"Alert: {e.Product.Name} has dropped below the critical level! Current quantity: {e.Product.Quantity}");

            if (e.Person != null)
            {
                Console.WriteLine($"Responsible person for purchase: {e.Person.Name}");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void OnFavoriteProductRestock(object sender, FavoriteProductRestockEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"Product restocked: {e.Product.Name} for {e.Person.Name}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void OnProductsBelowCriticalLevel(object sender, LowStockProductListEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Products below critical level:");
            foreach (var product in e.Products)
            {
                Console.WriteLine($"- {product.Name}, Quantity: {product.Quantity}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void DisplayMenu(string[] menus, Action[] actions)
        {
            bool exit = false;
            int y = 5;

            while (!exit)
            {
                Console.Clear();
                Console.SetCursorPosition(50, 3);
                Console.WriteLine("Choose an option:");
                for (int i = 0; i < menus.Length; i++)
                {
                    Console.SetCursorPosition(50, y);
                    Console.WriteLine($"{i + 1}. {menus[i]}");
                    y += 2;
                }
                Console.SetCursorPosition(50, y);
                Console.WriteLine($"{menus.Length + 1}. Exit");

                Console.SetCursorPosition(50, y += 2);
                Console.Write("Enter the number of the selected option: ");
                string input = Console.ReadLine();

                int selectedIndex;
                if (int.TryParse(input, out selectedIndex) && selectedIndex >= 1 && selectedIndex <= menus.Length + 1)
                {
                    if (selectedIndex == menus.Length + 1)
                    {
                        Console.WriteLine("Exiting...");
                        exit = true;
                    }
                    else
                    {
                        actions[selectedIndex - 1]();
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice, please try again.");
                    Console.ReadKey();
                }

                y = 5;
            }
        }
    }
}
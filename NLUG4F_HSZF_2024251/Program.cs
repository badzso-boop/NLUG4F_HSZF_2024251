using Microsoft.EntityFrameworkCore;
using NLUG4F_HSZF_2024251.Applicaion;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Globalization;
using System.Reflection;

namespace NLUG4F_HSZF_2024251
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ctx = new HouseHoldDbContext();
            JsonRead json = new JsonRead(ctx);

            PersonDataProvider personData = new PersonDataProvider(ctx);
            ProductDataProvider productData = new ProductDataProvider(ctx, personData);
            FridgeDataProvider fridgeData = new FridgeDataProvider(ctx);
            PantryDataProvider pantryData = new PantryDataProvider(ctx);

            ProductCRUD productCRUD = new ProductCRUD(ctx, productData, personData);
            PersonCRUD personCRUD = new PersonCRUD(ctx, personData, productData);
            FridgeCRUD fridgeCRUD = new FridgeCRUD(ctx, fridgeData, productData);
            PantryCRUD pantryCRUD = new PantryCRUD(ctx, pantryData, productData);

            Querry querrys = new Querry(ctx, productData, personData, fridgeData, pantryData);
            MakeFood MakeFood = new MakeFood(productCRUD, ctx, productData);
            Shopping shopping = new Shopping(ctx, productData, personData);

            productData.ProductBelowCriticalLevel += OnProductBelowCriticalLevel;
            shopping.FavoriteProductRestock += OnFavoriteProductRestock;
            querrys.ProductsBelowCriticalLevel += OnProductsBelowCriticalLevel;


            DisplayMenu(
                [
                    "Lekérdezések",
                    "Étel készítése",
                    "Bevásárlás",
                    "Beolvasás",
                    "Kiírás",
                    "DB Szerkesztés"
                ],
                [
                    () => DisplayMenu(
                        new string[] {
                            "Közeli lejáratú termékek",
                            "Kifogyóban lévő termékek",
                            "Maradék összkapacítás"
                        },
                        new Action[] {
                            querrys.GetExpiringSoon,
                            querrys.GetLowStockItems,
                            querrys.GetAllStockProduct
                        }
                    ),
                    MakeFood.Cook,
                    shopping.RestockProducts,
                    () => {json.SeedDatabase(ctx); },
                    querrys.ExportToTxt,
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
                                    productCRUD.Hozzaad,
                                    productCRUD.KiirasAll,
                                    productCRUD.Update,
                                    productCRUD.Delete
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
                                    personCRUD.Hozzaad,
                                    personCRUD.KiirasAll,
                                    personCRUD.Update,
                                    personCRUD.Delete
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
                                    fridgeCRUD.Hozzaad,
                                    fridgeCRUD.KiirasAll,
                                    fridgeCRUD.Update,
                                    fridgeCRUD.Delete
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
                                    pantryCRUD.Hozzaad,
                                    pantryCRUD.KiirasAll,
                                    pantryCRUD.Update,
                                    pantryCRUD.Delete
                                }
                            ),
                        }
                    )
                ]
            );
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
                Console.WriteLine("Válassz egy menüpontot:");
                for (int i = 0; i < menus.Length; i++)
                {
                    Console.SetCursorPosition(50, y);
                    Console.WriteLine($"{i + 1}. {menus[i]}");
                    y += 2;
                }
                Console.SetCursorPosition(50, y);
                Console.WriteLine($"{menus.Length + 1}. Kilépés");

                Console.SetCursorPosition(50, y += 2);
                Console.Write("Add meg a választott menüpont számát: ");
                string input = Console.ReadLine();

                int selectedIndex;
                if (int.TryParse(input, out selectedIndex) && selectedIndex >= 1 && selectedIndex <= menus.Length + 1)
                {
                    if (selectedIndex == menus.Length + 1)
                    {
                        Console.WriteLine("Kilépés...");
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
                    Console.WriteLine("Érvénytelen választás, próbáld újra.");
                    Console.ReadKey();
                }

                y = 5;
            }
        }
    }
}
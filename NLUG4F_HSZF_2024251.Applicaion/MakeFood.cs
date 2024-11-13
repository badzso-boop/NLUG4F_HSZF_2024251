using Castle.Components.DictionaryAdapter.Xml;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class MakeFood
    {
        public MakeFood(ProductCRUD productCRUD, HouseHoldDbContext context, ProductDataProvider productData)
        {
            this.productCRUD = productCRUD;
            this.context = context;
            this.productData = productData;
        }

        ProductCRUD productCRUD { get; set; }
        HouseHoldDbContext context { get; set; }
        ProductDataProvider productData { get; set; }

        static void Kiir(List<Product> products)
        {
            Console.WriteLine($"Number of products: {products.Count}");
            Console.WriteLine("Current products:");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}, Store in fridge: {product.StoreInFridge}");
                Console.WriteLine("----------------------");
            }
        }

        public void Cook()
        {
            Console.Clear();
            var products = productData.GetAll();
            if (products.Count > 0)
            {
                Kiir(products);

                List<int> ids = products.Select(p => p.Id).ToList();

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
                                    try
                                    {
                                        productData.Update(productToUpdate);
                                    }
                                    catch (InvalidProductDataException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
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
            }
            else
            {
                Console.WriteLine("No avaliable product to cook from! :(");
            }
        }
    }
}

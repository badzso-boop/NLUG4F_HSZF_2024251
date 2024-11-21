using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class Shopping
    {
        ProductDataProvider productData { get; set; }
        PersonDataProvider personData { get; set; }

        public event EventHandler<FavoriteProductRestockEventArgs> FavoriteProductRestock;
        public Shopping(ProductDataProvider productData, PersonDataProvider personData)
        {
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

        public void RestockProducts()
        {
            Console.Clear();
            Console.WriteLine("Fetching all products...");
            var products = productData.GetAll();
            var people = personData.GetAll();
            Kiir(products);

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

                                try
                                {
                                    productData.Update(productToUpdate);
                                    foreach (var person in people)
                                    {
                                        foreach (var personProduct in person.FavoriteProductIds)
                                        {
                                            if (personProduct == product.Id)
                                            {
                                                FavoriteProductRestock?.Invoke(this, new FavoriteProductRestockEventArgs(product, person));
                                            }
                                        }
                                    }
                                }
                                catch (InvalidProductDataException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
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
        }
    }
}

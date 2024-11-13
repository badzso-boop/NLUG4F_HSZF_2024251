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
    public class FridgeCRUD : CRUDActions
    {
        HouseHoldDbContext context { get; set; }
        FridgeDataProvider fridgeData { get; set; }
        ProductDataProvider productData { get; set; }

        public FridgeCRUD(HouseHoldDbContext context, FridgeDataProvider fridgeData, ProductDataProvider productData)
        {
            this.context = context;
            this.fridgeData = fridgeData;
            this.productData = productData;
        }

        static void Kiir(List<Fridge> fridges)
        {
            foreach (var fridge in fridges)
            {
                Console.WriteLine($"Id: {fridge.Id}, Capacity: {fridge.Capacity}, Products: ");
                foreach (var product in fridge.Products)
                {
                    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                    Console.WriteLine("+++++++++++++++++++++");
                }
                Console.WriteLine("----------------------");
            }
        }

        public void Hozzaad()
        {
            Console.Clear();
            Console.WriteLine("Adding a new Fridge...");

            Console.Write("Enter the capacity: ");
            int capacity = int.Parse(Console.ReadLine());


            List<int> ids = new List<int>();
            foreach (var product in productData.GetAll())
            {
                ids.Add(product.Id);
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                Console.WriteLine("----------------------");
            }

            List<int> Products = new List<int>();
            int input;

            Console.WriteLine("Write a number from the product ids:");

            do
            {
                Console.Write("Product id (-1 to end it): ");
                input = int.Parse(Console.ReadLine());

                if (input == -1)
                {
                    break;
                }

                if (ids.Contains(input))
                {
                    Products.Add(input);
                }
                else
                {
                    Console.WriteLine("Invalid id, please enter a valid product id from the list.");
                }

            } while (true);
            List<Product> selectedProducts = productData.GetAll()
                                                .Where(p => Products.Contains(p.Id))
                                                .ToList();

            Fridge newFridge = new Fridge(capacity, selectedProducts);
            fridgeData.Add(newFridge);
            Console.WriteLine("Fridge added successfully!");
        }

        public void KiirasAll()
        {
            Console.Clear();
            Console.WriteLine("Fetching all Fridge...");
            var fridges = fridgeData.GetAll();
            Console.WriteLine($"Number of fridges: {fridges.Count}");
            Console.WriteLine("Current fridges:");
            Kiir(fridges);
        }

        public void KiirasEgy(int id)
        {
            Fridge FridgeToWrite = fridgeData.GetById(id);
            Kiir(new List<Fridge> { FridgeToWrite });
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine("Fetching all Fridge...");
            var fridges = fridgeData.GetAll();
            Kiir(fridges);


            Fridge fridgeToUpdate = new Fridge();
            while (true)
            {
                Console.Write("Enter the ID of the product you want to update: ");

                if (!int.TryParse(Console.ReadLine(), out int fridgeId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                fridgeToUpdate = fridgeData.GetById(fridgeId);
                break;
            }
            Console.Write($"Enter the capacity for {fridgeToUpdate.Capacity}: ");
            int newCapacity = int.Parse(Console.ReadLine());
            fridgeToUpdate.Capacity = newCapacity;


            fridgeData.Update(fridgeToUpdate);
            Console.WriteLine("Fridge updated successfully!");
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("Fetching all Fridge...");
            var fridges = fridgeData.GetAll();
            Kiir(fridges);


            while (true)
            {
                Console.Write("Enter the ID of the fridge you want to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int fridgeId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                var fridge = fridgeData.GetById(fridgeId);
                if (fridge != null)
                {
                    fridgeData.Delete(fridgeId);
                    context.SaveChanges();
                    Console.WriteLine("Fridge deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Fridge not found.");
                }
                break;
            }
        }
    }
}

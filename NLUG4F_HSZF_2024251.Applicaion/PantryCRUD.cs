using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class PantryCRUD : CRUDActions<Pantry>
    {
        HouseHoldDbContext context { get; set; }
        PantryDataProvider pantryData { get; set; }
        ProductDataProvider productData { get; set; }

        public PantryCRUD(HouseHoldDbContext context, PantryDataProvider pantryData, ProductDataProvider productData)
        {
            this.context = context;
            this.pantryData = pantryData;
            this.productData = productData;
        }

        static void Kiir(List<Pantry> pantrys)
        {
            foreach (var pantry in pantrys)
            {
                Console.WriteLine($"Id: {pantry.Id}, Capacity: {pantry.Capacity}, Products: ");
                foreach (var product in pantry.Products)
                {
                    Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                    Console.WriteLine("+++++++++++++++++++++");
                }
                Console.WriteLine("----------------------");
            }
        }

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("Adding a new pantry...");

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

            
            Pantry newPantry = new Pantry(capacity, selectedProducts);
            pantryData.Add(newPantry);
            Console.WriteLine("Pantry added successfully!");
        }

        public List<Pantry> WriteAll()
        {
            return pantryData.GetAll();
        }

        public void WriteOne(int id)
        {
            Pantry PantryToWrite = pantryData.GetById(id);
            Kiir(new List<Pantry> { PantryToWrite });
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine("Fetching all pantry...");
            var pantrys = pantryData.GetAll();
            Kiir(pantrys);


            Pantry pantrytoUpdate = new Pantry();
            while (true)
            {
                Console.Write("Enter the ID of the pantry you want to update: ");

                if (!int.TryParse(Console.ReadLine(), out int pantryId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                pantrytoUpdate = pantryData.GetById(pantryId);
                break;
            }
            Console.Write($"Enter the capacity for {pantrytoUpdate.Capacity}: ");
            int newCapacity = int.Parse(Console.ReadLine());
            pantrytoUpdate.Capacity = newCapacity;


            pantryData.Update(pantrytoUpdate);
            Console.WriteLine("Pantry updated successfully!");
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("Fetching all pantry...");
            var pantrys = pantryData.GetAll();
            Kiir(pantrys);


            while (true)
            {
                Console.Write("Enter the ID of the pantry you want to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int pantryId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                var pantry = pantryData.GetById(pantryId);
                if (pantry != null)
                {
                    pantryData.Delete(pantryId);
                    context.SaveChanges();
                    Console.WriteLine("Pantry deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Pantry not found.");
                }
                break;
            }
        }
    }
}

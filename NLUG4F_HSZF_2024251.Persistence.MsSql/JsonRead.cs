using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class JsonRead
    {
        public HashSet<Person> people { get; set; }
        public HashSet<Product> products { get; set; }
        public Fridge fridge { get; set; }
        public Pantry pantry { get; set; }

        public JsonRead(HouseHoldDbContext ctx)
        {
            //SeedDatabase(ctx);
        }

        public void SeedDatabase(HouseHoldDbContext ctx)
        {
            people = new HashSet<Person>();
            products = new HashSet<Product>();
            fridge = new Fridge();
            pantry = new Pantry();

            string jsonFilePath = "jsons/data.json";
            string jsonString = File.ReadAllText(jsonFilePath);
            var householdData = JsonSerializer.Deserialize<TempData>(jsonString);

            if (householdData != null)
            {
                people = new HashSet<Person>(householdData.Persons);
                foreach (var product in householdData.Products)
                {
                    products.Add(new Product(product.Name, product.Quantity, product.CriticalLevel, product.BestBefore, product.StoreInFridge));
                }
                fridge = new Fridge(householdData.Fridge.Capacity);
                pantry = new Pantry(householdData.Pantry.Capacity);
            }

            ctx.Products.AddRange(products);
            ctx.SaveChanges();

            var fridgeProducts = ctx.Products
                                    .Where(p => householdData.Fridge.ProductIds.Contains(p.Id))
                                    .ToList();
            fridge.Products.AddRange(fridgeProducts);

            var pantryProducts = ctx.Products
                                    .Where(p => householdData.Pantry.ProductIds.Contains(p.Id))
                                    .ToList();
            pantry.Products.AddRange(pantryProducts);

            ctx.People.AddRange(people);
            ctx.Pantry.Add(pantry);
            ctx.Fridge.Add(fridge);

            ctx.SaveChanges();
        }

        //public class HouseholdData
        //{
        //    public Fridge Fridge { get; set; }
        //    public Pantry Pantry { get; set; }
        //    public List<Person> Persons { get; set; }
        //    public List<Product> Products { get; set; }
        //}

        public class TempData
        {
            public FridgeData Fridge { get; set; }
            public PantryData Pantry { get; set; }
            public List<Person> Persons { get; set; }
            public List<Product> Products { get; set; }
        }


        public class FridgeData
        {
            public int Capacity { get; set; }
            public List<int> ProductIds { get; set; }
        }

        public class PantryData
        {
            public int Capacity { get; set; }
            public List<int> ProductIds { get; set; }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NLUG4F_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class JsonRead : IJsonRead
    {
        public HashSet<Person> People { get; set; }
        public HashSet<Product> Products { get; set; }
        public Fridge Fridge { get; set; }
        public Pantry Pantry { get; set; }

        private HouseHoldDbContext ctx;

        public JsonRead(HouseHoldDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void SeedDatabase()
        {
            People = new HashSet<Person>();
            Products = new HashSet<Product>();
            Fridge = new Fridge();
            Pantry = new Pantry();

            string jsonFilePath = "jsons/data.json";
            string jsonString = File.ReadAllText(jsonFilePath);
            var householdData = JsonSerializer.Deserialize<TempData>(jsonString);

            if (householdData != null)
            {
                People = new HashSet<Person>(householdData.Persons);
                foreach (var product in householdData.Products)
                {
                    Products.Add(new Product(product.Name, product.Quantity, product.CriticalLevel, product.BestBefore, product.StoreInFridge));
                }
                Fridge = new Fridge(householdData.Fridge.Capacity);
                Pantry = new Pantry(householdData.Pantry.Capacity);
            }

            ctx.Products.AddRange(Products);
            ctx.SaveChanges();

            var FridgeProducts = ctx.Products
                                    .Where(p => householdData.Fridge.ProductIds.Contains(p.Id))
                                    .ToList();
            Fridge.Products.AddRange(FridgeProducts);

            var PantryProducts = ctx.Products
                                    .Where(p => householdData.Pantry.ProductIds.Contains(p.Id))
                                    .ToList();
            Pantry.Products.AddRange(PantryProducts);

            ctx.People.AddRange(People);
            ctx.Pantry.Add(Pantry);
            ctx.Fridge.Add(Fridge);

            ctx.SaveChanges();
        }

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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NLUG4F_HSZF_2024251.Persistence.MsSql;

namespace NLUG4F_HSZF_2024251.Test
{
    [TestClass]
    public class ProductDataProviderTests
    {
        private HouseHoldDbContext _context;
        private ProductDataProvider _productDataProvider;
        private Product product = new Product { Name = "Test Product", Quantity = 10, CriticalLevel = 5, BestBefore = DateTime.Today.AddDays(1), StoreInFridge = false };

        [TestInitialize]
        public void Setup()
        {
            _context = new HouseHoldDbContext();
            _productDataProvider = new ProductDataProvider(_context);
        }

        [TestMethod]
        public void AddProduct()
        {
            _productDataProvider.Add(product);

            var addedProduct = _context.Products.FirstOrDefault(p => p.Name == "Test Product");
            Assert.IsNotNull(addedProduct);
        }

        [TestMethod]
        public void GetProductById()
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            var result = _productDataProvider.GetById(product.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(product.Id, result.Id);
        }

        [TestMethod]
        public void UpdateProduct()
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            var LocaleProduct = product;
            LocaleProduct.Name = "Updated Product";

            _productDataProvider.Update(LocaleProduct);

            var updatedProduct = _context.Products.FirstOrDefault(p => p.Id == LocaleProduct.Id);
            Assert.AreEqual("Updated Product", updatedProduct.Name);
        }

        [TestMethod]
        public void DeleteProduct()
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            _productDataProvider.Delete(product.Id);

            var deletedProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        public void GetAllProducts()
        {
            _context.Products.AddRange(new List<Product>
        {
            new Product { Name = "Product 1", Quantity = 5, CriticalLevel = 2, BestBefore = DateTime.Now, StoreInFridge = false },
            new Product { Name = "Product 2", Quantity = 10, CriticalLevel = 5, BestBefore = DateTime.Now, StoreInFridge = true }
        });
            _context.SaveChanges();

            var products = _productDataProvider.GetAll();

            Assert.AreEqual(2, products.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidProductDataException))]
        public void Add_Error()
        {
            var invalidProduct = new Product("", -1, -1, DateTime.Now.AddDays(-1), true);
            _productDataProvider.Add(invalidProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void GetById_Error()
        {
            _productDataProvider.GetById(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidProductDataException))]
        public void Update_Error()
        {
            var invalidProduct = new Product("", -1, -1, DateTime.Now.AddDays(-1), true) { Id = 1 };
            _productDataProvider.Update(invalidProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void Delete_Error()
        {
            _productDataProvider.Delete(-1);
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using NLUG4F_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class ProductDataProvider : IProductDataProvider
    {
        private readonly HouseHoldDbContext _context;
        private readonly PersonDataProvider _personDataProvider;
        public event EventHandler<ProductPersonEventArgs> ProductBelowCriticalLevel;

        public ProductDataProvider(HouseHoldDbContext context, PersonDataProvider personDataProvider)
        {
            _context = context;
            _personDataProvider = personDataProvider;
        }

        public void Add(Product entity)
        {
            ValidateProduct(entity);
            _context.Products.Add(entity);
            _context.SaveChanges();
        }

        public Product? GetById(int id)
        {
            var product =  _context.Products.Find(id);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }
            return product;
        }

        public List<Product> GetAll()
        {
            var seged = _context.Products;
            return _context.Products.ToList();
        }

        public void Update(Product entity)
        {
            ValidateProduct(entity);
            var existingProduct = _context.Products.Find(entity.Id);
            if (existingProduct == null)
            {
                throw new ProductNotFoundException(entity.Id);
            }
            _context.Products.Update(existingProduct);
            _context.SaveChanges();

            if (entity.Quantity <= entity.CriticalLevel)
            {
                var responsiblePerson = _personDataProvider.GetAll().FirstOrDefault(p => p.ResponsibleForPurchase == true);
                ProductBelowCriticalLevel?.Invoke(this, new ProductPersonEventArgs(entity, responsiblePerson));
            }
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        private void ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new InvalidProductDataException("Product name cannot be null or empty.");
            }
            if (product.Quantity < 0)
            {
                throw new InvalidProductDataException("Product quantity cannot be negative.");
            }
            if (product.CriticalLevel < 0)
            {
                throw new InvalidProductDataException("Product critical level cannot be negative.");
            }
            if (product.BestBefore < DateTime.Now)
            {
                throw new InvalidProductDataException("Product best before date must be in the future.");
            }
        }
    }
}

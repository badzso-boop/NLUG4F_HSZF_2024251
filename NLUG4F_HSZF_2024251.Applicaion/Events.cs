using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class FavoriteProductRestockEventArgs : EventArgs
    {
        public Product Product { get; }
        public Person Person { get; }

        public FavoriteProductRestockEventArgs(Product product, Person person)
        {
            Product = product;
            Person = person;
        }
    }

    public class LowStockProductListEventArgs : EventArgs
    {
        public List<Product> Products { get; }

        public LowStockProductListEventArgs(List<Product> products)
        {
            Products = products;
        }
    }

}

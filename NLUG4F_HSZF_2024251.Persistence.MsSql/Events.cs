using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class ProductPersonEventArgs : EventArgs
    {
        public Product Product { get; }
        public Person Person { get; }

        public ProductPersonEventArgs(Product product, Person person)
        {
            Product = product;
            Person = person;
        }
    }
}

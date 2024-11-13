using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class InvalidProductDataException : Exception
    {
        public InvalidProductDataException(string message) : base(message) { }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int id)
            : base($"Product with ID {id} was not found.") { }
    }
}

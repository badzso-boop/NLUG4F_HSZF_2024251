using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class InvalidPersonDataException : Exception
    {
        public InvalidPersonDataException(string message) : base(message) { }
    }

    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException(int id)
            : base($"Person with ID {id} was not found.") { }
    }
}

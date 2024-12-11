using System;
using System.Collections.Generic;
using NLUG4F_HSZF_2024251.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class Middleman : IMiddle
    {
        private readonly IRepository<Product> _productDataProvider;
        private readonly IRepository<Person> _personDataProvider;

        public Middleman(IRepository<Product> productDataProvider, IRepository<Person> personDataProvider)
        {
            _productDataProvider = productDataProvider;
            _personDataProvider = personDataProvider;
        }

        public List<Product> getAllProduct()
        {
            var list = _productDataProvider.GetAll();
            return list;
        }

        public List<Person> getAllPerson()
        {
            var list = _personDataProvider.GetAll();
            return list;
        }
    }
}

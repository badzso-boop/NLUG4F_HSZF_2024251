using Castle.Components.DictionaryAdapter.Xml;
using NLUG4F_HSZF_2024251.Model;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class MakeFood
    {
        private readonly IProductDataProvider _productDataProvider;
        public MakeFood(IProductDataProvider productDataProvider)
        {
            _productDataProvider = productDataProvider;
        }

        public void Cook(List<Product> products)
        {
            foreach (var productToUpdate in products)
            {
                _productDataProvider.Update(productToUpdate);
            }
        }
    }
}

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
        private readonly IRepository<Product> _productDataProvider;
        public event EventHandler<ProductPersonEventArgs> ProductBelowCriticalLevel;
        public MakeFood(IRepository<Product> productDataProvider)
        {
            _productDataProvider = productDataProvider;
        }

        public void Cook(List<Product> products, List<Person> persons)
        {
            foreach (var productToUpdate in products)
            {
                if (productToUpdate.Quantity <= productToUpdate.CriticalLevel)
                {
                    var responsiblePerson = persons.FirstOrDefault(p => p.ResponsibleForPurchase == true);
                    ProductBelowCriticalLevel?.Invoke(this, new ProductPersonEventArgs(productToUpdate, responsiblePerson));
                }
                _productDataProvider.Update(productToUpdate);
            }
        }
    }
}

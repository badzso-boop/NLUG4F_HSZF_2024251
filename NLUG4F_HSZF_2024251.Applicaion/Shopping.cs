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
    public class Shopping
    {
        private readonly IRepository<Product> productData;
        private readonly IRepository<Person> personData;

        public event EventHandler<FavoriteProductRestockEventArgs> FavoriteProductRestock;
        public Shopping(IRepository<Product> productDataProvider, IRepository<Person> PersonDataProvider)
        {
            this.productData = productDataProvider;
            this.personData = PersonDataProvider;
        }

        public void RestockProducts(List<Product> productsToUpdate, List<Person> people)
        {
            foreach (var productToUpdate in productsToUpdate)
            {
                productData.Update(productToUpdate);
                foreach (var person in people)
                {
                    foreach (var personProduct in person.FavoriteProductIds)
                    {
                        if (personProduct == productToUpdate.Id)
                        {
                            FavoriteProductRestock?.Invoke(this, new FavoriteProductRestockEventArgs(productToUpdate, person));
                        }
                    }
                }
            }
        }
    }
}

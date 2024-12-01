using Microsoft.Extensions.DependencyInjection;
using NLUG4F_HSZF_2024251.Model;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class ServiceDatas : IServiceDatas
    {
        private readonly ServiceCollection serviceCollection = new ServiceCollection();
        public ProductDataProvider ProductDataProvider { get; private set; }
        public PersonDataProvider PersonDataProvider { get; private set; }
        public FridgeDataProvider FridgeDataProvider { get; private set; }
        public PantryDataProvider PantryDataProvider { get; private set; }
        public GetDatas GetDatas { get; private set; }

        public void Generate()
        {
            serviceCollection.AddTransient<DataProvider>();
            serviceCollection.AddTransient<GetDatas>();
            serviceCollection.AddTransient<HouseHoldDbContext>();

            serviceCollection.AddTransient<PersonDataProvider>();
            serviceCollection.AddTransient<ProductDataProvider>();
            serviceCollection.AddTransient<FridgeDataProvider>();
            serviceCollection.AddTransient<PantryDataProvider>();

            serviceCollection.AddTransient<IProductDataProvider, ProductDataProvider>();
            serviceCollection.AddTransient<IRepository<Person>, PersonDataProvider>();
            serviceCollection.AddTransient<IRepository<Fridge>, FridgeDataProvider>();
            serviceCollection.AddTransient<IRepository<Pantry>, PantryDataProvider>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            GetDatas = serviceProvider.GetRequiredService<GetDatas>();
            ProductDataProvider = serviceProvider.GetRequiredService<ProductDataProvider>();
            PersonDataProvider = serviceProvider.GetRequiredService<PersonDataProvider>();
            FridgeDataProvider = serviceProvider.GetRequiredService<FridgeDataProvider>();
            PantryDataProvider = serviceProvider.GetRequiredService<PantryDataProvider>();
        }
    }
}


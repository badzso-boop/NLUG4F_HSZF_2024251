using NLUG4F_HSZF_2024251.Applicaion;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Model
{
    public interface IQuerryRepository
    {
        List<Product> GetExpiringSoon();
        List<Product> GetLowStockItems();
        List<Product> GetAllStockProduct();
        bool ExportToTxt();
    }

    public interface IServiceDatas
    {
        ProductDataProvider ProductDataProvider { get; }
        PersonDataProvider PersonDataProvider { get; }
        FridgeDataProvider FridgeDataProvider { get; }
        PantryDataProvider PantryDataProvider { get; }

        void Generate();
    }

}

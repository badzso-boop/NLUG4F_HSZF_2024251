using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Model
{
    // IProductRepository
    // Leírás: Kiterjesztett interfész a Product entitások adatbázis-műveleteihez, amely az általános CRUD (Create, Read, Update, Delete) műveleteken kívül termékspecifikus metódusokat is tartalmaz, például a hamarosan lejáró vagy alacsony készletszintű termékek lekérdezésére.

    //Metódusok:
    //Task<IEnumerable<Product>> GetExpiringSoon():
    //Leírás: A hamarosan lejáró termékek lekérdezése.A metódus visszaadja azokat a termékeket, amelyek közel állnak a lejárati dátumukhoz(általában egy előre meghatározott időkereten belül).

    //Task<IEnumerable<Product>> GetLowStockItems():
    //Leírás: Az alacsony készletszintű termékek lekérdezése.Visszaadja azokat a termékeket, amelyek mennyisége eléri vagy alacsonyabb a kritikus szintnél, jelezve, hogy szükség lehet az utánpótlásukra.

    public interface IProductRepository
    {
        List<Product> GetExpiringSoon();
        List<Product> GetLowStockItems();
        List<Product> GetAllStockProduct();
        bool ExportToTxt();
    }
}

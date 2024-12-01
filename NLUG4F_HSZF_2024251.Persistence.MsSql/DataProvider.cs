using NLUG4F_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class DataProvider : IDataProvider
    {
        public HouseHoldDbContext _context;
        public JsonRead JsonRead { get; set; }
        public ProductDataProvider ProductDataProvider { get; set; }
        public PersonDataProvider PersonDataProvider { get; set; }
        public FridgeDataProvider FridgeDataProvider { get; set; }
        public PantryDataProvider PantryDataProvider { get; set; }

        public DataProvider()
        {
            _context = new HouseHoldDbContext();
            JsonRead = new JsonRead(_context);
            this.PersonDataProvider = new PersonDataProvider(_context);
            this.ProductDataProvider = new ProductDataProvider(_context, this.PersonDataProvider);
            this.FridgeDataProvider = new FridgeDataProvider(_context);
            this.PantryDataProvider = new PantryDataProvider(_context);
        }
    }
}

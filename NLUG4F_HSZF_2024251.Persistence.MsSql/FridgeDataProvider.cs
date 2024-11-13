using NLUG4F_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class FridgeDataProvider : IRepository<Fridge>
    {
        private readonly HouseHoldDbContext _context;

        public FridgeDataProvider(HouseHoldDbContext context)
        {
            _context = context;
        }

        public void Add(Fridge entity)
        {
            _context.Fridge.Add(entity);
            _context.SaveChanges();
        }

        public Fridge? GetById(int id)
        {
            return _context.Fridge.Find(id);
        }
        public List<Fridge> GetAll()
        {
            return _context.Fridge.ToList();
        }

        public void Update(Fridge entity)
        {
            _context.Fridge.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var fridge = _context.Fridge.Find(id);
            _context.Fridge.Remove(fridge);
            _context.SaveChanges();
        }
    }
}

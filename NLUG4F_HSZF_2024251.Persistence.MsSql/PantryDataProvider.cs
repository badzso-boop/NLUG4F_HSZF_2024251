using NLUG4F_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class PantryDataProvider : IRepository<Pantry>
    {
        private readonly HouseHoldDbContext _context;

        public PantryDataProvider(HouseHoldDbContext context)
        {
            _context = context;
        }

        public void Add(Pantry entity)
        {
            _context.Pantry.Add(entity);
            _context.SaveChanges();
        }

        public Pantry? GetById(int id)
        {
            return _context.Pantry.Find(id);
        }
        public List<Pantry> GetAll()
        {
            return _context.Pantry.ToList();
        }

        public void Update(Pantry entity)
        {
            _context.Pantry.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var pantry = _context.Pantry.Find(id);
            _context.Pantry.Remove(pantry);
            _context.SaveChanges();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NLUG4F_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class PersonDataProvider : IRepository<Person>
    {
        private readonly HouseHoldDbContext _context;
        public PersonDataProvider(HouseHoldDbContext context)
        {
            _context = context;
        }

        public void Add(Person entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new InvalidProductDataException("Person name cannot be null or empty.");
            }
            _context.People.Add(entity);
            _context.SaveChanges();
        }

        public List<Person> GetAll()
        {
            return _context.People.ToList();
        }

        public Person? GetById(int id)
        {
            var person = _context.People.Find(id);
            if (person == null) 
            {
                throw new PersonNotFoundException(id);
            }
            return person;
        }

        public void Update(Person entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new InvalidProductDataException("Person name cannot be null or empty.");
            }
            var existingPerson = _context.People.Find(entity.Id);
            if (existingPerson == null)
            {
                throw new PersonNotFoundException(entity.Id);
            }
            _context.People.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var person = _context.People.Find(id);
            if (person == null)
            {
                throw new PersonNotFoundException(id);
            }
            _context.People.Remove(person);
            _context.SaveChanges();
        }
    }
}

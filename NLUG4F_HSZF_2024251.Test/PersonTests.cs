using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLUG4F_HSZF_2024251.Applicaion;
using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Test
{
    [TestClass]
    public class PersonDataProviderTests
    {
        private HouseHoldDbContext _context;
        private PersonDataProvider _dataProvider;
        private ProductDataProvider _productDataProvider;
        private PersonCRUD _personCRUD;
        private Person person1 = new Person { Name = "John Doe", FavoriteProductIds = [1, 3], ResponsibleForPurchase = false };
        private Person person2 = new Person { Name = "John Peter", FavoriteProductIds = [4, 5], ResponsibleForPurchase = true };

        [TestInitialize]
        public void Setup()
        {
            _context = new HouseHoldDbContext();
            _dataProvider = new PersonDataProvider(_context);
            _productDataProvider = new ProductDataProvider(_context, _dataProvider);
            _personCRUD = new PersonCRUD(_context, _dataProvider, _productDataProvider);
        }

        [TestMethod]
        public void AddPerson()
        {
            _dataProvider.Add(person1);

            var result = _dataProvider.GetAll();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("John Doe", result.First().Name);
        }

        [TestMethod]
        public void GetAllPeople()
        {
            _dataProvider.Add(person1);
            _dataProvider.Add(person2);

            var result = _dataProvider.GetAll();
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetPersonById()
        {
            _dataProvider.Add(person1);

            var result = _dataProvider.GetById(person1.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Name);
        }

        [TestMethod]
        public void UpdatePerson()
        {
            var LocalePerson = person2;
            _dataProvider.Add(LocalePerson);

            LocalePerson.Name = "John Smith";
            _dataProvider.Update(LocalePerson);

            var result = _dataProvider.GetById(LocalePerson.Id);
            Assert.AreEqual("John Smith", result.Name);
        }

        [TestMethod]
        public void DeletePerson()
        {
            _dataProvider.Add(person1);

            _dataProvider.Delete(person1.Id);
            var result = _dataProvider.GetAll();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidProductDataException))]
        public void Add_Error()
        {
            var person = new Person { Name = null };
            _dataProvider.Add(person);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidProductDataException))]
        public void Update_Error()
        {
            var person = new Person { Id = 1, Name = "" };
            _dataProvider.Add(new Person { Name = "Valid Name", ResponsibleForPurchase = true });
            _dataProvider.Update(person);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void GetById_Error()
        {
            _dataProvider.GetById(999);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void Update_Error404()
        {
            var person = new Person { Id = 999, Name = "Non-existent Person" };
            _dataProvider.Update(person);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void Delete_Error()
        {
            _dataProvider.Delete(999);
        }
    }
}

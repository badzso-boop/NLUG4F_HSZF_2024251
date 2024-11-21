using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    public class PersonCRUD : CRUDActions<Person>
    {
        HouseHoldDbContext context { get; set; }
        PersonDataProvider personData { get; set; }
        ProductDataProvider productData { get; set; }
        public PersonCRUD(HouseHoldDbContext houseHoldDbContext, PersonDataProvider personData, ProductDataProvider productData)
        {
            context = houseHoldDbContext;
            this.personData = personData;
            this.productData = productData;
        }

        static void Kiir(List<Person> people)
        {
            Console.WriteLine("Current people:");
            foreach (var person in people)
            {
                Console.WriteLine($"Id: {person.Id}, Name: {person.Name}, Responsible for purchase: {person.ResponsibleForPurchase}, Favorite products: {string.Join(", ", person.FavoriteProductIds)}");
                Console.WriteLine("----------------------");
            }
        }

        public List<Person> WriteAll()
        {   
            return personData.GetAll();
        }


        public void WriteOne(int personId)
        {
            try
            {
                Person personToWrite = personData.GetById(personId);
                Console.WriteLine($"Id: {personToWrite.Id}, Name: {personToWrite.Name}, Responsible for purchase: {personToWrite.ResponsibleForPurchase}, Favorite products: {string.Join(", ", personToWrite.FavoriteProductIds)}");
            }
            catch (PersonNotFoundException ex)
            {
                Console.WriteLine($"Person with ID {personId} not found: {ex.Message}");
            }
        }

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("Adding a new person...");

            Console.Write("Enter the person name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Responsible for purchase? (yes/no): ");
            string responsibleForPurchaseInput = Console.ReadLine()?.ToLower();
            bool responsibleForPurchase = responsibleForPurchaseInput == "yes" || responsibleForPurchaseInput == "y";


            List<int> ids = new List<int>();
            foreach (var product in productData.GetAll())
            {
                ids.Add(product.Id);
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Quantity: {product.Quantity}, Critical Level: {product.CriticalLevel}, Best Before: {product.BestBefore}");
                Console.WriteLine("----------------------");
            }

            List<int> FavoriteProductIds = new List<int>();
            int input;

            Console.WriteLine("Write a number from the product ids (-1 to end):");

            do
            {
                Console.Write("Product id: ");
                input = int.Parse(Console.ReadLine());

                if (input == -1)
                {
                    break;
                }

                if (ids.Contains(input))
                {
                    FavoriteProductIds.Add(input);
                }
                else
                {
                    Console.WriteLine("Invalid id, please enter a valid product id from the list.");
                }

            } while (true);

            Person newPerson = new Person(name, responsibleForPurchase, FavoriteProductIds);

            try
            {
                personData.Add(newPerson);
                Console.WriteLine("Person added successfully!");
            }
            catch (InvalidPersonDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine("Fetching all people...");

            var people = personData.GetAll();
            if (people.Count == 0)
            {
                Console.WriteLine("No person found.");
                return;
            }

            Kiir(people);

            Person personToUpdate = new Person();
            while (true)
            {
                Console.Write("Enter the ID of a person you want to update: ");

                if (!int.TryParse(Console.ReadLine(), out int personId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                try
                {
                    personToUpdate = personData.GetById(personId);
                    break;
                }
                catch (PersonNotFoundException ex)
                {
                    Console.WriteLine($"Product with ID {personId} not found: {ex.Message}");
                }
            }

            Console.Write($"Enter new name for {personToUpdate.Name} (or press Enter to keep it the same): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) personToUpdate.Name = newName;

            Console.Write("Responsible for purchase? (yes/no): ");
            string responsibleForPurchaseInput = Console.ReadLine()?.ToLower();
            bool responsibleForPurchase = responsibleForPurchaseInput == "yes" || responsibleForPurchaseInput == "y";
            personToUpdate.ResponsibleForPurchase = responsibleForPurchase;


            //ezt majd csinald meg :)
            personToUpdate.FavoriteProductIds = personToUpdate.FavoriteProductIds;

            try
            {
                personData.Update(personToUpdate);
                Console.WriteLine("Person updated successfully!");
            }
            catch (InvalidPersonDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("Fetching all person...");
            
            var people = personData.GetAll();
            if (people.Count == 0)
            {
                Console.WriteLine("No person found.");
                return;
            }

            Kiir(people);

            while (true)
            {
                Console.Write("Enter the ID of a person you want to delete: ");

                if (!int.TryParse(Console.ReadLine(), out int personId))
                {
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                    continue;
                }

                try
                {
                    personData.Delete(personId);
                    Console.WriteLine("Person deleted successfully!");
                    break;
                }
                catch (PersonNotFoundException ex)
                {
                    Console.WriteLine($"Product with ID {personId} not found: {ex.Message}");
                }
            }
        }
    }
}

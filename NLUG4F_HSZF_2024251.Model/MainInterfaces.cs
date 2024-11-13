using NLUG4F_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Model
{
    //IRepository<T>
    //Leírás: Általános interfész az adatbázis műveletekhez, különböző entitások kezelésére, például Fridge, Pantry, Persons, Products osztályokkal.

    //Metódusok:
    //Task<IEnumerable<T>> GetAll(): Az összes entitás lekérdezése.
    //Task<T?> GetById(int id): Egy entitás lekérdezése azonosító alapján.
    //void Add(T entity): Új entitás hozzáadása.
    //void Update(T entity): Létező entitás frissítése.
    //void Delete(int id): Entitás törlése azonosító alapján.

    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        T? GetById(int id);
        List<T> GetAll();
    }

    //IHouseholdMemberService
    //Leírás: A háztartási tagok kezelésére és kedvenc élelmiszereik kezelésére szolgáló interfész.
    //Metódusok:
    //Task AddMemberAsync(Person person): Új személy hozzáadása.
    //Task RemoveMemberAsync(int personId): Személy törlése.
    //Task<List<Person>> GetAllMembersAsync(): Az összes háztartási tag lekérdezése.
    //Task AddFavoriteFoodAsync(int personId, Product favoriteFood): Kedvenc élelmiszer hozzáadása egy személyhez.

    public interface IHouseholdMemberService
    {

    }
}

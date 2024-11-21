using NLUG4F_HSZF_2024251.Persistence.MsSql;

namespace NLUG4F_HSZF_2024251.Applicaion
{
    //INotificationService
    //Leírás: Események elsütése és értesítések küldése a háztartás tagjainak.
    //Metódusok:
    //void NotifyPerson(Person person, string message): Egy konkrét személy értesítése.
    //void NotifyAllMembers(string message): Az összes háztartás tagjának értesítése.
    //void NotifyResponsiblePerson(string message): A beszerzésért felelős személy értesítése.
    //Task NotifyFavoriteFoodArrivalAsync(Product product): Kedvenc élelmiszer érkezése esetén értesítések küldése az érintett személyeknek.

    public interface INotificationService
    {

    }

    public interface CRUDActions<T>
    {
        List<T> WriteAll();
        void WriteOne(int id);
        void Add();
        void Update();
        void Delete();
    }
}

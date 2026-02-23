using ContactManager.Models;

namespace ContactManager.Interfaces
{
    public interface IContactService
    {
        void AddContact(string name, string phone, string email);
        void EditContact(int id, string name, string phone, string email);
        void DeleteContact(int id);
        Contact? GetContact(int id);
        List<Contact> GetAllContacts();
        List<Contact> Search(string query);
        List<Contact> Filter(string criteria);
        Task LoadAsync();
        Task SaveAsync();
    }
}
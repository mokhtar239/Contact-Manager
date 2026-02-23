using ContactManager.Models;

namespace ContactManager.Interfaces
{
    public interface IContactRepository
    {
        Task<List<Contact>> LoadAllAsync();
        Task SaveAllAsync(List<Contact> contacts);
    }
}
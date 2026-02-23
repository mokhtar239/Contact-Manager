using ContactManager.Interfaces;
using ContactManager.Models;

namespace ContactManager.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private List<Contact> _contacts = new();

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadAsync()
        {
            _contacts = await _repository.LoadAllAsync();
        }

        private int GenerateId()
        {
            if (_contacts.Count == 0)
                return 1;
            return _contacts.Max(c => c.Id) + 1;
        }

        public void AddContact(string name, string phone, string email)
        {
            var contact = new Contact(name, phone, email);
            contact.Id = GenerateId();
            _contacts.Add(contact);
        }

        public void EditContact(int id, string name, string phone, string email)
        {
            var contact = GetContact(id);
            if (contact == null)
                throw new Exception($"Contact with Id {id} not found.");

            if (!string.IsNullOrWhiteSpace(name))
                contact.Name = name;
            if (!string.IsNullOrWhiteSpace(phone))
                contact.Phone = phone;
            if (!string.IsNullOrWhiteSpace(email))
                contact.Email = email;
        }

        public void DeleteContact(int id)
        {
            var contact = GetContact(id);
            if (contact == null)
                throw new Exception($"Contact with Id {id} not found.");

            _contacts.Remove(contact);
        }

        public Contact? GetContact(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }

        public List<Contact> GetAllContacts()
        {
            return _contacts;
        }

        public List<Contact> Search(string query)
        {
            string lower = query.ToLower();
            return _contacts.Where(c =>
                c.Name.ToLower().Contains(lower) ||
                c.Phone.ToLower().Contains(lower) ||
                c.Email.ToLower().Contains(lower)
            ).ToList();
        }

        public List<Contact> Filter(string criteria)
        {
            return _contacts.Where(c =>
                c.Name.StartsWith(criteria, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public async Task SaveAsync()
        {
            await _repository.SaveAllAsync(_contacts);
        }
    }
}

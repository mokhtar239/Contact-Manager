using System.Text.Json;
using ContactManager.Interfaces;
using ContactManager.Models;

namespace ContactManager.Repositories
{
    public class JsonContactRepository : IContactRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonContactRepository(string filePath = "contacts.json")
        {
            _filePath = filePath;
            _options = new JsonSerializerOptions { WriteIndented = true };
        }

        public async Task<List<Contact>> LoadAllAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Contact>();

            string json = await File.ReadAllTextAsync(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<Contact>();

            return JsonSerializer.Deserialize<List<Contact>>(json, _options)
                   ?? new List<Contact>();
        }

        public async Task SaveAllAsync(List<Contact> contacts)
        {
            string json = JsonSerializer.Serialize(contacts, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}

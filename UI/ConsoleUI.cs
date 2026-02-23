using ContactManager.Interfaces;
using ContactManager.Models;
using ContactManager.Validators;

namespace ContactManager.UI
{
    public class ConsoleUI
    {
        private readonly IContactService _service;

        public ConsoleUI(IContactService service)
        {
            _service = service;
        }

        public async Task RunAsync()
        {
            await _service.LoadAsync();

            var contacts = _service.GetAllContacts();
            Console.WriteLine($"\nLoaded {contacts.Count} contact(s).");

            if (contacts.Count > 0)
                DisplayContactTable(contacts);

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = ReadInput("\nSelect an option: ");

                switch (choice)
                {
                    case "1": AddContact(); break;
                    case "2": EditContact(); break;
                    case "3": DeleteContact(); break;
                    case "4": ViewContact(); break;
                    case "5": ListContacts(); break;
                    case "6": SearchContacts(); break;
                    case "7": FilterContacts(); break;
                    case "8": await SaveContacts(); break;
                    case "9": running = await ExitApp(); break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("\n===== Contact Manager =====");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. View Contact");
            Console.WriteLine("5. List Contacts");
            Console.WriteLine("6. Search");
            Console.WriteLine("7. Filter");
            Console.WriteLine("8. Save");
            Console.WriteLine("9. Exit");
            Console.WriteLine("===========================");
        }

        private void AddContact()
        {
            Console.WriteLine("\n--- Add Contact ---");

            string name = ReadValidated("Name: ", ContactValidator.ValidateName);
            string phone = ReadValidated("Phone: ", ContactValidator.ValidatePhone);
            string email = ReadValidated("Email: ", ContactValidator.ValidateEmail);

            _service.AddContact(name, phone, email);
            Console.WriteLine("Contact added successfully.");
        }

        private void EditContact()
        {
            Console.WriteLine("\n--- Edit Contact ---");

            var contacts = _service.GetAllContacts();
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts to edit.");
                return;
            }

            DisplayContactTable(contacts);

            int id = ReadId("\nEnter contact Id to edit: ");
            var contact = _service.GetContact(id);
            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine($"Editing: {contact.Name} | {contact.Phone} | {contact.Email}");
            Console.WriteLine("(Press Enter to keep current value)\n");

            string name = ReadOptionalValidated($"Name [{contact.Name}]: ", ContactValidator.ValidateName);
            string phone = ReadOptionalValidated($"Phone [{contact.Phone}]: ", ContactValidator.ValidatePhone);
            string email = ReadOptionalValidated($"Email [{contact.Email}]: ", ContactValidator.ValidateEmail);

            _service.EditContact(id, name, phone, email);
            Console.WriteLine("Contact updated successfully.");
        }

        private void DeleteContact()
        {
            Console.WriteLine("\n--- Delete Contact ---");

            var contacts = _service.GetAllContacts();
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts to delete.");
                return;
            }

            DisplayContactTable(contacts);

            int id = ReadId("\nEnter contact Id to delete: ");
            var contact = _service.GetContact(id);
            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine($"Are you sure you want to delete '{contact.Name}'?");
            string confirm = ReadInput("Type 'yes' to confirm: ");

            if (confirm.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                _service.DeleteContact(id);
                Console.WriteLine("Contact deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }
        }

        private void ViewContact()
        {
            Console.WriteLine("\n--- View Contact ---");

            int id = ReadId("Enter contact Id: ");
            var contact = _service.GetContact(id);
            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            DisplaySingleContact(contact);
        }

        private void ListContacts()
        {
            Console.WriteLine("\n--- All Contacts ---");
            var contacts = _service.GetAllContacts();

            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts found.");
                return;
            }

            DisplayContactTable(contacts);
        }

        private void SearchContacts()
        {
            Console.WriteLine("\n--- Search Contacts ---");
            Console.WriteLine("You can search by name, phone, or email.");
            string query = ReadInput("Enter name or phone or email: ");

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Search query cannot be empty.");
                return;
            }

            var results = _service.Search(query);

            if (results.Count == 0)
            {
                Console.WriteLine("No matching contacts found.");
                return;
            }

            Console.WriteLine($"Found {results.Count} result(s):");
            DisplayContactTable(results);
        }

        private void FilterContacts()
        {
            Console.WriteLine("\n--- Filter Contacts ---");
            string criteria = ReadInput("Filter by name starting with: ");

            if (string.IsNullOrWhiteSpace(criteria))
            {
                Console.WriteLine("Filter criteria cannot be empty.");
                return;
            }

            var results = _service.Filter(criteria);

            if (results.Count == 0)
            {
                Console.WriteLine("No matching contacts found.");
                return;
            }

            Console.WriteLine($"Found {results.Count} result(s):");
            DisplayContactTable(results);
        }

        private async Task SaveContacts()
        {
            await _service.SaveAsync();
            Console.WriteLine("Contacts saved successfully.");
        }

        private async Task<bool> ExitApp()
        {
            string answer = ReadInput("Save before exiting? (yes/no): ");
            if (answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                await _service.SaveAsync();
                Console.WriteLine("Contacts saved.");
            }

            Console.WriteLine("Goodbye!");
            return false;
        }

        // --- Display Helpers ---

        private void DisplayContactTable(List<Contact> contacts)
        {
            Console.WriteLine();
            Console.WriteLine($"{"Id",-5}{"Name",-20}{"Phone",-18}{"Email",-40}{"Created",-20}");
            Console.WriteLine(new string('-', 103));

            foreach (var c in contacts)
            {
                Console.WriteLine($"{c.Id,-5}{c.Name,-20}{c.Phone,-18}{c.Email,-40}{c.CreatedAt:yyyy-MM-dd HH:mm}");
            }
        }

        private void DisplaySingleContact(Contact c)
        {
            Console.WriteLine($"\n  Id:      {c.Id}");
            Console.WriteLine($"  Name:    {c.Name}");
            Console.WriteLine($"  Phone:   {c.Phone}");
            Console.WriteLine($"  Email:   {c.Email}");
            Console.WriteLine($"  Created: {c.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        }

        // --- Input Helpers ---

        private string ReadInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private int ReadId(string prompt)
        {
            while (true)
            {
                string input = ReadInput(prompt);
                if (int.TryParse(input, out int id) && id > 0)
                    return id;
                Console.WriteLine("Please enter a valid positive number.");
            }
        }

        private string ReadValidated(string prompt, Func<string, (bool IsValid, string Error)> validate)
        {
            while (true)
            {
                string input = ReadInput(prompt);
                var result = validate(input);
                if (result.IsValid)
                    return input;
                Console.WriteLine($"  Error: {result.Error}");
            }
        }

        private string ReadOptionalValidated(string prompt, Func<string, (bool IsValid, string Error)> validate)
        {
            while (true)
            {
                string input = ReadInput(prompt);

                // Empty means keep current value
                if (string.IsNullOrWhiteSpace(input))
                    return string.Empty;

                var result = validate(input);
                if (result.IsValid)
                    return input;
                Console.WriteLine($"  Error: {result.Error}");
            }
        }
    }
}

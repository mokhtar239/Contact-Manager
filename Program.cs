using ContactManager.Interfaces;
using ContactManager.Repositories;
using ContactManager.Services;
using ContactManager.UI;

IContactRepository repository = new JsonContactRepository();
IContactService service = new ContactService(repository);
ConsoleUI ui = new ConsoleUI(service);

await ui.RunAsync();

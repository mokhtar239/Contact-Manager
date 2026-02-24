# Contact Manager CLI

A command-line Contact Management System built with C# applying OOP and SOLID principles. Uses JSON file storage for persistence.

## Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later

## How to Run

**```bash
cd ContactManager
dotnet run
```**

## Menu Options

### 1. Add Contact
Allows the user to create a new contact. The system will prompt for:
- **Name** — Must be at least 2 characters long and cannot be empty.
- **Phone** — Must be 7 to 15 digits, with an optional `+` prefix (e.g. `+201234567890`).
- **Email** — Must be a valid email format (e.g. `user@example.com`).

Each field is validated before proceeding. If the input is invalid, an error message is shown and the user is asked to re-enter. The **Id** and **Creation Date** are generated automatically.

### 2. Edit Contact
Allows the user to update an existing contact. The system displays all contacts in a table, then asks for the **Id** of the contact to edit. For each field (Name, Phone, Email), the current value is shown in brackets. The user can:
- Type a new value to update it (same validation rules as Add).
- Press **Enter** to keep the current value unchanged.

### 3. Delete Contact
Allows the user to remove a contact. The system displays all contacts in a table, then asks for the **Id** of the contact to delete. A confirmation prompt (`Type 'yes' to confirm`) is shown before deletion to prevent accidental removal.

### 4. View Contact
Displays the full details of a single contact. The user enters the **Id**, and the system shows:
- Id
- Name
- Phone
- Email
- Creation Date (with date and time)

### 5. List Contacts
Displays all stored contacts in a formatted table with columns: **Id**, **Name**, **Phone**, **Email**, and **Created** date. If there are no contacts, a message is shown indicating the list is empty.

### 6. Search
Searches across all contacts by **name**, **phone**, or **email**. The user enters a search query and the system performs a case-insensitive search. Any contact whose name, phone, or email contains the query text will be returned. Results are displayed in a table.

### 7. Filter
Filters contacts by the starting letters of their **name**. For example, entering `A` will show all contacts whose names start with "A" (case-insensitive). Results are displayed in a table.

### 8. Save
Saves all current contacts to `contacts.json`. Any additions, edits, or deletions made during the session are persisted to the file. Without saving, changes exist only in memory and will be lost on exit.

### 9. Exit
Exits the application. Before closing, the user is asked `Save before exiting? (yes/no)`. Choosing **yes** saves all changes to the JSON file before quitting. Choosing **no** exits without saving.

## Architecture

```
ContactManager/
├── Models/          → Contact entity
├── Interfaces/      → IContactRepository, IContactService
├── Repositories/    → JsonContactRepository (JSON file I/O)
├── Services/        → ContactService (business logic)
├── Validators/      → ContactValidator (input validation)
├── UI/              → ConsoleUI (menu and user interaction)
└── Program.cs       → Composition root
```

### SOLID Principles Applied

| Principle | Implementation |
|-----------|---------------|
| Single Responsibility | Each class has one focused purpose |
| Open/Closed | New storage backends can be added without modifying existing code |
| Liskov Substitution | Any IContactRepository implementation is interchangeable |
| Interface Segregation | Separate interfaces for data access and business logic |
| Dependency Inversion | Service depends on abstractions (interfaces), not concrete classes |

## Data Storage

Contacts are stored in `contacts.json` in the application directory. The file is created automatically on first save.

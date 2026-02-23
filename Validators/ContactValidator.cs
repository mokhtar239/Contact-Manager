using System.Text.RegularExpressions;

namespace ContactManager.Validators
{
    public static class ContactValidator
    {
        public static (bool IsValid, string Error) ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Name cannot be empty.");
            if (name.Length < 2)
                return (false, "Name must be at least 2 characters.");
            return (true, string.Empty);
        }

        public static (bool IsValid, string Error) ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return (false, "Phone cannot be empty.");
            if (!Regex.IsMatch(phone, @"^\+?[0-9]{7,15}$"))
                return (false, "Phone must be 7-15 digits (optional + prefix).");
            return (true, string.Empty);
        }

        public static (bool IsValid, string Error) ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email cannot be empty.");
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return (false, "Invalid email format.");
            return (true, string.Empty);
        }
    }
}

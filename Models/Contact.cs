namespace ContactManager.Models
{
    public class Contact
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        
        public Contact() { }

        
        public Contact(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
            CreatedAt = DateTime.Now;  
        }
    }
}

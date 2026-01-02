namespace ProjektIznynierski.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public string Pesel { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}

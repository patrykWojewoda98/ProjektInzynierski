namespace ProjektIznynierski.Application.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsAdmin { get; set; }
        public string Pesel { get; set; }
    }
}

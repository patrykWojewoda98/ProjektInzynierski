using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Domain.Entities
{
    public class ClientInterfaceConfig : BaseEntity
    {
        public ClientInterfacePlatform Platform { get; set; }
        public ClientInterfaceType InterfaceType { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int OrderIndex { get; set; }
        public bool IsVisible { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
        public Employee? ModifiedByEmployee { get; set; }
    }
}

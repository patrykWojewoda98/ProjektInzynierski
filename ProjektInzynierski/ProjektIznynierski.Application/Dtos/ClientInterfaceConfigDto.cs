using ProjektIznynierski.Domain.Enums;

namespace ProjektIznynierski.Application.Dtos
{
    public class ClientInterfaceConfigDto
    {
        public int Id { get; set; }
        public ClientInterfacePlatform Platform { get; set; }
        public ClientInterfaceType InterfaceType { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int OrderIndex { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? ModifiedByEmployeeId { get; set; }
    }

    /// <summary>
    /// Simplified DTO for client menu (visible items only, ordered).
    /// </summary>
    public class ClientMenuConfigItemDto
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int OrderIndex { get; set; }
    }
}

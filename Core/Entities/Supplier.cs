using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Core.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string CNPJ { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryControl.Core.Entities
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string SKU { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Range(0.01, double.MaxValue), Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        
        public Supplier? Supplier { get; set; }
    }
}
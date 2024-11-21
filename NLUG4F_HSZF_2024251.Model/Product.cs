using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class Product : IEntity
    {
        public Product(string name, decimal quantity, decimal criticalLevel, DateTime bestBefore, bool storeInFridge)
        {
            Name = name;
            Quantity = quantity;
            CriticalLevel = criticalLevel;
            BestBefore = bestBefore;
            StoreInFridge = storeInFridge;
        }

        public Product()
        {
            
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal CriticalLevel { get; set; }
        [Required]
        public DateTime BestBefore { get; set; }
        [Required]
        public bool StoreInFridge { get; set; }

        // Navigation Property
        public int? FridgeId { get; set; }
        public Fridge Fridge { get; set; }

        public int? PantryId { get; set; }
        public Pantry Pantry { get; set; }
    }
}

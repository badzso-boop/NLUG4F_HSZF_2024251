using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class Fridge : IEntity
    {
        public Fridge(int capacity)
        {
            Capacity = capacity;
            Products = new List<Product>();
        }

        public Fridge(int capacity, List<Product> productsToAdd)
        {
            Capacity = capacity;
            Products = productsToAdd;
        }

        public Fridge()
        {
            Products = new List<Product>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Capacity { get; set; }

        public List<Product> Products { get; set; }
    }
}

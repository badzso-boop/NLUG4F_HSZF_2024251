using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLUG4F_HSZF_2024251.Persistence.MsSql
{
    public class Person : IEntity
    {
        public Person(string name, bool responsibleForPurchase)
        {
            Name = name;
            ResponsibleForPurchase = responsibleForPurchase;
            FavoriteProductIds = new List<int>();
        }

        public Person(string name, bool responsibleForPurchase, List<int> FavoriteProductIdsInput)
        {
            Name = name;
            ResponsibleForPurchase = responsibleForPurchase;
            FavoriteProductIds = FavoriteProductIdsInput;
        }

        public Person()
        {
            FavoriteProductIds = new List<int>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public bool ResponsibleForPurchase { get; set; }
        public List<int> FavoriteProductIds { get; set; }
    }
}

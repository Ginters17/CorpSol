using System.ComponentModel.DataAnnotations;

namespace CorpSol.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}

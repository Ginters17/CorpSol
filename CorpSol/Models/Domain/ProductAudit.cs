using System.ComponentModel.DataAnnotations;

namespace CorpSol.Models.Domain
{
    public class ProductAudit
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ChangedByUserId { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; }

        [Required]
        public string ChangeType { get; set; } = null!;

        public string Changes { get; set; } = null!;
    }
}

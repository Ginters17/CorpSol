using System.ComponentModel.DataAnnotations;

namespace CorpSol.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}

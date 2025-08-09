using CorpSol.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace CorpSol.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Username { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
    }
}

using CorpSol.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace CorpSol.Models.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Username { get; set; } = "";

        public string PasswordHash { get; set; } = "";

        [Required]
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
    }
}

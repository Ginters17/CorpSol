using CorpSol.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CorpSol.Services
{
    public interface IAuthService
    {
        Task<IActionResult> RegisterAsync(RegisterRequestDTO request);
        IActionResult Login(LoginRequestDTO request);
    }
}

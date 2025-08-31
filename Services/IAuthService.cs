using Scholarship_Plaatform_Backend.Models;

namespace Scholarship_Plaatform_Backend.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registration(User model, string role);
        Task<(int, string)> Login(LoginModel model);
    }
}

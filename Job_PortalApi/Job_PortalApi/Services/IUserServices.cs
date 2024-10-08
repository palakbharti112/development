using Job_PortalApi.Models;
using System.Security.Claims;

namespace Job_PortalApi.Services
{
    public interface IUserServices
    {
        Task<Usermodel> RegisterAsync(Usermodel user);
        Task<loginresponse> LoginAsync(Loginresponsemodel userlogin);
        Task<IEnumerable<Usermodel>> GetAllUsersAsync();
        //Task GetTokenValue(ClaimsIdentity identity);
        Task<Usermodel> GetUserByIdAsync(int id);
        Task<Usermodel> UpdateUserAsync(Usermodel user,int userId);
    }
}

using Job_PortalApi.Models;

namespace Job_PortalApi.Services
{
    public interface IProfileServices
    {
        Task<UserProfile> GetUserProfileByIdAsync(int userId);
        Task AddUserProfileAsync(userProfilemodel userProfile);
        Task UpdateUserProfileAsync(userProfilemodel userProfile, int id);
        Task DeleteUserProfileAsync(int userId);

        Task<Employer> GetEmployerProfileByIdAsync(int employerId);
        Task AddEmployerProfileAsync(EmployerProfileModel employerProfile);
        Task UpdateEmployerProfileAsync(EmployerProfileModel employerProfile,int id);
        Task DeleteEmployerProfileAsync(int employerId);
    }
}

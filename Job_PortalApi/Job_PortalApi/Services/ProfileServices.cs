using AutoMapper;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Job_PortalApi.Services
{
    public class ProfileServices: IProfileServices
    {
        private readonly IRepository<UserProfile> _profilerepository;
        private readonly IRepository<Employer> _employerProfileRepository;
        private readonly IMapper _mapper;
       

        public ProfileServices(IRepository<UserProfile> profilerepository,IRepository<Employer> employerProfileRepository,IMapper mapper)
        {
            _profilerepository = profilerepository;
            _employerProfileRepository = employerProfileRepository;
            _mapper = mapper;
            
        }

        public async  Task AddUserProfileAsync(userProfilemodel userProfile)
        {

            var profileEntity = _mapper.Map<UserProfile>(userProfile);
            profileEntity.CreatedAt = DateTime.UtcNow;
            await _profilerepository.AddAsync(profileEntity);
            await _profilerepository.SaveChangesAsync();
        }

        

        public async Task DeleteUserProfileAsync(int userId)
        {
            var userProfile = await _profilerepository.GetEntityByConditionAsync(x => x.UserId == userId && x.IsDeleted != true);

         
            if (userProfile == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
           
            userProfile.IsDeleted = true;
         await   _profilerepository.UpdateAsync(userProfile);
            await _profilerepository.SaveChangesAsync();
            
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(int userId)
        {
            return await _profilerepository.GetEntityByConditionAsync
                (up => up.UserId == userId);

        }
        public async Task UpdateUserProfileAsync(userProfilemodel userProfile,int id)
        {
            var existingProfile = await _profilerepository.GetByIdAsync(id);


            if (existingProfile == null)
            {

                throw new KeyNotFoundException("profile not found.");
            }
            _mapper.Map(userProfile, existingProfile);
            existingProfile.UpdatedAt = DateTime.UtcNow;
            await _profilerepository.UpdateAsync(existingProfile);
            await _profilerepository.SaveChangesAsync();
            
        }
        //Employer Profile
        public async Task AddEmployerProfileAsync(EmployerProfileModel employerProfile)
        {
            var profileEntity = _mapper.Map<Employer>(employerProfile);
            profileEntity.CreatedAt = DateTime.UtcNow;

            await _employerProfileRepository.AddAsync(profileEntity);
            await _employerProfileRepository.SaveChangesAsync();
        }

        public async Task DeleteEmployerProfileAsync(int employerId)
        {

            var EmployerProfile = await _employerProfileRepository.GetEntityByConditionAsync(x => x.UserId == employerId && x.Isdeleted != true);

            if (EmployerProfile == null)
            {
                throw new KeyNotFoundException($"User with ID {employerId} not found.");
            }

            EmployerProfile.Isdeleted = true;
            await _employerProfileRepository.UpdateAsync(EmployerProfile);
            await _employerProfileRepository.SaveChangesAsync();

        }
        public async Task<Employer> GetEmployerProfileByIdAsync(int employerId)
        {
            return await _employerProfileRepository.GetEntityByConditionAsync
             (up => up.UserId == employerId);

        }

        public async  Task UpdateEmployerProfileAsync(EmployerProfileModel employerProfile,int id)
        {
            var existingProfile = await _employerProfileRepository.GetByIdAsync(id);


            if (existingProfile == null)
            {

                throw new KeyNotFoundException("Employer profile not found.");
            }
            _mapper.Map(existingProfile, employerProfile);
            existingProfile.UpdatedAt = DateTime.UtcNow;
            await _employerProfileRepository.UpdateAsync(existingProfile);
            await _employerProfileRepository.SaveChangesAsync();

        }

      
    }
}

using AutoMapper;
using Job_PortalApi.Models;

namespace Job_PortalApi
{
    public class AutomapperProfile:Profile
    {
        public AutomapperProfile()
        {
           // CreateMap<Usermodel, User>();
            CreateMap<User, Usermodel>().ReverseMap();
            CreateMap<UserProfile, userProfilemodel>().ReverseMap();
            CreateMap<Usermodel, User>()
    .ForMember(dest => dest.UserId, opt => opt.Ignore()); // Ignore UserId
            CreateMap<EmployerProfileModel, Employer>()
          .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Managed explicitly
          .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Managed explicitly

            CreateMap<Employer, EmployerProfileModel>();
            CreateMap<Job, JobModel>().ReverseMap();
        }


    }
}

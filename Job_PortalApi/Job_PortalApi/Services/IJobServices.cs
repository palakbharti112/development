using Job_PortalApi.Models;

namespace Job_PortalApi.Services
{
    public interface IJobServices
    {

        Task<Job> GetJobByIdAsync(int jobId);

     
        Task<IEnumerable<Job>> GetAllJobsAsync();

       
        Task<Job> GetJobsByEmployerIdAsync(int employerId);

        
        Task AddJobAsync(JobModel job);

        
        Task UpdateJobAsync(JobModel job, int id);

      
        Task DeleteJobAsync(int jobId);
        
    }
}

using AutoMapper;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Microsoft.EntityFrameworkCore;

namespace Job_PortalApi.Services
{
    public class JobServices:IJobServices
    {
        private readonly IRepository<Job> _jobrepository;
        private readonly IMapper _mapper;

        public JobServices(IRepository<Job> jobrepository, IMapper mapper)
        {
            _jobrepository = jobrepository;
            _mapper = mapper;
        }

        public  async Task   AddJobAsync(JobModel jobmodel)
        {

            var jobEntity = _mapper.Map<Job>(jobmodel);
            jobEntity.CreatedAt = DateTime.UtcNow;
            await _jobrepository.AddAsync(jobEntity);
            await _jobrepository.SaveChangesAsync();
        }

        public async  Task DeleteJobAsync(int jobId)
        {

            var jobEntity = await _jobrepository.GetEntityByConditionAsync(x => x.Id == jobId && x.IsDeleted != true);

            if (jobEntity == null)
            {
                throw new KeyNotFoundException($"User with ID {jobId} not found.");
            }

           jobEntity.IsDeleted = true;
            await _jobrepository.UpdateAsync(jobEntity);
            await _jobrepository.SaveChangesAsync();
        }

        public async  Task<IEnumerable<Job>> GetAllJobsAsync()
        {

            return await _jobrepository.GetAllAsync();
                
        }

        public async Task<Job> GetJobByIdAsync(int jobId)
        {
            return await _jobrepository.GetEntityByConditionAsync
             (up => up.Id == jobId && up.IsDeleted!=true);
        }

        public  async  Task<Job> GetJobsByEmployerIdAsync(int employerId)
        {

            return await _jobrepository.GetEntityByConditionAsync
             (up => up.EmployerId == employerId);
        }

      

        public async Task UpdateJobAsync(JobModel jobmodel,int id)
        {
            var existingJob = await _jobrepository.GetByIdAsync(id);


            if (existingJob == null)
            {

                throw new KeyNotFoundException("Employer profile not found.");
            }
            _mapper.Map(existingJob, jobmodel);
            existingJob.UpdatedAt = DateTime.UtcNow;
            await _jobrepository.UpdateAsync(existingJob);
            await _jobrepository.SaveChangesAsync();
        }
    }
}

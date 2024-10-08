using AutoMapper;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Job_PortalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Job_PortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
      
        private readonly IMapper _mapper;
        private readonly IJobServices _jobServices;

        public JobController( IMapper mapper,IJobServices jobServices)
        {
            
            _mapper = mapper;
            _jobServices = jobServices;
        }
        [HttpPost("AddJobs")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AddJobs(JobModel jobModel)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            jobModel.EmployerId = userId;
            var existingJob = await _jobServices.GetJobByIdAsync(jobModel.Id);
            var response = new GeneralResponseModel() { };
            if (existingJob == null)
            {
                    var userprofile = _jobServices.AddJobAsync(jobModel);
                    response.Status = true;
                    response.Message = ErrorMessages.SuccessAdded;
                    response.Data = existingJob;
            }
            else
            {
                response.Status = true;
                response.Message = ErrorMessages.AlreadyExist;
                response.Data = null;
            }
          return Ok(response);
           }
        [HttpPost("UpdateJobs")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJobs(JobModel jobModel)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            jobModel.EmployerId = userId;
            var existingJob = await _jobServices.GetJobByIdAsync(jobModel.Id);
            var response = new GeneralResponseModel() { };
            if (existingJob != null)
            {
                _mapper.Map(jobModel, existingJob);

                existingJob.UpdatedAt = DateTime.UtcNow;
                await _jobServices.UpdateJobAsync(jobModel,jobModel.Id);

                response.Status = true;
                response.Message = " Job updated successfully.";
                response.Data = existingJob;

            }
            else
            {
                response.Status = true;
                response.Message = ErrorMessages.AlreadyExist;
                response.Data = null;
            }
            return Ok(response);
        }
        [HttpDelete("DeleteJobs/{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> DeleteJobs(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            var existingJob = await _jobServices.GetJobByIdAsync(id);
           
            var response = new GeneralResponseModel() { };
            if (existingJob != null)
            {
              existingJob.DeletedAt = DateTime.UtcNow;
                await _jobServices.DeleteJobAsync(id);
                response.Status = true;
                response.Message = " Job Deleted successfully.";
                response.Data = existingJob;

            }
            else
            {
                response.Status = true;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }
            
            return Ok(response);
        }
        [HttpGet("GetJobById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetJobById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            var existingJob = await _jobServices.GetJobByIdAsync(id);
            var response = new GeneralResponseModel() { };
            if (existingJob != null)
            {
                response.Status = true;
                response.Message = "Job by Id fetched successfully.";
                response.Data = existingJob;

            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }
            return Ok(response);
        }
    }
}

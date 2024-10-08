using AutoMapper;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Job_PortalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Job_PortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IProfileServices _profileServices;

        public ProfileController(IMapper mapper, IConfiguration configuration, IProfileServices profileServices)
        {
            _mapper = mapper;
            _configuration = configuration;
            _profileServices = profileServices;
        }
        [HttpPost("AddUserProfile")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> AddUserProfile(userProfilemodel userProfilemodel)
        {
            if (userProfilemodel == null)
            {
                return BadRequest();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            userProfilemodel.UserId = userId;
            var response = new GeneralResponseModel();
            var existingUser = await _profileServices.GetUserProfileByIdAsync(userId);
            if (existingUser == null)
            {
                var userprofile = _profileServices.AddUserProfileAsync(userProfilemodel);
                response.Status = true;
                response.Message = ErrorMessages.SuccessAdded;
                response.Data = userprofile;

            }
            else
            {
                _mapper.Map(existingUser, userProfilemodel);
                await _profileServices.UpdateUserProfileAsync(userProfilemodel, userProfilemodel.Id);

                response.Status = true;
                response.Message = "User updated successfully.";
                response.Data = existingUser;
            }
            return Ok(response);
        }
        [HttpGet("GetProfileById")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> GetUserProfileById()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            var existingUser = await _profileServices.GetUserProfileByIdAsync(userId);
            var response = new GeneralResponseModel() { };
            if (existingUser != null)
            {
                response.Status = true;
                response.Message = "User Data by Id fetched successfully.";
                response.Data = existingUser;

            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }
                return Ok(response);
        }
        [HttpDelete("DeleteUserProfile")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DeleteProfileById()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            var existingUser = await _profileServices.GetUserProfileByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            var deletedProfile = _profileServices.DeleteUserProfileAsync(userId);
            if (deletedProfile != null)
            {
                return Ok(new { message = "User profile deleted successfully." });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting user profile.");

        }

        //Employer Apis for Profile
        [HttpPost("AddEmployerProfile")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AddEmployerProfile(EmployerProfileModel employerProfileModel)
        {
            if (employerProfileModel == null)
            {
                return BadRequest();
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            employerProfileModel.UserId = userId;
            var existingUser = await _profileServices.GetEmployerProfileByIdAsync(userId);
            var response = new GeneralResponseModel() { };
            if (existingUser == null)
            {
                var employerprofile= _profileServices.AddEmployerProfileAsync(employerProfileModel);
                response.Status = true;
                response.Message = ErrorMessages.SuccessAdded;
                response.Data = employerprofile;
              

                
            }
            else
            {
                _mapper.Map(existingUser, employerProfileModel);
                await _profileServices.UpdateEmployerProfileAsync(employerProfileModel, employerProfileModel.Id);
                response.Status = true;
                response.Message = "User Profile updated successfully.";
                response.Data = existingUser;
            }
            return Ok(response);
        }
        [HttpGet("GetEmployerProfile")]
        [Authorize(Roles = "Employer")]
        public async Task <IActionResult> GetEmployerProfileById()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);
            var existingUser = await _profileServices.GetEmployerProfileByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound("User profile not found.");
            }

            var response = new GeneralResponseModel() { };
            if (existingUser != null)
            {
                response.Status = true;
                response.Message = "User Data by Id fetched successfully.";
                response.Data = existingUser;

            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }

            // Return the user profile
            return Ok(response);
        }
    }
}
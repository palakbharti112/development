using AutoMapper;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Job_PortalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Job_PortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public UserController(IConfiguration configuration,IMapper mapper,IUserServices userServices)
        {
            
            _configuration = configuration;
            _mapper = mapper;
            _userServices = userServices;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Usermodel user)
        {
            var response = new GeneralResponseModel() { };
            var registeredUser = await _userServices.RegisterAsync(user);
            if (registeredUser!=null)
            {
                response.Status = true;
                response.Message = ErrorMessages.SuccessAdded;
                response.Data = registeredUser;
            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }
            return Ok(response);
        }
            [HttpPost("Login")]
        public async Task<IActionResult> Login(Loginresponsemodel userlogin)
        {
            var response = new GeneralResponseModel() { };
            var user = await _userServices.LoginAsync(userlogin);
            string token = user.Token; 
          
            if (user != null)
            {
                response.Status = true;
                response.Message = ErrorMessages.SuccessAdded;
                response.Token = token;
                response.Data = user;
            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }
            return Ok(response);
        }
      
        [HttpGet("GetAllusers")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetAllUsers()
        {

            var users = await _userServices.GetAllUsersAsync();
            var response = new GeneralResponseModel() { };
            if (users != null)
            {
                response.Status = true;
                response.Message = ErrorMessages.SuccessDataFetched;
                response.Data = users;
            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
            }
            return Ok(response);
        }
        [HttpPut("UpdateUser")]
        [Authorize]
        public  async Task<IActionResult> UpdateUser(Usermodel usermodel)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token or user not logged in.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var updatedUser = await _userServices.UpdateUserAsync(usermodel, userId);
            var response = new GeneralResponseModel() { };
            if (updatedUser != null)
            {
                response.Status = true;
                response.Message = "User updated successfully.";
                response.Data = updatedUser;
                
            }
            else
            {
                response.Status = false;
                response.Message = ErrorMessages.SomeError;
                response.Data = null;
           
            }
            return Ok(response);
        }
        [HttpGet("GetUserById")]
        [Authorize]
        public async Task<IActionResult> GetOneUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token or user not logged in.");
            }
            var userId = int.Parse(userIdClaim.Value);
            var user = await _userServices.GetUserByIdAsync(userId);
            var response = new GeneralResponseModel() { };
            if (user != null)
            {
                response.Status = true;
                response.Message = "User Data by Id fetched successfully.";
                response.Data = user;

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

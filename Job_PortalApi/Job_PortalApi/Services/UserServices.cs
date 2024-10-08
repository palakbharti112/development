using AutoMapper;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Plugins;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Job_PortalApi.Services
{
    public class UserServices:IUserServices
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserServices(IRepository<User> userRepository,IMapper mapper,IConfiguration configuration)
        {
           _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Usermodel>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<Usermodel>>(users);

        }

        public async Task<Usermodel> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {

                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return _mapper.Map<Usermodel>(user);
        }

        public async Task<loginresponse> LoginAsync(Loginresponsemodel userlogin)
        {

            
                if (userlogin == null || string.IsNullOrWhiteSpace(userlogin.Email) || string.IsNullOrWhiteSpace(userlogin.PasswordHash))
                {
                    throw new ArgumentException("Invalid login request. Email and password are required.");
                }
                var user = await _userRepository.GetEntityByConditionAsync(u => u.Email == userlogin.Email);
                if (user == null)
                {
                    throw new ArgumentException("Data not found of User");
                }
            
                if (string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                throw new ArgumentException("Password hash not found");
                }
                if (!BCrypt.Net.BCrypt.Verify(userlogin.PasswordHash, user.PasswordHash))
                {
                throw new UnauthorizedAccessException("Invalid credentials.");
                 }
                var userModel = _mapper.Map<Usermodel>(user);
                string token = CreateToken(user);
                return new loginresponse
                {
                    Token = token,
                    User = userModel
                };
            
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Email,user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
         new Claim(ClaimTypes.Role, user.Userroles)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secretkey").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                 issuer: _configuration["JWT:Issuer"],
        audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

            public async Task<Usermodel> RegisterAsync(Usermodel user)
        {

            {

                if (user == null)
                {
                    throw new ArgumentException("User data is null");
                }
                if (string.IsNullOrEmpty(user.Userroles))
                {

                    throw new ArgumentException("Role is required (either JobSeeker or Employer)");
                }
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                var users = _mapper.Map<User>(user);
                users.PasswordHash = passwordHash;
                await _userRepository.AddAsync(users);


                await _userRepository.SaveChangesAsync();
                user.UserId = users.UserId;
                return (user);


            }
        }
        public ulong GetTokenValue(ClaimsIdentity identity)
        {

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claim = identity.Claims;
            var usernameClaim = claim
                .Where(x => x.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault();

            return Convert.ToUInt64(usernameClaim.Value);
        }
        public async Task<Usermodel> UpdateUserAsync(Usermodel user,int id)
        {
                var user1 = await _userRepository.GetByIdAsync(id);
                if (user1 == null)
                {

                throw new KeyNotFoundException("User not found.");
                }
                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                }
                _mapper.Map(user, user1);
                user1.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user1);
                await _userRepository.SaveChangesAsync();
            return user;
            
        }
    }
}

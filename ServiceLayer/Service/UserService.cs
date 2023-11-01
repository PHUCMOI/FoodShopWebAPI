using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.AspNetCore.Http;
using Models_Layer.ModelRequest;
using Services_Layer.ServiceInterfaces;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelRequest;
using Newtonsoft.Json.Linq;

namespace Services_Layer.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserDAO userDAO;
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenService tokenService;
        private string generatedToken = null;
        public UserService(IUserDAO userDAO, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ITokenService tokenService) 
        {
            this.userDAO = userDAO;
            this.passwordHasher = passwordHasher;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration; 
            this.tokenService = tokenService;
        }

        public async Task<bool> Create(UserRequest userRequest)
        {
            int userID = 1; // Temporary variable
            try
            {
                string password = passwordHasher.HashPassword(userRequest.Password);
                User user = new User()
                {
                    UserId = userRequest.UserId,
                    UserName = userRequest.UserName,
                    Password = password,
                    PhoneNumber = userRequest.PhoneNumber,
                    Role = userRequest.Role,
                    Status = userRequest.Status,
                    CreateBy = userID,
                    CreateDate = DateTime.Now,
                    UpdateBy = userID,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false
                };

                var result = await userDAO.Create(user);
                if (result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CreateToken(LoginRequest login)
        {
            var user = await userDAO.GetUserByUserName(login.UserName);
            var claims = new[] {
                     new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim("username", user.UserName),
                     new Claim("userid", user.UserId.ToString()),
                     new Claim("role", user.Role),
                     new Claim("phonenumber", user.PhoneNumber)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signIn);


            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }

        public Task<bool> Delete(int id)
        {
            try
            {
                bool flag = userDAO.DeleteAsync(id);
                if (flag)
                {
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRequest> GetUserByID(int id)
        {
            var user = await userDAO.GetUser(id);
            if (user == null)
            {
                throw new Exception("Null");
            }
            else { return user; }
        }

        public async Task<UserRequest> GetUserByUserName(string username)
        {
            var user = await userDAO.GetUserByUserName(username);
            if (user == null)
            {
                throw new Exception("Null");
            }
            else { return user; }
        }

        public async Task<List<UserRequest>> GetUserList()
        {
            var userList = await userDAO.GetListUser();
            return userList;
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            try
            {
                var user = GetUserByUserName(loginRequest.UserName);
                if (user != null)
                {
                    if (passwordHasher.VerifyPassword(loginRequest.Password, user.Result.Password))
                    {
                        
                        
                        generatedToken = tokenService.BuildToken(configuration["Jwt:Key"].ToString(), configuration["Jwt:Issuer"].ToString(), await user);                        
                        if (generatedToken != null)
                        {
                            var session = httpContextAccessor.HttpContext.Session;
                            var userDataJson = JsonConvert.SerializeObject(user.Result);
                            session.Set("UserData", Encoding.UTF8.GetBytes(userDataJson));
                            user.Result.AccessToken = generatedToken;
                            session.SetString("Token", generatedToken);
                            return generatedToken;
                        }
                    }
                    return "Login Failed";
                }
                return "Need Infomation";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            int userID = 1; // Temporary variable
            try
            {
                string password = passwordHasher.HashPassword(registerRequest.Password);
                User user = new User()
                {
                    UserName = registerRequest.UserName,
                    Password = password,
                    PhoneNumber = registerRequest.PhoneNumber,
                    Role = "Customer",
                    Status = "Active",
                    CreateBy = userID,
                    CreateDate = DateTime.Now,
                    UpdateBy = userID,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false
                };

                await userDAO.Create(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // login 1. check username 1.1 get in4 2. check pass
        public async Task<bool> UpdateAsync(UpdateUser updateUser)
        {
            if (updateUser != null)
            {
                var tempUser = new User()
                {
                    UserId = updateUser.userId,
                    UserName = updateUser.UserName,
                    PhoneNumber = updateUser.PhoneNumber,
                    Role = updateUser.Role,
                    Status = updateUser.Status
                };
                var res = await userDAO.UpdateAsync(tempUser, 1);                   
                
                if (res)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new Exception();
            }
        }


    }
}

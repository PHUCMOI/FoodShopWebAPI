using Fooding_Shop.Models;
using ModelLayer.ModelRequest;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.ServiceInterfaces
{
    public interface IUserService
    {
        Task<List<UserRequest>> GetUserList();
        Task<UserRequest> GetUserByID(int id);
        Task<bool> UpdateAsync(User user);
        Task<bool> Delete(int id);
        Task<bool> Create(UserRequest user);
        Task<UserRequest> GetUserByUserName(string username);
        Task<string> Login(LoginRequest loginRequest); 
        Task Register(RegisterRequest registerRequest);
        Task<string> CreateToken(LoginRequest login);
    }
}

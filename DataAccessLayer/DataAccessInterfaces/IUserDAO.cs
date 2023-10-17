using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IUserDAO
    {
        Task<List<UserRequest>> GetListUser();
        Task<UserRequest> GetUser(int id);
        Task<UserRequest> GetUserByUserName(string username);
        Task<bool> UpdateAsync(User user, int userID);
        Task<bool> Create(User user);
        bool DeleteAsync(int id);
    }
}

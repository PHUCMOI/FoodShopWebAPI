using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface ICategoryDAO
    {
        Task<List<CategoryRequest>> GetListCategory();
        Task<Category> GetCategory(int id);
        Task<bool> UpdateAsync(Category category ,int userID);
        Task<bool> Create(Category category);
        bool DeleteAsync(int id);
    }
}

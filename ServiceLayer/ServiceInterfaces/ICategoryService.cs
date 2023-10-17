using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.ServiceInterfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryRequest>> GetCategoryList();
        Task<Category> GetCategoryByID(int id);
        Task<bool> UpdateAsync(Category category);
        Task<bool> Delete(int id);
        Task<bool> Create(CategoryRequest category);
    }
}

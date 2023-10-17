using DataAccessLayer.DataAccess;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using Services_Layer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services_Layer.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDAO categoryDAO;
        public CategoryService(ICategoryDAO categoryDAO)
        {
            this.categoryDAO = categoryDAO;
        }

        public async Task<bool> Create(CategoryRequest categoryRequest)
        {
            int userID = 1; // Temporary variable
            try
            {
                Category category = new Category()
                {
                    CategoryName = categoryRequest.CategoryName,
                    CreateBy = userID,
                    CreateDate = DateTime.Now,
                    UpdateBy = userID,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false
                };

                var result = await categoryDAO.Create(category);
                if (result != null)
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

        public Task<bool> Delete(int id)
        {
            try
            {
                bool flag = categoryDAO.DeleteAsync(id);
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

        public Task<List<CategoryRequest>> GetCategoryList()
        {
            var categoryList = categoryDAO.GetListCategory();
            return categoryList;
        }

        public Task<Category> GetCategoryByID(int id)
        {
            var category = categoryDAO.GetCategory(id);
            return category;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            int userID = 1; // Temporary parameter
            if (category != null)
            {
                var result = await categoryDAO.UpdateAsync(category, userID);
                if (result != null)
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

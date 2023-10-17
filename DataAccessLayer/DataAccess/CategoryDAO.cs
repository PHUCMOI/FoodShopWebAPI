using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models_Layer.ModelRequest;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DataAccess
{
    public class CategoryDAO : ICategoryDAO
    {
        private IConfiguration configuration;
        private readonly FoodingShopContext _foodContext;
        public CategoryDAO(IConfiguration configuration, FoodingShopContext foodContext)
        {
            this.configuration = configuration;
            _foodContext = foodContext;
        }

        public async Task<bool> Create(Category category)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"INSERT INTO [dbo].[Category]
                                           (CategoryName
                                           ,CreateBy
                                           ,CreateDate
                                           ,IsDeleted
                                           ,UpdateBy
                                           ,UpdateDate)
                                     VALUES
                                           (@CategoryName
                                           ,@CreateBy
                                           ,@CreateDate
                                           ,@IsDeleted
                                           ,@UpdateBy
                                           ,@UpdateDate)";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@CategoryName", category.CategoryName));
                        command.Parameters.Add(new SqlParameter("@CreateBy", category.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", category.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", category.IsDeleted));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", category.UpdateBy));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", category.UpdateDate));

                        var result = command.ExecuteNonQuery();
                        con.Close();
                        if (result != null)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool DeleteAsync(int id)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        con.Open();
                        var query = @"UPDATE [dbo].[Category]
                                  SET IsDeleted = 1
                                  WHERE CategoryID = @CategoryID";

                        command.Parameters.Add(new SqlParameter("@CategoryID", id));
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        con.Close();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };
        }

        public async Task<Category> GetCategory(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT CategoryID
                                          ,CategoryName
                                          ,CreateBy
                                          ,CreateDate
                                          ,UpdateBy
                                          ,UpdateDate
                                          ,IsDeleted
                                      FROM [dbo].[Category]
                                      WHERE CategoryID = {id}";
                    var user = await con.QuerySingleOrDefaultAsync<Category>(query, new { Id = id });

                    return user;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CategoryRequest>> GetListCategory()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT CategoryID
                                          ,CategoryName                                          
                                      FROM [dbo].[Category]
                                      WHERE IsDeleted = 0";
                    var categoryList = await con.QueryAsync<CategoryRequest>(query);
                    return categoryList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(Category category, int userID)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = _foodContext.Database.GetDbConnection().CreateCommand())
                    {
                        var query = @"UPDATE [dbo].[Category]
                          SET CategoryName = @CategoryName,
                              CreateBy = @CreateBy,
                              CreateDate = @CreateDate,
                              IsDeleted = @IsDeleted,
                              UpdateBy = @UpdateBy,
                              UpdateDate = @UpdateDate 
                          WHERE CategoryID = @CategoryID";

                        command.CommandText = query;
                        // Add parameters
                        command.Parameters.Add(new SqlParameter("@CategoryName", category.CategoryName));
                        command.Parameters.Add(new SqlParameter("@CreateBy", category.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", category.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", false));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", userID));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@CategoryID", category.CategoryId));

                        await _foodContext.Database.OpenConnectionAsync();
                        var result = command.ExecuteNonQuery();
                        con.Close();
                        if (result != null)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Sql Query Error\n" + ex.Message);
                }
            }
        }
    }
}

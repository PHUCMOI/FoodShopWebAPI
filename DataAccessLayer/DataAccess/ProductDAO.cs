using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models_Layer.ModelRequest;
using Dapper;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DataAccessLayer.DataAccess
{
    public class ProductDAO : IProductDAO
    {
        private readonly IConfiguration _configuration;
        private readonly FoodingShopContext _foodContext;
        public ProductDAO(IConfiguration configuration, FoodingShopContext foodingShopContext)
        {
            _configuration = configuration;
            _foodContext = foodingShopContext;
        }
        public async Task<bool> Create(Product product)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"INSERT INTO Products
                                           (ProductName
                                           ,ProductCategory
                                           ,Description
                                           ,Price
                                           ,ImgURL
                                           ,CreateBy
                                           ,CreateDate
                                           ,IsDeleted
                                           ,UpdateBy
                                           ,UpdateDate)
                                     VALUES
                                           (@ProductName
                                           ,@ProductCategory
                                           ,@Description
                                           ,@Price
                                           ,@ImgURL
                                           ,@CreateBy
                                           ,@CreateDate
                                           ,@IsDeleted
                                           ,@UpdateBy
                                           ,@UpdateDate)";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
                        command.Parameters.Add(new SqlParameter("@ProductCategory", product.ProductCategory));
                        command.Parameters.Add(new SqlParameter("@Description", product.Description));
                        command.Parameters.Add(new SqlParameter("@Price", product.Price));
                        command.Parameters.Add(new SqlParameter("@ImgUrl", product.ImgUrl));
                        command.Parameters.Add(new SqlParameter("@CreateBy", product.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", product.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", product.IsDeleted));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", product.UpdateBy));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", product.UpdateDate));

                        var result = command.ExecuteNonQuery();
                        con.Close();
                        if(result != null)
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
            using (var con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        con.Open();
                        var query = @"UPDATE Products
                                  SET IsDeleted = 1
                                  WHERE ProductID = @ProductID";

                        command.Parameters.Add(new SqlParameter("@ProductID", id));
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        con.Close();  
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }                
            };
        }

        public async Task<List<ProductRequest>> GetListProduct()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT ProductID
                                          ,ProductName
                                          ,ProductCategory
                                          ,Description
                                          ,Price
                                          ,ImgURL
                                      FROM Products
                                      WHERE IsDeleted = 0";
                    var productList = await con.QueryAsync<ProductRequest>(query);
                    return productList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT ProductID
                                          ,ProductName
                                          ,ProductCategory
                                          ,Description
                                          ,Price
                                          ,ImgURL
                                          ,CreateBy
                                          ,CreateDate
                                          ,UpdateBy
                                          ,UpdateDate
                                          ,IsDeleted
                                      FROM Products
                                      WHERE ProductID = {id}";
                    var product = await con.QuerySingleOrDefaultAsync<Product>(query, new { Id = id });

                    return product;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string SearchQueryString(string value, decimal? minPrice, decimal? maxPrice)
        {
            string query = string.Empty;
            if(value != null)
            {
                query += @"AND ProductName LIKE '%' + @ProductName + '%'  OR ProductCategory LIKE '%' + @ProductCategory + '%' OR Description LIKE '%' + @Description + '%'";
            }
            else if (minPrice != null && maxPrice != null)
            {
                query += @"AND Price >= @MinPrice AND Price <= @MaxPrice";
            }
            return query;
        }

        public async Task<List<ProductRequest>> SearchProducts(string value, decimal? minPrice, decimal? maxPrice)
        {
            string searchQuery = string.Empty;
            try
            {               
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
                {
                    if (value != null || (minPrice != null && maxPrice != null))
                    {
                        searchQuery = SearchQueryString(value, minPrice, maxPrice);
                    }
                    var query = $@"SELECT ProductID
                                          ,ProductName
                                          ,ProductCategory
                                          ,Description
                                          ,Price
                                          ,ImgURL
                                      FROM Products
                                      WHERE IsDeleted = 0 {searchQuery}";

                    var parameters = new
                    {
                        ProductName = value,
                        ProductCategory = value,
                        Description = value,
                        MinPrice = minPrice, 
                        MaxPrice = maxPrice,
                    };
                    var productList = await con.QueryAsync<ProductRequest>(query, parameters);
                    return productList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(Product product, int userID)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = _foodContext.Database.GetDbConnection().CreateCommand())
                    {
                        var query = @"UPDATE Products
                          SET ProductName = @ProductName,
                              ProductCategory = @ProductCategory,
                              Description = @Description,
                              Price = @Price,
                              ImgURL = @ImgUrl,
                              CreateBy = @CreateBy,
                              CreateDate = @CreateDate,
                              IsDeleted = @IsDeleted,
                              UpdateBy = @UpdateBy,
                              UpdateDate = @UpdateDate 
                          WHERE ProductID = @ProductId";

                        command.CommandText = query;
                        // Add parameters
                        command.Parameters.Add(new SqlParameter("@ProductName", product.ProductName));
                        command.Parameters.Add(new SqlParameter("@ProductCategory", product.ProductCategory));
                        command.Parameters.Add(new SqlParameter("@Description", product.Description));
                        command.Parameters.Add(new SqlParameter("@Price", product.Price));
                        command.Parameters.Add(new SqlParameter("@ImgUrl", product.ImgUrl));
                        command.Parameters.Add(new SqlParameter("@CreateBy", product.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", product.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", false)); 
                        command.Parameters.Add(new SqlParameter("@UpdateBy", userID));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@ProductId", product.ProductId));

                        await _foodContext.Database.OpenConnectionAsync();
                        var result = command.ExecuteNonQuery();
                        con.Close();
                        if(result != null)
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

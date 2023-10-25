using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class CartDAO : ICartDAO
    {
        private readonly IConfiguration configuration;
        private readonly FoodingShopContext foodContext;
        public CartDAO(IConfiguration configuration, FoodingShopContext foodContext)
        {
            this.configuration = configuration;
            this.foodContext = foodContext;
        }

        public async Task<bool> ClearCart(int userId)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"DELETE FROM [dbo].[Cart]
                                      WHERE UserID = @UserID";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@UserID", userId));

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

        public async Task<bool> CreateProductCart(Cart cart)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"INSERT INTO [dbo].[Cart]
                                           (UserID
                                           ,UserName
                                           ,ProductID
                                           ,Quantity
                                           ,CreateBy
                                           ,CreateDate
                                           ,UpdateBy
                                           ,UpdateDate
                                           ,IsDeleted)
                                     VALUES
                                           (@UserID
                                           ,@UserName
                                           ,@ProductID
                                           ,@Quantity
                                           ,@CreateBy
                                           ,@Createdate
                                           ,@UpdateBy
                                           ,@UpdateDate
                                           ,@IsDeleted);
                                      SELECT CONVERT(int, SCOPE_IDENTITY()) as OrderID";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@UserID", cart.UserId));
                        command.Parameters.Add(new SqlParameter("@UserName", cart.UserName));
                        command.Parameters.Add(new SqlParameter("@ProductID", cart.ProductID));
                        command.Parameters.Add(new SqlParameter("@Quantity", cart.Quantity));
                        command.Parameters.Add(new SqlParameter("@CreateBy", cart.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", cart.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", cart.IsDeleted));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", cart.UpdateBy));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", cart.UpdateDate));

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

        public async Task<bool> DeleteItem(Cart cart)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"DELETE FROM [dbo].[Cart]
                                      WHERE UserID = @UserID AND ProductID = @ProductID AND UserName = @UserName";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@UserID", cart.UserId));
                        command.Parameters.Add(new SqlParameter("@UserName", cart.UserName));
                        command.Parameters.Add(new SqlParameter("@ProductID", cart.ProductID));

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

        public async Task<List<CartRequest>> GetCartList(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT UserID
                                          ,UserName
                                            ,ProductID
                                            ,Quantity
                                      FROM [dbo].[Cart]
                                      WHERE IsDeleted = 0 AND UserID = @UserID";
                    var parameters = new
                    {
                        UserId = userId,
                    };
                    var cartList = await con.QueryAsync<CartRequest>(query, parameters);
                    return cartList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateProductCart(Cart cart)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {

                    using (var command = foodContext.Database.GetDbConnection().CreateCommand())
                    {
                        await foodContext.Database.OpenConnectionAsync();

                        var query = $@"UPDATE [dbo].[Cart]
                           SET UserName = @UserName
                              ,UserID = @UserID
                              ,ProductID = @ProductID
                              ,Quantity = @Quanitty
                              ,UpdateBy = @UpdateBy
                              ,UpdateDate = @UpdateDate
                              ,IsDeleted = @IsDeleted
                            WHERE UserID = @UserID AND ProductID = @ProductID";
                        command.CommandText = query;

                        command.Parameters.Add(new SqlParameter("@UserName", cart.UserName));
                        command.Parameters.Add(new SqlParameter("@UserID", cart.UserId));
                        command.Parameters.Add(new SqlParameter("@Quanitty", cart.Quantity));
                        command.Parameters.Add(new SqlParameter("@ProductID", cart.ProductID));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", false));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", cart.UpdateBy));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now));

                        var result = await command.ExecuteNonQueryAsync();
                        if (result != null)
                            return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}

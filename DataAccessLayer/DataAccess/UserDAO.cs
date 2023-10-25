using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class UserDAO : IUserDAO
    {
        private readonly IConfiguration _configuration;
        private readonly FoodingShopContext _foodContext;

        public UserDAO(IConfiguration configuration, FoodingShopContext foodContext)
        {
            _configuration = configuration;
            _foodContext = foodContext;
        }

        public async Task<bool> Create(User user)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"INSERT INTO [dbo].[User]
                                           (UserName
                                          ,Password
                                          ,Role
                                          ,PhoneNumber
                                          ,Status
                                           ,CreateBy
                                           ,CreateDate
                                           ,IsDeleted
                                           ,UpdateBy
                                           ,UpdateDate)
                                     VALUES
                                           (@UserName
                                           ,@Password
                                           ,@Role
                                           ,@PhoneNumber
                                           ,@Status
                                           ,@CreateBy
                                           ,@CreateDate
                                           ,@IsDeleted
                                           ,@UpdateBy
                                           ,@UpdateDate)";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                        command.Parameters.Add(new SqlParameter("@Password", user.Password));
                        command.Parameters.Add(new SqlParameter("@Role", user.Role));
                        command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@Status", user.Status));
                        command.Parameters.Add(new SqlParameter("@CreateBy", user.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", user.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", user.IsDeleted));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", user.UpdateBy));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", user.UpdateDate));

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
            using (var con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        con.Open();
                        var query = @"UPDATE [dbo].[User]
                                  SET IsDeleted = 1
                                  WHERE UserID = @UserID";

                        command.Parameters.Add(new SqlParameter("@UserID", id));
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

        public async Task<List<UserRequest>> GetListUser()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT UserID
                                          ,UserName
                                          ,Password
                                          ,Role
                                          ,PhoneNumber
                                          ,Status
                                      FROM [dbo].[User]
                                      WHERE IsDeleted = 0";
                    var userList = await con.QueryAsync<UserRequest>(query);
                    return userList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRequest> GetUser(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT UserID
                                          ,UserName
                                          ,Password
                                          ,Role
                                          ,PhoneNumber
                                          ,Status
                                      FROM [dbo].[User]
                                      WHERE UserID = {id} AND IsDeleted = 0";
                    var user = await con.QuerySingleOrDefaultAsync<UserRequest>(query, new { Id = id });

                    return user;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserRequest> GetUserByUserName(string username)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT UserID
                                          ,UserName
                                          ,Password
                                          ,Role
                                          ,PhoneNumber
                                          ,Status
                                          ,IsDeleted
                                      FROM [dbo].[User]
                                      WHERE UserName = '{username}'";
                    var user = await con.QuerySingleOrDefaultAsync<UserRequest>(query, new { Username = username });

                    return user;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(User user, int userID)
        {
            using (var con = new SqlConnection(_configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = _foodContext.Database.GetDbConnection().CreateCommand())
                    {
                        var query = @"UPDATE [dbo].[User]
                          SET UserName = @UserName,
                              Password = @Password,
                              Role = @Role,
                              PhoneNumber = @PhoneNumber,
                              Status = @Status,
                              IsDeleted = @IsDeleted,
                              UpdateBy = @UpdateBy,
                              UpdateDate = @UpdateDate 
                          WHERE UserID = @UserID";

                        command.CommandText = query;
                        // Add parameters
                        command.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                        command.Parameters.Add(new SqlParameter("@Password", user.Password));
                        command.Parameters.Add(new SqlParameter("@Role", user.Role));
                        command.Parameters.Add(new SqlParameter("@PhoneNumber", user.PhoneNumber));
                        command.Parameters.Add(new SqlParameter("@Status", user.Status));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", false));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", userID));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@UserID", user.UserId));

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

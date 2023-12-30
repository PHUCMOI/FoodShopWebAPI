using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using ModelLayer.PayPalModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class RestaurantMapDAO : IRestaurantMapDAO
    {
        private readonly IConfiguration configuration;
		private readonly FoodingShopContext foodContext;

		public RestaurantMapDAO(IConfiguration configuration, FoodingShopContext foodContext)
		{
			this.configuration = configuration;
			this.foodContext = foodContext;
		}

		public async Task<bool> AddNewTable(RestaurantMap map)
		{
			using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
			{
				try
				{
					using (var command = con.CreateCommand())
					{
						await con.OpenAsync();
						var query = @"INSERT INTO [dbo].[RestaurantMap]
                                               (RestaurantID
                                               ,PositionX
                                               ,PositionY
                                               ,Cols
                                               ,Rows
                                               ,CreateBy
                                               ,CreateAt
                                               ,UpdateBy
                                               ,UpdateAt
                                               ,IsDeleted
                                               ,TableId)
                                         VALUES
                                               (@RestaurantID
                                               ,@PositionX
                                               ,@PositionY
                                               ,@Cols
                                               ,@Rows
                                               ,@CreateBy
                                               ,@Createdate
                                               ,@UpdateBy
                                               ,@UpdateDate
                                               ,@IsDeleted
                                               ,@TableId)";

						command.CommandText = query;
						command.Parameters.Add(new SqlParameter("@RestaurantID", map.RestaurantId));
						command.Parameters.Add(new SqlParameter("@PositionX", map.PositionX));
						command.Parameters.Add(new SqlParameter("@PositionY", map.PositionY));
						command.Parameters.Add(new SqlParameter("@Cols", map.Cols));
						command.Parameters.Add(new SqlParameter("@Rows", map.Rows));
						command.Parameters.Add(new SqlParameter("@CreateBy", map.CreateBy));
						command.Parameters.Add(new SqlParameter("@CreateDate", map.CreateAt));
						command.Parameters.Add(new SqlParameter("@IsDeleted", map.IsDeleted));
						command.Parameters.Add(new SqlParameter("@UpdateBy", map.UpdateBy));
						command.Parameters.Add(new SqlParameter("@UpdateDate", map.UpdateAt));
						command.Parameters.Add(new SqlParameter("@TableId", map.TableId));

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

		public async Task<bool> CreateMaps(List<RestaurantMap> restaurantMaps)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    int count = 0;
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        foreach (var map in restaurantMaps)
                        {
                            var query = @"INSERT INTO [dbo].[RestaurantMap]
                                               (RestaurantID
                                               ,PositionX
                                               ,PositionY
                                               ,Cols
                                               ,Rows
                                               ,CreateBy
                                               ,CreateAt
                                               ,UpdateBy
                                               ,UpdateAt
                                               ,IsDeleted)
                                         VALUES
                                               (@RestaurantID
                                               ,@PositionX
                                               ,@PositionY
                                               ,@Cols
                                               ,@Rows
                                               ,@CreateBy
                                               ,@Createdate
                                               ,@UpdateBy
                                               ,@UpdateDate
                                               ,@IsDeleted)";

                            command.Parameters.Clear();
                            command.CommandText = query;
                            command.Parameters.Clear();
                            command.Parameters.Add(new SqlParameter("@RestaurantID", map.RestaurantId));
                            command.Parameters.Add(new SqlParameter("@PositionX", map.PositionX));
                            command.Parameters.Add(new SqlParameter("@PositionY", map.PositionY));
                            command.Parameters.Add(new SqlParameter("@Cols", map.Cols));
                            command.Parameters.Add(new SqlParameter("@Rows", map.Rows));
                            command.Parameters.Add(new SqlParameter("@CreateBy", map.CreateBy));
                            command.Parameters.Add(new SqlParameter("@CreateDate", map.CreateAt));
                            command.Parameters.Add(new SqlParameter("@IsDeleted", map.IsDeleted));
                            command.Parameters.Add(new SqlParameter("@UpdateBy", map.UpdateBy));
                            command.Parameters.Add(new SqlParameter("@UpdateDate", map.UpdateAt));

                            var result = command.ExecuteNonQuery();
                            if (result != null)
                            {
                                count++;
                            }
                        }
                        if (count == restaurantMaps.Count)
                        {
                            con.Close();
                            return true;
                        }
                        con.Close();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> DeleteMaps(DeleteTableRequest deleteTableRequest)
        {
			using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
			{
				try
				{
					using (var command = con.CreateCommand())
					{
						await con.OpenAsync();
						var query = @"DELETE FROM [dbo].[RestaurantMap]
                                      WHERE TableId = @TableId AND RestaurantId = @RestaurantId";

						command.CommandText = query;
						command.Parameters.Add(new SqlParameter("@TableId", deleteTableRequest.TableId));
						command.Parameters.Add(new SqlParameter("@RestaurantId", deleteTableRequest.RestaurantId));

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

        public async Task<List<RestaurantMapResponse>> RestaurantMaps(int restaurantId)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT RestaurantID, PositionX, PositionY, Cols, Rows, TableId
                                            FROM RestaurantMap
                                            WHERE RestaurantID = @Id AND IsDeleted = 0";

                    var restaurantMaps = await con.QueryAsync<RestaurantMapResponse>(query, new { Id = restaurantId });

                    return restaurantMaps.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateMaps(List<RestaurantMapRequest> restaurantMap)
        {
			using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
			{
				try
				{
					int count = 0;
					foreach (var item in restaurantMap)
                    {
						using (var command = foodContext.Database.GetDbConnection().CreateCommand())
						{
							await foodContext.Database.OpenConnectionAsync();

							var query = $@"UPDATE [dbo].[RestaurantMap]
                                       SET [PositionX] = @PositionX
                                          ,[PositionY] = @PositionY
                                          ,[Cols] = @Cols
                                          ,[Rows] = @Rows
                                          ,[CreateAt] = @CreateAt
                                          ,[UpdateAt] = @UpdateAt
                                          ,[UpdateBy] = @UpdateBy
                                          ,[CreateBy] = @CreateBy
                                     WHERE [TableId] = @TableId AND RestaurantId = @RestaurantID";
							command.CommandText = query;

                            command.Parameters.Clear();
							command.Parameters.Add(new SqlParameter("@RestaurantID", item.RestaurantId));
							command.Parameters.Add(new SqlParameter("@PositionX", item.PositionX));
							command.Parameters.Add(new SqlParameter("@PositionY", item.PositionY));
							command.Parameters.Add(new SqlParameter("@Cols", item.Cols));
							command.Parameters.Add(new SqlParameter("@Rows", item.Rows));
							command.Parameters.Add(new SqlParameter("@CreateAt", DateTime.Now));
							command.Parameters.Add(new SqlParameter("@CreateBy", item.UserId));
							command.Parameters.Add(new SqlParameter("@UpdateBy", item.UserId));
							command.Parameters.Add(new SqlParameter("@UpdateAt", DateTime.Now));
							command.Parameters.Add(new SqlParameter("@TableId", item.TableId));

							var result = command.ExecuteNonQuery();
							if (result != null)
							{
								count++;
							}
						}
						if (count == restaurantMap.Count)
						{
							return true;
						}
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

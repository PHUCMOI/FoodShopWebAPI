using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class RestaurantDAO : IRestaurantDAO
    {
        private readonly IConfiguration configuration;
        public RestaurantDAO(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }
        public Task<bool> Create(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Restaurant>> GetListRestaurant()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT RestaurantId, RestaurantName
                                  FROM [dbo].[Restaurant]
                                  WHERE IsDeleted = 0";
                    var restaurantList = await con.QueryAsync<Restaurant>(query);
                    return restaurantList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Restaurant> GetRestaurantById(int restaurantId)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT RestaurantID
                                          ,RestaurantName
                                          ,CreateBy
                                          ,CreateDate
                                          ,UpdateBy
                                          ,UpdateDate
                                          ,IsDeleted
                                      FROM [dbo].[Restaurant]
                                      WHERE CategoryID = {restaurantId}";
                    var restaurant = await con.QuerySingleOrDefaultAsync<Restaurant>(query, new { Id = restaurantId });

                    return restaurant;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<bool> Update(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}

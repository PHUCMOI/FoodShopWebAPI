using DataAccessLayer.DataAccessInterfaces;
using ModelLayer.Models;
using ServiceLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantDAO restaurantDAO;
        public RestaurantService(IRestaurantDAO restaurantDAO)
        {
            this.restaurantDAO = restaurantDAO;
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
            var restaurantList = await restaurantDAO.GetListRestaurant();
            return restaurantList;
        }

        public async Task<Restaurant> GetRestaurantById(int restaurantId)
        {
            var restaurant = await restaurantDAO.GetRestaurantById(restaurantId);
            return restaurant;
        }

        public Task<bool> Update(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}

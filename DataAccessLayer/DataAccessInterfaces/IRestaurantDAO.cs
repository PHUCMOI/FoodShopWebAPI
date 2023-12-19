using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IRestaurantDAO
    {
        Task<List<Restaurant>> GetListRestaurant();
        Task<Restaurant> GetRestaurantById(int restaurantId);
        Task<bool> Create(Restaurant restaurant);
        Task<bool> Update(Restaurant restaurant);
        Task<bool> Delete(int restaurantId);
    }
}

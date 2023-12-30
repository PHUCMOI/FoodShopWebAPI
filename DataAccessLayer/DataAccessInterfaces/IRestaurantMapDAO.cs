using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IRestaurantMapDAO 
    {
        public Task<List<RestaurantMapResponse>> RestaurantMaps(int restaurantId);
        public Task<bool> CreateMaps(List<RestaurantMap> restaurantMaps);
        public Task<bool> DeleteMaps(DeleteTableRequest deleteTableRequest);
        public Task<bool> UpdateMaps(List<RestaurantMapRequest> restaurantMap);
        public Task<bool> AddNewTable(RestaurantMap map);
    }
}

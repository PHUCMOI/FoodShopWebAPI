using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IRestaurantMapService
    {   
        Task<bool> CreateMaps(List<RestaurantMapRequest> map);
        Task<List<RestaurantMapResponse>> GetRestaurantMaps(GetRestaurantMap getRestaurantMap);
        Task<bool> UpdateMaps(List<RestaurantMapRequest> map);
        Task<bool> AddNewTable(RestaurantMapRequest restaurantMapRequest);
        Task<bool> DeleteTable(DeleteTableRequest deleteTableRequest);
        
    }
}

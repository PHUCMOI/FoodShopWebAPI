using DataAccessLayer.DataAccessInterfaces;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using PayPal.v1.Orders;
using ServiceLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models_Layer.Enum.EnumModel;

namespace ServiceLayer.Service
{
    public class RestaurantMapService : IRestaurantMapService
    {
        private readonly IRestaurantMapDAO restaurantMapDAO;
        private readonly IReservationDAO reservationDAO;
        private readonly IReservationService reservationService;
        public RestaurantMapService(IRestaurantMapDAO restaurantMapDAO, IReservationDAO reservationDAO, IReservationService reservationService) 
        { 
            this.restaurantMapDAO = restaurantMapDAO;
            this.reservationDAO = reservationDAO;
            this.reservationService = reservationService;
        }

		public Task<bool> AddNewTable(RestaurantMapRequest restaurantMapRequest)
		{
			var map = new RestaurantMap()
			{
				TableId = restaurantMapRequest.TableId,
				RestaurantId = restaurantMapRequest.RestaurantId,
				PositionX = restaurantMapRequest.PositionX,
				PositionY = restaurantMapRequest.PositionY,
				Cols = restaurantMapRequest.Cols,
				Rows = restaurantMapRequest.Rows,
				CreateBy = restaurantMapRequest.UserId,
				CreateAt = DateTime.Now,
				UpdateAt = DateTime.Now,
				UpdateBy = restaurantMapRequest.UserId,
				IsDeleted = false
			};

			var res = restaurantMapDAO.AddNewTable(map);
			return res;
		}

		public async Task<bool> CreateMaps(List<RestaurantMapRequest> maps)
        {
            try
            {
                if(maps != null)
                {
                    var restaurantMap = new List<RestaurantMap>();
                    foreach(var mapItem in maps)
                    {
                        var map = new RestaurantMap() {
                            RestaurantId = mapItem.RestaurantId,
                            PositionX = mapItem.PositionX,
                            PositionY = mapItem.PositionY,
                            Cols = mapItem.Cols,
                            Rows = mapItem.Rows,
                            CreateAt = DateTime.Now,
                            CreateBy = 1,
                            UpdateAt = DateTime.Now,
                            UpdateBy = 1,
                            IsDeleted = false
                        };
                        restaurantMap.Add(map);
                    }

                    var res = await restaurantMapDAO.CreateMaps(restaurantMap);
                    return res;
                }
                return false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

		public async Task<bool> DeleteTable(DeleteTableRequest deleteTableRequest)
		{
			return await restaurantMapDAO.DeleteMaps(deleteTableRequest);
		}

		public async Task<List<RestaurantMapResponse>> GetRestaurantMaps(GetRestaurantMap getRestaurantMap)
        {
            var restaurantMap = await restaurantMapDAO.RestaurantMaps(getRestaurantMap.RestaurantId);
            var reservationListByDate = await reservationDAO.GetListReservationByDate(getRestaurantMap.ReservationDate, getRestaurantMap.RestaurantId);

			foreach (var item in restaurantMap)
			{
				item.isAvailable = true;

				if (getRestaurantMap.NumberPeople == (int)NumberOfPeople.LessThan2 && !(item.Cols == 1 && item.Rows == 1))
				{
					item.isAvailable = false;
				}
				else if (getRestaurantMap.NumberPeople == (int)NumberOfPeople.ThreeToFour &&
						!((item.Cols == 2 && item.Rows == 1) || (item.Cols == 1 && item.Rows == 2)))
				{
					item.isAvailable = false;
				}
				else if (getRestaurantMap.NumberPeople == (int)NumberOfPeople.MoreThan4 && !(item.Cols == 2 && item.Rows == 2))
				{
					item.isAvailable = false;
				}

				if (item.isAvailable &&
					reservationListByDate.Any(reservation => reservation.PositionX == item.PositionX &&
											reservation.PositionY == item.PositionY &&
											reservation.TimeReservation.ToString() == getRestaurantMap.ReservationTime + ":00"))
				{
					item.isAvailable = false;
				}
			}

			return restaurantMap;
		}

		public async Task<bool> UpdateMaps(List<RestaurantMapRequest> map)
		{
			try
			{
				if (map != null)
				{				
					var res = await restaurantMapDAO.UpdateMaps(map);
					return res;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}

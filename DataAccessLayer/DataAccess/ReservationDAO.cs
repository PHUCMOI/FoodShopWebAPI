using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class ReservationDAO : IReservationDAO
    {
        private readonly IConfiguration configuration;
        public ReservationDAO(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<bool> Create(Reservation resservation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int reservationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reservation>> GetListReservation()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reservation>> GetListReservationByDate(string reservationDate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT [ReservationID]
                                      ,[UserID]
                                      ,[RestaurantID]
                                      ,[NumberOfCustomer]
                                      ,[PositionX]
                                      ,[PositionY]
                                      ,[DateReservation]
                                      ,[TimeReservation]
                                  FROM [FoodingShop].[dbo].[Reservation]
                                  WHERE [RestaurantID] = @RestaurantID AND [DateReservation] = @DateReservation";
                    var parameters = new
                    {
                        RestaurantID = 1,
                        DateReservation = reservationDate
                    };
                    var cartList = await con.QueryAsync<Reservation>(query, parameters);
                    return cartList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		public async Task<bool> MakeReservation(Reservation reservation)
		{
			using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
			{
				try
				{
					using (var command = con.CreateCommand())
					{
						await con.OpenAsync();
						var query = @"INSERT INTO [dbo].[Reservation]
                                           (UserId
                                           ,RestaurantId
                                           ,NumberOfCustomer
                                           ,PositionX
                                           ,PositionY
                                           ,DateReservation
                                           ,TimeReservation
                                           ,CreateBy
                                           ,CreatedAt
                                           ,UpdateBy
                                           ,UpdateAt
                                           ,IsDeleted)
                                     VALUES
                                           (@UserId
                                           ,@RestaurantId
                                           ,@NumberOfCustomer
                                           ,@PositionX
                                           ,@PositionY
                                           ,@DateReservation
                                           ,@TimeReservation
                                           ,@CreateBy
                                           ,@CreateAt
                                           ,@UpdateBy
                                           ,@UpdateAt
                                           ,@IsDeleted)";

						command.CommandText = query;
						command.Parameters.Add(new SqlParameter("@UserId", reservation.UserId));
						command.Parameters.Add(new SqlParameter("@RestaurantId", reservation.RestaurantId));
						command.Parameters.Add(new SqlParameter("@NumberOfCustomer", reservation.NumberOfCustomer));
						command.Parameters.Add(new SqlParameter("@PositionX", reservation.PositionX));
						command.Parameters.Add(new SqlParameter("@PositionY", reservation.PositionY));
						command.Parameters.Add(new SqlParameter("@DateReservation", reservation.DateReservation));
						command.Parameters.Add(new SqlParameter("@TimeReservation", reservation.TimeReservation));
						command.Parameters.Add(new SqlParameter("@CreateBy", reservation.CreateBy));
						command.Parameters.Add(new SqlParameter("@CreateAt", reservation.CreateAt));
						command.Parameters.Add(new SqlParameter("@IsDeleted", reservation.IsDeleted));
						command.Parameters.Add(new SqlParameter("@UpdateBy", reservation.UpdateBy));
						command.Parameters.Add(new SqlParameter("@UpdateAt", reservation.UpdateAt));

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

		public Task<bool> Update(Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
}

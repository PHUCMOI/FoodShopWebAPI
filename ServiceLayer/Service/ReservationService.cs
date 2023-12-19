using DataAccessLayer.DataAccessInterfaces;
using Microsoft.AspNetCore.Http;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using ServiceLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationDAO reservationDAO;
        public ReservationService(IReservationDAO reservationDAO)
        {
            this.reservationDAO = reservationDAO;
        }

		public async Task<List<TimeListResponse>> GetTimeList(GetTimeList getTimeList)
		{
			var listTime = GenerateTimeList(getTimeList.reservationDate);

			var listTimeAvailable = new List<TimeListResponse>();

			foreach (var time in listTime)
			{
				var timeAvailable = new TimeListResponse()
				{
					Time = time,
					Available = true
				};

				listTimeAvailable.Add(timeAvailable);
			}

			return listTimeAvailable; 
		}


		public List<string> GenerateTimeList(string reservationDate)
		{
			List<string> timeList = new List<string>();
			DateTime currentDate = DateTime.Parse(reservationDate);

			DateTime startTime;
			DateTime endTime;

			if (currentDate.Date == DateTime.Now.Date)
			{
				DateTime currentTime = DateTime.Now;
				int nextMultipleOf15 = (int)Math.Ceiling((double)currentTime.Minute / 60) * 60;
				startTime = currentTime.AddMinutes(nextMultipleOf15 - currentTime.Minute);
			}
			else
			{
				startTime = currentDate.Date.AddHours(9);
			}

			endTime = currentDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

			DateTime currentTimeSlot = startTime;
			while (currentTimeSlot <= endTime)
			{
				timeList.Add(FormatTime(currentTimeSlot));
				currentTimeSlot = currentTimeSlot.AddMinutes(60);
			}

			return timeList;
		}


		public string FormatTime(DateTime date)
		{
			string hours = date.Hour.ToString().PadLeft(2, '0');
			string minutes = date.Minute.ToString().PadLeft(2, '0');
			return $"{hours}:{minutes}";
		}

		public Task<bool> MakeReservation(ReservationRequest reservationRequest)
		{
			var reservation = new Reservation()
			{
				UserId = reservationRequest.UserID,
				RestaurantId = reservationRequest.RestaurantID,
				NumberOfCustomer = reservationRequest.NumberOfCustomer,
				PositionX = reservationRequest.PositionX,
				PositionY = reservationRequest.PositionY,
				DateReservation = DateTime.Parse(reservationRequest.DateReservation),
				TimeReservation = TimeSpan.Parse(reservationRequest.TimeReservation),
				CreateAt = DateTime.Now,
				CreateBy = reservationRequest.UserID,
				UpdateAt = DateTime.Now,
				UpdateBy = reservationRequest.UserID,
				IsDeleted = false
			};

			var res = reservationDAO.MakeReservation(reservation);
			return res;
		}
	}
}

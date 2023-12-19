using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
	public class ReservationRequest
	{
		public int UserID { get; set; }
		public int RestaurantID { get; set; }
		public int NumberOfCustomer { get; set; }
		public int PositionX { get; set; }
		public int PositionY { get; set; }
		public string DateReservation { get; set; }
		public string TimeReservation { get; set; }
	}
}

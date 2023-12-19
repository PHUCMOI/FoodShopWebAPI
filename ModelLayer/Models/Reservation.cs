using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId {  get; set; }
        public int NumberOfCustomer {  get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public DateTime DateReservation {  get; set; }
        public TimeSpan TimeReservation {  get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

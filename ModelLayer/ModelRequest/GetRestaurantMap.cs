using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
    public class GetRestaurantMap
    {
        public int RestaurantId { get; set; }   
        public int NumberPeople { get; set; }
        public string ReservationDate { get; set; }
        public string ReservationTime { get; set;}
    }
}

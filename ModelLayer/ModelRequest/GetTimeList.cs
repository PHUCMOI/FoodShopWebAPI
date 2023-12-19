using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
    public class GetTimeList
    {
        public int restaurantId { get; set; }
        public string reservationDate { get; set; }
    }
}

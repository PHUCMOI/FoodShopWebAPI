using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
    public class RestaurantMapRequest
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int TableId { get; set; }
    }
}

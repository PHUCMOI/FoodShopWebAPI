using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class RestaurantMap
    {
        public int RestaurantId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? IsDeleted { get; set; }
        public int TableId { get; set; }
    }
}

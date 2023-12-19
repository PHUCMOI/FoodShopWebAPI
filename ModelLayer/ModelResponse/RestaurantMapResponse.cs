using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelResponse
{
	public class RestaurantMapResponse
	{
		public int RestaurantId { get; set; }
		public int PositionX { get; set; }
		public int PositionY { get; set; }
		public int Cols { get; set; }
		public int Rows { get; set; }
		public bool isAvailable { get; set; }
		public int TableId {  get; set; }
	}
}

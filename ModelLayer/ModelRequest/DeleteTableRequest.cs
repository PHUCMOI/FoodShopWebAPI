using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
	public class DeleteTableRequest
	{
		public int UserId { get; set; }
		public int RestaurantId { get; set; }
		public int TableId {  get; set; }
	}
}

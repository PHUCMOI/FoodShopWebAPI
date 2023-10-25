using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
    public class DeleteOrderDetail
    {
        public int orderId { get; set; }
        public int productId {  get; set; }
    }
}

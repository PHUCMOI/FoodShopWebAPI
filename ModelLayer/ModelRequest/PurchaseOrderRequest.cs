using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelRequest
{
    public class PurchaseOrderRequest
    {
        public OrderRequest OrderRequest { get; set; }
        public UserRequest UserRequest { get; set; }
        public List<ProductRequest> Products { get; set; }
        public List<OrderDetailRequest> OrderDetails {get; set;}
    }
}

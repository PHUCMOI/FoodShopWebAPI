using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models_Layer.Enum.EnumModel;

namespace Models_Layer.ModelResponse
{
    public class OrderDetailModelView
    {
        public Order order { get; set; }
        public List<OrderDetailRequest> orderDetails { get; set; }
        public List<ProductRequest> products { get; set; }
        public PayMethod payMethod { get; set; }

    }

    
}

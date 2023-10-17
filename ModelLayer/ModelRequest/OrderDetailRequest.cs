using Fooding_Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelRequest
{
    public class OrderDetailRequest
    {
        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public ProductRequest Products { get; set; }
    }
}

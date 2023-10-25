using ModelLayer.ModelRequest;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelResponse
{
    public class CartResponse
    {
        public List<ProductRequest> product {  get; set; }
        public List<CartRequest> cart { get; set; }
    }
}

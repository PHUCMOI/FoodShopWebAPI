using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelResponse
{
    public class ProductModelView
    {
        public Product ProductRequest { get; set; }
        public List<CategoryRequest> CategoryRequest { get; set; }
    }
}

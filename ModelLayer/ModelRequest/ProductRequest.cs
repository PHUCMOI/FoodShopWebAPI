using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelRequest
{
    public partial class ProductRequest
    {
        public int ProductId { get; set; }
        
        public string? ProductName { get; set; }

        public string? ProductCategory { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; } 

        public string? ImgUrl { get; set; }
    }
}

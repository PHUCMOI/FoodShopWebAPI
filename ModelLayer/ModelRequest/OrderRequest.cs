using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelRequest
{
    public class OrderRequest
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public string? UserName { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        [StringLength(100, ErrorMessage = "Message cannot exceed 100 characters.")]
        public string? Message { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? PayMethod { get; set; }

        public string? Status { get; set; }

        public List<OrderDetailRequest> Details { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.ModelRequest
{
    public class PurchaseOrderRequest
    {
        public UserCheckout User { get; set; }
        public string? PayMethod { get; set; }
        public string? Status { get; set; }
        public string Message { get; set; }
        public string TotalPrice { get; set; }
        public List<OrderDetailCheckout> OrderDetail { get; set; }
    }

    public class OrderDetailCheckout
    {
        public int ProductId { get; set; }
        public int Quantity {  get; set; }
    }

    public class UserCheckout
    {
        public int UserId {  get; set; }
        public string UserName { get; set; }    
        public string PhoneNumber {  get; set; }
        public string Address { get; set; }
    }

    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }   
        public int UserId { get; set; }
        public UserCheckout User { get; set; }
        public string? PayMethod { get; set; }
        public string? Status { get; set; }
        public string Message { get; set; }
        public string TotalPrice { get; set; }
        public List<OrderDetailUpdate> OrderDetail { get; set; }
    }

    public class OrderDetailUpdate
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductRequest? Product { get; set; }
    }
}

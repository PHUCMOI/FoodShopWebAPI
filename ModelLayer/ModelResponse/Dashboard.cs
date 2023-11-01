using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelResponse
{
    public class Dashboard
    {
        public List<TopProduct> TopProductSellers {  get; set; }
        public List<TopUser> TopCustomer {  get; set; }
        public string TotalIncome {  get; set; }
    }

    public class TopProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }

    public class TopUser
    {
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public decimal? TotalOrderAmount { get; set; }
    }
}

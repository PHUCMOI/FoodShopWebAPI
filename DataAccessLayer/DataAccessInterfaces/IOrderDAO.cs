using Fooding_Shop.Models;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IOrderDAO
    {
        Task<List<OrderRequest>> GetAllOrders();
        Task<UpdateOrderRequest> GetOrder(int id);
        Task<bool> Create(Order order, List<OrderDetail> orderDetails);
        Task<bool> UpdateAsync(UpdateOrderRequest updateOrderRequest, int userID, decimal totalPrice);
        Task<bool> Delete(int id);

    }
}

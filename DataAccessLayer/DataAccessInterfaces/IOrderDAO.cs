using Fooding_Shop.Models;
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
        Task<OrderDetailModelView> GetOrder(int id);
        Task<bool> Create(Order order, List<OrderDetail> orderDetails);
        Task<bool> UpdateAsync(OrderDetailModelView orderDetailModelView, int userID, decimal totalPrice);
        bool Delete(int id);

    }
}

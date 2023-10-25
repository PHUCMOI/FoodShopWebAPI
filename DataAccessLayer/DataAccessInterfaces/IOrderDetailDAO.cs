using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IOrderDetailDAO
    {
        Task<List<OrderDetailUpdate>> GetOrderDetail(int orderID);
        Task<List<OrderDetailUpdate>> GetListOrderDetail();

        Task<bool> Update(List<OrderDetailUpdate> orderDetail, int orderId);
        Task<bool> Create(List<OrderDetail> orderDetails, int orderID);
        bool Delete(int orderID, int productID);
    }
}

using Fooding_Shop.Models;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<bool> CreateProductCart(CartRequest cart);
        Task<bool> Create(PurchaseOrderRequest purchaseOrderRequest);
        Task<List<OrderRequest>> GetOrderList();
        Task<OrderDetailModelView> GetOrderByID(int id);
        Task<bool> Update(OrderDetailModelView orderDetailModelView);
        Task<bool> Delete(int id);
        Task<bool> DeleteOrderDetail(int OrderID, int productID);

    }
}

using DataAccessLayer.DataAccess;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging.Abstractions;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using Services_Layer.ServiceInterfaces;
using System.Net.WebSockets;

namespace Services_Layer.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDetailDAO orderDetailDAO;
        private readonly IOrderDAO orderDAO;
        private readonly IUserDAO userDAO;
        private readonly ICartDAO cartDAO;
        public OrderService(IOrderDAO orderDAO, IOrderDetailDAO orderDetailDAO, IUserDAO userDAO, ICartDAO cartDAO)
        {
            this.orderDAO = orderDAO;
            this.orderDetailDAO = orderDetailDAO;
            this.userDAO = userDAO;
            this.cartDAO = cartDAO;
        }        

        public async Task<int> Create(PurchaseOrderRequest purchaseOrderRequest)
        {
            try
            {                
                if(purchaseOrderRequest != null)
                {
                    var Message = string.Empty;
                    if(purchaseOrderRequest.Message == null)
                    {
                        Message = "None";
                    }
                    else
                    {
                        Message = purchaseOrderRequest.Message;
                    }
                    var order = new Order()
                    {
                        UserId = purchaseOrderRequest.User.UserId,
                        UserName = purchaseOrderRequest.User.UserName,
                        Address = purchaseOrderRequest.User.Address,
                        TotalPrice = purchaseOrderRequest.TotalPrice,
                        PayMethod = purchaseOrderRequest.PayMethod,
                        Message = Message,
                        Status = "Prepare",
                        CreateBy = purchaseOrderRequest.User.UserId,
                        CreateDate = DateTime.Now,
                        UpdateBy = purchaseOrderRequest.User.UserId,
                        UpdateDate = DateTime.Now,
                        IsDeleted = false
                    };

                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    foreach(var orderRequest in purchaseOrderRequest.OrderDetail)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.ProductId = orderRequest.ProductId;
                        orderDetail.Quantity = orderRequest.Quantity;
                        orderDetail.CreateBy = purchaseOrderRequest.User.UserId;
                        orderDetail.CreateDate = DateTime.Now;
                        orderDetail.UpdateBy = purchaseOrderRequest.User.UserId;
                        orderDetail.UpdateDate = DateTime.Now; 
                        orderDetail.IsDeleted = false;

                        orderDetails.Add(orderDetail);
                    }

                    var result = await orderDAO.Create(order, orderDetails);
                    if (result != 0)
                    {
                        return result;
                    }
                    return 0;
                }
                else
                {
                    throw new Exception("Order is null");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<OrderRequest>> GetOrderList()
        {
            var orderList = await orderDAO.GetAllOrders();
            return orderList;
        }

        public async Task<UpdateOrderRequest> GetOrderByID(int id)
        {
            var order = await orderDAO.GetOrder(id);
            var orderDetail = await orderDetailDAO.GetOrderDetail(id);
            decimal totalPrice = 0;
            for (int i = 0; i < orderDetail.Count; i++)
            {
                totalPrice += orderDetail[i].Product.Price.Value * orderDetail[i].Quantity;
            }
            order.TotalPrice = Convert.ToInt64(totalPrice).ToString();
            return order;
        }

        public async Task<bool> Update(UpdateOrderRequest updateOrderRequest)
        {
            int userID = 1; // Temporary parameter
            if (updateOrderRequest != null)
            {
                var totalPrice = Convert.ToDecimal(updateOrderRequest.TotalPrice);
                var result = await orderDAO.UpdateAsync(updateOrderRequest, userID, totalPrice);
                if (result)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                bool flag = await orderDAO.Delete(id);
                if (flag)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<bool> DeleteOrderDetail(int orderID, int productID)
        {
            try
            {
                bool flag = orderDetailDAO.Delete(orderID, productID);
                if (flag)
                {
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrderStatus(int orderId, string status, int userId)
        {

            if (orderId != null)
            {
                orderDAO.UpdateOrderStatus(orderId, status);
                if(userId > 0)
                {
                    await cartDAO.ClearCart(userId);
                }
            }
        }
    }
}

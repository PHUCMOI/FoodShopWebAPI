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
        public OrderService(IOrderDAO orderDAO, IOrderDetailDAO orderDetailDAO)
        {
            this.orderDAO = orderDAO;
            this.orderDetailDAO = orderDetailDAO;
        }
        public async Task<bool> CreateProductCart(CartRequest cartRequest)
        {
            try
            {
                if (cartRequest != null)
                {                    
                    var cart = new Cart()
                    {
                        
                    };                    

                    var result = await orderDAO.CreateCart(cart);
                    if (result)
                    {
                        return true;
                    }
                    return false;
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

        public async Task<bool> Create(PurchaseOrderRequest purchaseOrderRequest)
        {
            try
            {                
                if(purchaseOrderRequest != null)
                {
                    var Message = string.Empty;
                    if(purchaseOrderRequest.OrderRequest.Message == null)
                    {
                        Message = "None";
                    }
                    else
                    {
                        Message = purchaseOrderRequest.OrderRequest.Message;
                    }
                    var order = new Order()
                    {
                        UserId = purchaseOrderRequest.UserRequest.UserId,
                        UserName = purchaseOrderRequest.UserRequest.UserName,
                        Address = purchaseOrderRequest.OrderRequest.Address,
                        TotalPrice = purchaseOrderRequest.OrderRequest.TotalPrice,
                        PayMethod = purchaseOrderRequest.OrderRequest.PayMethod,
                        Message = Message,
                        Status = "Prepare",
                        CreateBy = purchaseOrderRequest.UserRequest.UserId,
                        CreateDate = DateTime.Now,
                        UpdateBy = purchaseOrderRequest.UserRequest.UserId,
                        UpdateDate = DateTime.Now,
                        IsDeleted = false
                    };

                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    foreach(var orderRequest in purchaseOrderRequest.OrderDetails)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.ProductId = orderRequest.ProductId;
                        orderDetail.Quantity = orderRequest.Quantity;
                        orderDetail.CreateBy = purchaseOrderRequest.UserRequest.UserId;
                        orderDetail.CreateDate = DateTime.Now;
                        orderDetail.UpdateBy = purchaseOrderRequest.UserRequest.UserId;
                        orderDetail.UpdateDate = DateTime.Now; 
                        orderDetail.IsDeleted = false;

                        orderDetails.Add(orderDetail);
                    }

                    var result = await orderDAO.Create(order, orderDetails);
                    if (result)
                    {
                        return true;
                    }
                    return false;
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

        public async Task<OrderDetailModelView> GetOrderByID(int id)
        {
            var order = await orderDAO.GetOrder(id);
            var orderDetail = await orderDetailDAO.GetOrderDetail(id);
            decimal totalPrice = 0;
            for (int i = 0; i < orderDetail.Count; i++)
            {
                totalPrice += orderDetail[i].Products.Price.Value * orderDetail[i].Quantity.Value;
            }
            order.order.TotalPrice = Convert.ToInt64(totalPrice).ToString();
            return order;
        }

        public async Task<bool> Update(OrderDetailModelView orderDetailModelView)
        {
            int userID = 1; // Temporary parameter
            if (orderDetailModelView != null)
            {
                var totalPrice = Convert.ToDecimal(orderDetailModelView.order.TotalPrice);
                var result = await orderDAO.UpdateAsync(orderDetailModelView, userID, totalPrice);
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

        public Task<bool> Delete(int id)
        {
            try
            {
                bool flag = orderDAO.Delete(id);
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
    }
}

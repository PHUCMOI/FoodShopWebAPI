using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class OrderDAO : IOrderDAO
    {
        private readonly IConfiguration configuration;
        private readonly IOrderDetailDAO orderDetailDAO;
        private readonly IProductDAO productDAO;
        private readonly IUserDAO userDAO;
        private readonly FoodingShopContext _foodContext;
        public OrderDAO(IConfiguration configuration, IOrderDetailDAO orderDetailDAO, FoodingShopContext foodContext, IProductDAO productDAO, IUserDAO userDAO)
        {
            this.configuration = configuration;
            this.orderDetailDAO = orderDetailDAO;
            _foodContext = foodContext;
            this.productDAO = productDAO;
            this.userDAO = userDAO;
        }
        public async Task<int> Create(Order order, List<OrderDetail> orderDetails)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        var query = @"INSERT INTO [dbo].[Order]
                                           (UserID
                                           ,UserName
                                           ,Address
                                           ,TotalPrice
                                           ,PayMethod
                                           ,Status
                                           ,Message
                                           ,CreateBy
                                           ,CreateDate
                                           ,UpdateBy
                                           ,UpdateDate
                                           ,IsDeleted)
                                     VALUES
                                           (@UserID
                                           ,@UserName
                                           ,@Address
                                           ,@TotalPrice
                                           ,@PayMethod
                                           ,@Status
                                           ,@Message
                                           ,@CreateBy
                                           ,@Createdate
                                           ,@UpdateBy
                                           ,@UpdateDate
                                           ,@IsDeleted);
                                      SELECT CONVERT(int, SCOPE_IDENTITY()) as OrderID";

                        command.CommandText = query;
                        command.Parameters.Add(new SqlParameter("@UserID", order.UserId));
                        command.Parameters.Add(new SqlParameter("@UserName", order.UserName));
                        command.Parameters.Add(new SqlParameter("@Address", order.Address));
                        command.Parameters.Add(new SqlParameter("@TotalPrice", order.TotalPrice));
                        command.Parameters.Add(new SqlParameter("@PayMethod", order.PayMethod));
                        command.Parameters.Add(new SqlParameter("@Status", order.Status));
                        command.Parameters.Add(new SqlParameter("@Message", order.Message));
                        command.Parameters.Add(new SqlParameter("@CreateBy", order.CreateBy));
                        command.Parameters.Add(new SqlParameter("@CreateDate", order.CreateDate));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", order.IsDeleted));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", order.UpdateBy));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", order.UpdateDate));

                        int orderID = (int)command.ExecuteScalar();
                        con.Close();

                        if(orderID > 0)
                        {
                            var result = await orderDetailDAO.Create(orderDetails, orderID);
                            if (result)
                            {
                                return orderID;
                            }
                        }
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }        

        public async Task<bool> Delete(int id)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        con.Open();
                        var query = @"UPDATE [dbo].[Order]
                                  SET IsDeleted = 1
                                  WHERE OrderID = @OrderID";

                        command.Parameters.Add(new SqlParameter("@OrderID", id));
                        command.CommandText = query;
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();
                        var queryDetail = @"UPDATE [dbo].[OrderDetail]
                                  SET IsDeleted = 1
                                  WHERE OrderID = @OrderID";
                        command.Parameters.Add(new SqlParameter("@OrderID", id));
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        con.Close();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };
        }

        public async Task<List<OrderRequest>> GetAllOrders()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = @"SELECT OrderID,
                                         UserId,
                                         UserName,
                                         Address,
                                         PayMethod,
                                         TotalPrice,
                                         Status,
                                         Message
                                      FROM [dbo].[Order]
                                      WHERE IsDeleted = 0";
                    var orderList = await con.QueryAsync<OrderRequest>(query);
                    return orderList.ToList();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateOrderRequest> GetOrder(int id)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT OrderID
                                         ,UserID
                                         ,UserName
                                         ,Address
                                         ,PayMethod
                                         ,TotalPrice
                                         ,Status
                                         ,Message
                                         ,CreateBy
                                         ,CreateDate
                                         ,UpdateBy
                                         ,UpdateDate
                                         ,IsDeleted
                                      FROM [dbo].[Order] 
                                      WHERE OrderID = {id}";
                    var order = await con.QuerySingleOrDefaultAsync<Order>(query, new { Id = id });
                    var orderDetail = await orderDetailDAO.GetOrderDetail(order.OrderId);
                    var productlist = await productDAO.GetListProduct();
                    var user = await userDAO.GetUser((int)order.UserId);
                    var userCheckout = new UserCheckout()
                    {
                        UserId = (int)order.UserId,
                        UserName = order.UserName,
                        Address = order.Address,
                        PhoneNumber = user.PhoneNumber
                    };
                    return new UpdateOrderRequest()
                    {
                        OrderId = order.OrderId,
                        UserId = (int)order.UserId,
                        PayMethod = order.PayMethod,
                        Status = order.Status,
                        Message = order.Message,
                        TotalPrice = order.TotalPrice,
                        OrderDetail = orderDetail,
                        User = userCheckout                    };
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(UpdateOrderRequest updateOrderRequest, int userID, decimal totalPrice)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {

                    using (var command = _foodContext.Database.GetDbConnection().CreateCommand())
                    {
                        await _foodContext.Database.OpenConnectionAsync();

                        var query = $@"UPDATE [dbo].[Order]
                           SET UserName = @UserName
                              ,Address = @Address
                              ,TotalPrice = @TotalPrice
                              ,PayMethod = @PayMethod
                              ,Status = @Status
                              ,Message = @Message
                              ,UpdateBy = @UpdateBy
                              ,UpdateDate = @UpdateDate
                              ,IsDeleted = @IsDeleted
                            WHERE OrderID = @OrderID";

                        command.CommandText = query;

                        command.Parameters.Add(new SqlParameter("@UserName", updateOrderRequest.User.UserName));
                        command.Parameters.Add(new SqlParameter("@Address", updateOrderRequest.User.Address));
                        command.Parameters.Add(new SqlParameter("@TotalPrice", totalPrice.ToString()));
                        command.Parameters.Add(new SqlParameter("@PayMethod", updateOrderRequest.PayMethod));
                        command.Parameters.Add(new SqlParameter("@Status", updateOrderRequest.Status));
                        command.Parameters.Add(new SqlParameter("@Message", updateOrderRequest.Message));
                        command.Parameters.Add(new SqlParameter("@IsDeleted", false));
                        command.Parameters.Add(new SqlParameter("@UpdateBy", userID));
                        command.Parameters.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("@OrderID", updateOrderRequest.OrderId));

                        var result = await command.ExecuteNonQueryAsync();
                        if(result != null)
                        {
                            try
                            {
                                var flag = await orderDetailDAO.Update(updateOrderRequest.OrderDetail, updateOrderRequest.OrderId);
                                if (flag)
                                {
                                    return true;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("OrderDetail update failed", ex);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public void UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var updateOrderQuery = $@"UPDATE [dbo].[Order]
                                              SET [Status] = '{status}'
                                              WHERE OrderID = {orderId}";
                    connection.Execute(updateOrderQuery);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

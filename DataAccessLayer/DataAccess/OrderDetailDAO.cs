using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models_Layer.ModelRequest;
using System.Data;

namespace DataAccessLayer.DataAccess
{
    public class OrderDetailDAO : IOrderDetailDAO
    {
        private readonly IConfiguration configuration;
        private readonly IProductDAO productDAO;
        private readonly FoodingShopContext _foodContext;
        public OrderDetailDAO(IConfiguration configuration, IProductDAO productDAO, FoodingShopContext foodContext)
        {
            this.configuration = configuration;
            this.productDAO = productDAO;
            _foodContext = foodContext;
        }

        public async Task<bool> Create(List<OrderDetail> orderDetails, int orderID)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        await con.OpenAsync();
                        foreach(var order in orderDetails)
                        {
                            var query = @"INSERT INTO [dbo].[OrderDetail]
                                               (OrderID
                                               ,ProductID
                                               ,Quantity
                                               ,CreateBy
                                               ,CreateDate
                                               ,UpdateBy
                                               ,UpdateDate
                                               ,IsDeleted)
                                         VALUES
                                               (@OrderID
                                               ,@ProductID
                                               ,@Quantity
                                               ,@CreateBy
                                               ,@Createdate
                                               ,@UpdateBy
                                               ,@UpdateDate
                                               ,@IsDeleted)";

                            command.CommandText = query;
                            command.Parameters.Clear();
                            command.Parameters.Add(new SqlParameter("@OrderID", orderID));
                            command.Parameters.Add(new SqlParameter("@ProductID", order.ProductId));
                            command.Parameters.Add(new SqlParameter("@Quantity", order.Quantity));
                            command.Parameters.Add(new SqlParameter("@CreateBy", order.CreateBy));
                            command.Parameters.Add(new SqlParameter("@CreateDate", order.CreateDate));
                            command.Parameters.Add(new SqlParameter("@IsDeleted", order.IsDeleted));
                            command.Parameters.Add(new SqlParameter("@UpdateBy", order.UpdateBy));
                            command.Parameters.Add(new SqlParameter("@UpdateDate", order.UpdateDate));

                            var result = command.ExecuteNonQuery();
                            con.Close();
                            if(result != null)
                            {
                                return true;
                            }
                            return false;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public bool Delete(int orderID, int productID)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = con.CreateCommand())
                    {
                        con.Open();
                        var query = @"UPDATE [dbo].[OrderDetail]
                                  SET IsDeleted = 1
                                  WHERE OrderID = @OrderID AND ProductID = @ProductID";

                        command.Parameters.Add(new SqlParameter("@OrderID", orderID));
                        command.Parameters.Add(new SqlParameter("@ProductID", productID));
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

        public async Task<List<OrderDetailRequest>> GetOrderDetail(int orderID)
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT OrderID
                                          ,ProductID
                                          ,Quantity
                                          ,CreateBy
                                          ,CreateDate
                                          ,UpdateBy
                                          ,UpdateDate
                                          ,IsDeleted
                                      FROM [dbo].[OrderDetail]
                                      WHERE OrderID = {orderID}
                                      AND IsDeleted <> 1 ";
                    var orderDetail = await con.QueryAsync<OrderDetailRequest>(query, new { Id = orderID });
                    orderDetail.ToList();

                    foreach (var order in orderDetail)
                    {
                        var product = await productDAO.GetProduct((int)order.ProductId);
                        order.Products = new ProductRequest();
                        order.Products.ProductName = product.ProductName;
                        order.Products.ProductCategory = product.ProductCategory;
                        order.Products.Price = product.Price;
                    }                   

                    return (List<OrderDetailRequest>)orderDetail;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(List<OrderDetailRequest> orderDetail)
        {
            using (var con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
            {
                try
                {
                    using (var command = _foodContext.Database.GetDbConnection().CreateCommand())
                    {
                        int count = 0;
                        foreach (var order in orderDetail)
                        {
                            var query = $@"UPDATE OrderDetail
                                          SET Quantity = @Quantity
                                          WHERE OrderID = @OrderID AND ProductID = @ProductID";

                            command.CommandText = query;
                            command.CommandType = CommandType.Text;
                            command.Parameters.Clear();
                            command.Parameters.Add(new SqlParameter("@Quantity", order.Quantity));
                            command.Parameters.Add(new SqlParameter("@OrderID", order.OrderId));
                            command.Parameters.Add(new SqlParameter("@ProductID", order.ProductId));
                            await _foodContext.Database.OpenConnectionAsync();
                            command.ExecuteNonQuery();
                            count++;
                        }
                        if (count == orderDetail.Count)
                        {
                            con.Close();
                            return true;
                        }
                        
                    }
                    con.Close();
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}

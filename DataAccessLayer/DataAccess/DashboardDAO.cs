using Dapper;
using DataAccessLayer.DataAccessInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ModelLayer.ModelResponse;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class DashboardDAO : IDashboardDAO
    {
        private readonly IConfiguration configuration;
        public DashboardDAO(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }

        public async Task<List<BarChartCategory>> BarChartData()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT
                        c.CategoryName AS Category,
                        SUM(od.Quantity) AS TotalQuantity
                    FROM
                        Category c
                    LEFT JOIN
                        Products p
                    ON
                        c.CategoryName = p.ProductCategory
                    LEFT JOIN
                        OrderDetail od
                    ON
                        od.ProductId = p.ProductId
                    WHERE c.IsDeleted = 0 AND od.IsDeleted = 0
                    GROUP BY
                        c.CategoryName;
                    ";
                    var data = await con.QueryAsync<BarChartCategory>(query);

                    return data.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TopUser>> topCustomers()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT TOP 3
                                        u.UserId,
                                        SUM(CONVERT(FLOAT, TotalPrice)) AS TotalOrderAmount,
                                        u.UserName
                                    FROM [dbo].[User] u
                                    LEFT JOIN [dbo].[Order] o ON u.UserId = o.UserId
                                    WHERE o.IsDeleted = 0
                                    GROUP BY u.UserId, u.UserName
                                    ORDER BY TotalOrderAmount DESC;";
                    var data = await con.QueryAsync<TopUser>(query);

                    return data.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TopProduct>> topProducts()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT TOP 3
                                        od.ProductId,
                                        SUM(od.Quantity) AS Quantity,
                                        p.ProductName
                                    FROM OrderDetail od
                                    LEFT JOIN Products p ON od.ProductId = p.ProductId
                                    WHERE od.IsDeleted = 0
                                    GROUP BY od.ProductId, p.ProductName
                                    ORDER BY Quantity DESC;";
                    var data = await con.QueryAsync<TopProduct>(query);

                    return data.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<string> TotalIncome()
        {
            try
            {
                using (IDbConnection con = new SqlConnection(configuration.GetConnectionString("FoodingShopDB")))
                {
                    var query = $@"SELECT SUM(CONVERT(FLOAT, TotalPrice)) AS TotalIncome
                    FROM [dbo].[Order]
                    WHERE IsDeleted = 0";

                    var totalIncome = con.QueryFirstOrDefault<double>(query);

                    return Task.FromResult(totalIncome.ToString());
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

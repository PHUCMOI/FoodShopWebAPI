using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using ModelLayer.ModelResponse;
using ServiceLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly ICategoryDAO categoryDAO;    
        private readonly IProductDAO productDAO;
        private readonly IOrderDetailDAO orderDetailDAO;
        public DashboardService(ICategoryDAO categoryDAO, IProductDAO productDAO, IOrderDetailDAO orderDetailDAO) 
        {
            this.categoryDAO = categoryDAO;
            this.productDAO = productDAO;
            this.orderDetailDAO = orderDetailDAO;
        }

        public async Task<List<BarChartCategory>> GetDataBarChart()
        {
            try 
            {
                var categories = await categoryDAO.GetListCategory();
                var products = await productDAO.GetListProduct();
                var orderDetails = await orderDetailDAO.GetListOrderDetail();
                var categoryQuantities = categories
                        .GroupJoin(
                            products,
                            category => category.CategoryName,
                            product => product.ProductCategory,
                            (category, productGroup) => new
                            {
                                Category = category,
                                TotalQuantity = productGroup
                                    .SelectMany(product => orderDetails
                                        .Where(orderDetail => orderDetail.ProductId == product.ProductId))
                                    .Sum(orderDetail => orderDetail.Quantity)
                            }
                        );

                var barchartData = new List<BarChartCategory>();
                foreach (var categoryQuantity in categoryQuantities)
                {
                    var barchartItem = new BarChartCategory()
                    {
                        Category = categoryQuantity.Category.CategoryName,
                        Count = categoryQuantity.TotalQuantity                        
                    };
                    barchartData.Add(barchartItem);
                }
                return barchartData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<PieChartData>> GetDataPieChart()
        {
            try
            {
                var categories = await categoryDAO.GetListCategory();
                var products = await productDAO.GetListProduct();

                var groupedProducts = products.GroupBy(p => p.ProductCategory);

                var categoryCounts = groupedProducts.Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count()
                });

                int totalProducts = products.Count;

                var categoryPercentages = categoryCounts.Select(c => new
                {
                    Category = c.Category,
                    Percentage = (double)c.Count / totalProducts * 100
                });
                var pieChartData = new List<PieChartData>();
                foreach (var categoryPercentage in categoryPercentages)
                {
                    var pieChartItem = new PieChartData()
                    {
                        Category = categoryPercentage.Category,
                        Value = categoryPercentage.Percentage
                    };
                    pieChartData.Add(pieChartItem);
                }
                return pieChartData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

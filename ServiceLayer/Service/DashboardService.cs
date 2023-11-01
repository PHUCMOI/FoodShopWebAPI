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
        private readonly IOrderDAO orderDAO;
        private readonly IUserDAO userDAO;
        private readonly IDashboardDAO dashboardDAO;
        public DashboardService(ICategoryDAO categoryDAO, IProductDAO productDAO, IOrderDetailDAO orderDetailDAO, IOrderDAO orderDAO, IUserDAO userDAO, IDashboardDAO dashboardDAO) 
        {
            this.categoryDAO = categoryDAO;
            this.productDAO = productDAO;
            this.orderDetailDAO = orderDetailDAO;
            this.orderDAO = orderDAO;
            this.userDAO = userDAO;
            this.dashboardDAO = dashboardDAO;
        }

        public async Task<Dashboard> DashboardData()
        {
            try
            {
                var totalIncome = await dashboardDAO.TotalIncome();
                var topProductSellers = await dashboardDAO.topProducts();
                var topCustomer = await dashboardDAO.topCustomers();

                var dashboardData = new Dashboard()
                {
                    TotalIncome = totalIncome,
                    TopProductSellers = topProductSellers, 
                    TopCustomer = topCustomer,
                };
                return dashboardData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BarChartCategory>> GetDataBarChart()
        {
            try 
            {
                var barchartData = await dashboardDAO.BarChartData();
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

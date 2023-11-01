using ModelLayer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IDashboardDAO
    {
        Task<List<BarChartCategory>> BarChartData();

        Task<string> TotalIncome();
        Task<List<TopProduct>> topProducts();
        Task<List<TopUser>> topCustomers();
    }
}

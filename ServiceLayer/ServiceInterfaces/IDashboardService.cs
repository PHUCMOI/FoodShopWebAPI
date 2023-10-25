using ModelLayer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IDashboardService
    {
        Task<List<PieChartData>> GetDataPieChart();
        Task<List<BarChartCategory>> GetDataBarChart();

    }
}

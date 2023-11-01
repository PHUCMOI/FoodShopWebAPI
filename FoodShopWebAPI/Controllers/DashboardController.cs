using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelResponse;
using ServiceLayer.ServiceInterfaces;

namespace FoodShopWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [HttpGet("piechart")]
        public async Task<ActionResult<List<PieChartData>>> GetPieChartData()
        {
            try
            {
                var data = await dashboardService.GetDataPieChart();
                return Json(data);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("barchartcategory")]
        public async Task<ActionResult<List<PieChartData>>> GetBarChartData()
        {
            try
            {
                var data = await dashboardService.GetDataBarChart();
                return Json(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PieChartData>>> DashboardData()
        {
            try
            {
                var data = await dashboardService.DashboardData();
                return Json(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

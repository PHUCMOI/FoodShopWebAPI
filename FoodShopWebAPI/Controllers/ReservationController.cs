using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ServiceLayer.ServiceInterfaces;

namespace FoodShopWebAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class ReservationController : Controller
    {
        private readonly IReservationService reservationService;
        public ReservationController(IReservationService reservationService) 
        {
            this.reservationService = reservationService;
        }

		[HttpPost("GetTimeList")]
		public async Task<ActionResult<List<TimeListResponse>>> GetTimeList([FromBody] GetTimeList getTimeList)
		{
			return await reservationService.GetTimeList(getTimeList);
		}

		[HttpPost("MakeReservation")]
		public async Task<ActionResult> MakeReservation([FromBody] ReservationRequest reservationRequest)
		{
			return Ok(await reservationService.MakeReservation(reservationRequest));
		}
	}
}

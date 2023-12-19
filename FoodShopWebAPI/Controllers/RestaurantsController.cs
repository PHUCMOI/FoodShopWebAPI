using Fooding_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using ServiceLayer.ServiceInterfaces;
using Services_Layer.ServiceInterfaces;

namespace FoodShopWebAPI.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class RestaurantsController : Controller
	{
		private readonly IRestaurantMapService restaurantMapService;
		private readonly IRestaurantService restaurantService;
		public RestaurantsController(IRestaurantService restaurantService, IRestaurantMapService restaurantMapService)
		{
			this.restaurantService = restaurantService;
			this.restaurantMapService = restaurantMapService;
		}
		// GET: CategoryController
		[HttpGet]
		public async Task<ActionResult<List<Restaurant>>> Index()
		{
			try
			{
				var restaurantList = await restaurantService.GetListRestaurant();
				if (restaurantList != null)
				{
					return Ok(restaurantList);
				}
				else
				{
					return BadRequest("restaurant list is null");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET: CategoryController/Details/5
		[HttpGet("Details/{id}")]
		public async Task<ActionResult<Restaurant>> Details(int id)
		{
			try
			{
				var restaurant = await restaurantService.GetRestaurantById(id);
				if (restaurant != null)
				{
					return Ok(restaurant);
				}
				else
				{
					return BadRequest("category is null");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST: CategoryController/Create
		[HttpPost("Create")]
		public async Task<ActionResult<bool>> Create(Restaurant restaurant)
		{
			try
			{
				if (restaurant != null)
				{
					var res = await restaurantService.Create(restaurant);
					if (res)
					{
						return Ok(res);
					}
				}
				return BadRequest("failed");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// GET: CategoryController/Edit/5
		[HttpPut("Update/{id}")]
		public async Task<ActionResult<bool>> Update(Restaurant restaurant)
		{
			try
			{
				if (restaurant != null)
				{
					var result = await restaurantService.Update(restaurant);
					if (result)
					{
						return Ok(result);
					}
				}
				return BadRequest("failed");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		// GET: CategoryController/Delete/5
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				if (id > 0)
				{
					var result = await restaurantService.Delete(id);
					if (result)
					{
						return Ok(result);
					}
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("CreateMaps")]
		public async Task<IActionResult> CreateMaps(List<RestaurantMapRequest> map)
		{
			try
			{
				if (map != null)
				{
					var res = await restaurantMapService.CreateMaps(map);
					return Ok(res);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("updateMaps")]
		public async Task<IActionResult> UpdateMaps(List<RestaurantMapRequest> map)
		{
			try
			{
				if (map != null)
				{
					var res = await restaurantMapService.UpdateMaps(map);
					return Ok(res);
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("GetMaps")]
		public async Task<ActionResult<List<RestaurantMapResponse>>> GetRestaurantMaps(GetRestaurantMap getRestaurantMap)
		{
			try
			{
				var restaurantList = await restaurantMapService.GetRestaurantMaps(getRestaurantMap);
				if (restaurantList != null)
				{
					return Ok(restaurantList);
				}
				else
				{
					return BadRequest("restaurant map list is null");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


	}
}
using Microsoft.AspNetCore.Mvc;
using Fooding_Shop.Models;
using Services_Layer.ServiceInterfaces;
using Models_Layer.ModelRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using ModelLayer.ModelRequest;

namespace Fooding_Shop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //POST: Register
        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult Register(RegisterRequest registerRequest)
        {
            if (registerRequest != null)
            {
                try
                {
                    _userService.Register(registerRequest);
                    return Ok(registerRequest);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                {
                    return BadRequest();
                }
                else
                {
                    var token = await _userService.Login(loginRequest);
                    return Ok(new TokenApiDTO()
                    {
                        AccessToken = token,
                        RefreshToken = ""
                        
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Users
        [HttpGet]
        public async Task<ActionResult<List<UserRequest>>> Index()
        {
            try
            {
                var userList = await _userService.GetUserList();
                if (userList != null)
                {
                    return Ok(userList);
                }
                else
                {
                    return BadRequest("user list is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Users/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<UserRequest>> Details(int id)
        {
            try
            {
                var user = await _userService.GetUserByID(id);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return BadRequest("user is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserRequest userRequest)
        {
            try
            {
                if (userRequest != null)
                {
                    var result = await _userService.Create(userRequest);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Users/Edit/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {

                if (user != null)
                {
                    var result = await _userService.UpdateAsync(user);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Users/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    var result = await _userService.Delete(id);
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
    }
}

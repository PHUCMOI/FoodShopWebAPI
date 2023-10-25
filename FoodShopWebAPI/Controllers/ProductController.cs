using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using Newtonsoft.Json;
using Services_Layer.ServiceInterfaces;
using System.Text;

namespace Fooding_Shop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly ICheckImageService checkImageService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;
        public ProductController(IProductService productService,
            IHttpContextAccessor httpContextAccessor,
            ICheckImageService checkImageService,
            IWebHostEnvironment environment,
            ICategoryService categoryService,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            this.productService = productService;
            this.httpContextAccessor = httpContextAccessor;
            this.checkImageService = checkImageService;
            this.environment = environment;
            this.categoryService = categoryService;
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductRequest>>> GetListProduct()
        {
            try
            {
                var productList = await productService.GetProductList();
                if (productList != null)
                {
                    return Ok(productList);
                }
                else
                {
                    return BadRequest();
                }            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Search/")]
        public async Task<ActionResult<List<ProductRequest>>> SearchProducts(string? value, decimal? minPrice, decimal? maxPrice)
       {
            try
            {
                var productList = await productService.SearchProducts(value, minPrice, maxPrice);
                if (productList != null)
                {
                    return Ok(productList);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<ActionResult<ProductRequest>> Details(int id)
        {
            try
            {
                var product = await productService.GetProductByID(id);
                if (product != null)
                {
                    return product;
                }
                else
                {
                    return BadRequest("product is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(ProductRequest productRequest)
        {
            try
            {
                var session = httpContextAccessor.HttpContext.Session;
                var userDataBytes = session.Get("UserData");
                UserRequest user = new UserRequest();
                if (userDataBytes != null)
                {
                    var userDataJson = Encoding.UTF8.GetString(userDataBytes);
                    user = JsonConvert.DeserializeObject<UserRequest>(userDataJson);
                }
                var isImage = checkImageService.IsImage(productRequest.ImgUrl);
                if (isImage)
                {
                    if (productRequest != null)
                    {
                        var result = await productService.UpdateAsync(productRequest, user.UserId);
                        if (result)
                        {
                            return Ok(result);
                        }
                        return BadRequest();
                    }
                    return BadRequest("Need more product information");
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                var result = await productService.Delete(id);
                if(result)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductRequest productRequest)
        {
            var session = httpContextAccessor.HttpContext.Session;
            var userDataBytes = session.Get("UserData");
            UserRequest user = new UserRequest();
            if (userDataBytes != null)
            {
                var userDataJson = Encoding.UTF8.GetString(userDataBytes);
                user = JsonConvert.DeserializeObject<UserRequest>(userDataJson);
            }
            var isImage = checkImageService.IsImage(productRequest.ImgUrl);
            if(isImage)
            {
                if (productRequest != null)
                {
                    var result = await productService.Create(productRequest, user.UserId);
                    if (result)
                    {
                        return Ok(result);
                    }
                    return BadRequest();
                }
                return BadRequest("Need more product information");
            }
            return BadRequest("Not is a image");
        }

    }
}

using Fooding_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models_Layer.ModelRequest;
using Services_Layer.Service;
using Services_Layer.ServiceInterfaces;
using System.Data;

namespace Fooding_Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        // GET: CategoryController
        [HttpGet]
        public async Task<ActionResult<List<CategoryRequest>>> Index()
        {
            try
            {
                var categoryList = await categoryService.GetCategoryList();
                if (categoryList != null)
                {
                    return Ok(categoryList);
                }
                else
                {
                    return BadRequest("category list is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: CategoryController/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<CategoryRequest>> Details(int id)
        {
            try
            {
                var category = await categoryService.GetCategoryByID(id);
                if (category != null)
                {
                    return Ok(category);
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
        public async Task<ActionResult<bool>> Create(CategoryRequest categoryRequest)
        {
            try
            {
                if (categoryRequest != null)
                {
                    var category = await categoryService.Create(categoryRequest);
                    if (category)
                    {
                        return Ok(category);
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
        public async Task<ActionResult<bool>> Update(Category category)
        {
            try
            {
                if(category  != null)
                {
                    var result = await categoryService.UpdateAsync(category);
                    if(result)
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
                if(id > 0)
                {
                    var result = await categoryService.Delete(id);
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
    }
}

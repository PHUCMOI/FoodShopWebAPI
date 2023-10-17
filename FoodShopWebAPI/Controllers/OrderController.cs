﻿using Fooding_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using Newtonsoft.Json;
using Services_Layer.Service;
using Services_Layer.ServiceInterfaces;
using System.Text;

namespace Fooding_Shop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        public OrderController(IOrderService orderService, IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.httpContextAccessor = httpContextAccessor;
        }
        // GET: OrderController
        [HttpGet]
        public async Task<ActionResult<List<OrderRequest>>> Index()
        {
            try
            {
                var orderList = await orderService.GetOrderList();
                if (orderList != null)
                {
                    return Ok(orderList);
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

        // GET: OrderController/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<OrderDetailModelView>> Details(int id)
        {
            try
            {
                var order = await orderService.GetOrderByID(id);
                if (order != null)
                {
                    return Ok(order);
                }
                else
                {
                    return BadRequest("order is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: OrderController/Create
        [HttpPost("Create/{id}")]
        public async Task<ActionResult> CreateAsync(PurchaseOrderRequest purchaseOrderRequest)
        {
            try
            {
                if(purchaseOrderRequest != null)
                {
                    var result = await orderService.Create(purchaseOrderRequest);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest() ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult> AddToCart(CartRequest cart)
        {
            try
            {
                if (cart != null)
                {
                    var result = await orderService.CreateProductCart(cart);
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

        // GET: OrderController/Edit/5
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(OrderDetailModelView orderDetailModelView)
        {
            try
            {
                if(orderDetailModelView != null)
                {
                    var result = await orderService.Update(orderDetailModelView);
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

        // POST: OrderController/Delete/5
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if(id > 0)
                {
                    var result = await orderService.Delete(id);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteOrderDetail/{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int orderID, int productID)
        {
            try
            {
                if (orderID > 0 && productID > 0)
                {
                    var result = await orderService.DeleteOrderDetail(orderID, productID);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

    }
}
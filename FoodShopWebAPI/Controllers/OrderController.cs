﻿using Fooding_Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using ModelLayer.PayPalModel;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using Newtonsoft.Json;
using ServiceLayer.ServiceInterfaces;
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
        private readonly IPaypalService paypalService;
        public OrderController(IOrderService orderService, IProductService productService, IHttpContextAccessor httpContextAccessor, IPaypalService paypalService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.httpContextAccessor = httpContextAccessor;
            this.paypalService = paypalService;
        }
        // GET: OrderController
        [HttpGet]
        public async Task<ActionResult<List<OrderRequest>>> GetListOrder()
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
        public async Task<ActionResult<UpdateOrderRequest>> Details(int id)
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
        [HttpPost("Create")]
        public async Task<ActionResult> CreateAsync(PurchaseOrderRequest purchaseOrderRequest)
        {
            try
            {
                if(purchaseOrderRequest != null)
                {
                    var result = await orderService.Create(purchaseOrderRequest);
                    if (result != 0)
                    {
                        return Ok(true);
                    }
                }
                return BadRequest() ;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // GET: OrderController/Edit/5
        [HttpPost("Update")]
        public async Task<ActionResult> Update(UpdateOrderRequest updateOrderRequest)
        {
            try
            {
                if(updateOrderRequest != null)
                {
                    var result = await orderService.Update(updateOrderRequest);
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
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
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

        [HttpPost("DeleteOrderDetail")]
        public async Task<IActionResult> DeleteOrderDetail(DeleteOrderDetail deleteOrderDetail)
        {
            try
            {
                if (deleteOrderDetail != null)
                {
                    var result = await orderService.DeleteOrderDetail(deleteOrderDetail.orderId, deleteOrderDetail.productId);
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

        [HttpPost("paypal")]
        public async Task<ActionResult<PayPalPayment>> PaymentPayPal(PurchaseOrderRequest purchaseOrderRequest)
        {
            try
            {
                if(purchaseOrderRequest != null)
                {
                    var payment = await paypalService.CreatePaymentUrl(purchaseOrderRequest);
                    return Ok(payment);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("PaymentSuccess")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] int orderId, int userId)
        {
            await orderService.UpdateOrderStatus(orderId, "Paid", userId);

            return Redirect("http://localhost:4200/product");
        }

        [AllowAnonymous]
        [HttpGet("PaymentFail")]
        public async Task<IActionResult> PaymentFail([FromQuery] int orderId)
        {
            await orderService.UpdateOrderStatus(orderId, "Prepare", 0);

            return Ok(false);
        }
    }
}

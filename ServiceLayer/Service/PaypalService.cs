using DataAccessLayer.DataAccessInterfaces;
using Microsoft.Extensions.Configuration;
using ModelLayer.PayPalModel;
using Models_Layer.ModelRequest;
using PayPal.Core;
using PayPal.v1.Payments;
using ServiceLayer.ServiceInterfaces;
using Services_Layer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer.Service
{
    public class PaypalService : IPaypalService
    {
        private readonly IProductDAO productDAO;
        private readonly IConfiguration configuration;
        private readonly IOrderService orderService;
        public PaypalService(IConfiguration configuration, IProductDAO productDAO, IOrderService orderService) 
        {
            this.configuration = configuration;
            this.productDAO = productDAO;
            this.orderService = orderService;
        }

        public async Task<PayPalPayment> CreatePaymentUrl(PurchaseOrderRequest purchaseOrderRequest)
        {
            var paypalOrderId = await orderService.Create(purchaseOrderRequest);
            var userId = purchaseOrderRequest.User.UserId;
            var returnUrl = $"{configuration["PaymentCallBack:ReturnUrl"]}/api/order/PaymentSuccess";
            var cancelUrl = $"{configuration["PaymentCallBack:ReturnUrl"]}/api/order/PaymentFail";           

            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            decimal total = 0;
            foreach (var item in purchaseOrderRequest.OrderDetail)
            {
                var product = await productDAO.GetProductNameByID(item.ProductId);
                itemList.Items.Add(new Item()
                {
                    Name = product.ProductName,
                    Currency = "USD",
                    Price = Convert.ToInt32(product.Price).ToString(),
                    Quantity = item.Quantity.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
                decimal c = Math.Round(product.Price.Value * item.Quantity);
                total += c;
            }

            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }
                        },
                        ItemList = itemList,
                        Description = $"Invoice #{purchaseOrderRequest.Message}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl =
                       $"{returnUrl}?payment_method=PayPal&success=1&orderId={paypalOrderId}&userId={userId}",
                    CancelUrl =
                        $"{cancelUrl}?payment_method=PayPal&success=0&orderId={paypalOrderId}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            return await ExecutePaymentAsync(payment);
        }

        public async Task<PayPalPayment> ExecutePaymentAsync(Payment payment)
        {
            var envSandbox = new SandboxEnvironment(configuration["Paypal:ClientId"], configuration["Paypal:SecretKey"]);
            var client = new PayPalHttpClient(envSandbox);

            var request = new PaymentCreateRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
            {
                var errorResponse = new PayPalPayment
                {
                    url = null,
                    statusCode = ((int)statusCode).ToString(),
                    errorCode = null,
                    Message = "Invalid status code: " + statusCode.ToString()
                };

                return errorResponse;
            }
            var result = response.Result<Payment>();
            using var links = result.Links.GetEnumerator();


            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }
            var reponsePayPal = new PayPalPayment
            {
                url = paymentUrl,
                statusCode = ((int)statusCode).ToString(),
                errorCode = null,
                Message = "Success"
            };

            return reponsePayPal;
        }
    }
}

using ModelLayer.PayPalModel;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IPaypalService
    {
        Task<PayPalPayment> CreatePaymentUrl(PurchaseOrderRequest purchaseOrderRequest);
    }
}

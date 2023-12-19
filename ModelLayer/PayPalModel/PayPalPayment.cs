using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.PayPalModel
{
    public class PayPalPayment
    {
        public string url { get; set; }
        public string statusCode { get; set; }
        public string errorCode { get; set; }
        public string Message { get; set; }
    }
}

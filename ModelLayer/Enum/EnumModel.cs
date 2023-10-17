using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_Layer.Enum
{
    public class EnumModel
    {
        public enum PayMethod
        {
            Cash = 1,
            PayPal = 2,
            VNPay = 3
        }
        public enum Status
        {
            Active = 1,
            Unavailable,
        }
        public enum Role
        {
            Admin = 1,
            Customer,
        }
    }
}

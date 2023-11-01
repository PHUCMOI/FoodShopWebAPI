using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ModelRequest
{
    public class UpdateUser
    {
        public int userId { get; set; }
        public string UserName { get; set;}
        public string Role {  get; set;}
        public string Status { get; set;}
        public string PhoneNumber { get; set;}
    }
}

using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.ServiceInterfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserRequest user);
        bool IsTokenValid(string key, string issuer, string token);
    }

}

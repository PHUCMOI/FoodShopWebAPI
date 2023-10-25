using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceInterfaces
{
    public interface ICartService
    {
        Task<bool> CreateProductCart(CartRequest cart);
        Task<bool> UpdateProductCart(CartRequest cart);
        Task<bool> DeleteItem(CartRequest cart);
        Task<bool> ClearCart(int userId);
        Task<CartResponse> GetCarts(int userId);
    }
}

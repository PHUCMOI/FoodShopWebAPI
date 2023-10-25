using Fooding_Shop.Models;
using ModelLayer.ModelRequest;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface ICartDAO
    {
        Task<bool> CreateProductCart(Cart cart);
        Task<bool> UpdateProductCart(Cart cart);
        Task<bool> DeleteItem(Cart cart);
        Task<List<CartRequest>> GetCartList(int userId);
        Task<bool> ClearCart(int userId);

    }
}

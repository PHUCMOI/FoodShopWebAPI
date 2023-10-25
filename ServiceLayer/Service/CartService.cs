using DataAccessLayer.DataAccess;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using Models_Layer.ModelRequest;
using ServiceLayer.ServiceInterfaces;
using Services_Layer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public class CartService : ICartService
    {
        private readonly ICartDAO cartDAO;
        private readonly IProductDAO productDAO;
        private readonly IAutoMapperService autoMapperService;
        public CartService(ICartDAO cartDAO, IProductDAO productDAO, IAutoMapperService autoMapperService) 
        {
            this.cartDAO = cartDAO;
            this.productDAO = productDAO;
            this.autoMapperService = autoMapperService;
        }

        public async Task<bool> ClearCart(int userId)
        {
            var result = await cartDAO.ClearCart(userId);
            if (result)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateProductCart(CartRequest cartRequest)
        {
            try
            {
                if (cartRequest != null)
                {
                    var cart = new Cart()
                    {
                        UserId = cartRequest.UserId,
                        UserName = cartRequest.UserName,
                        ProductID = cartRequest.ProductID,
                        Quantity = cartRequest.Quantity,
                        CreateBy = cartRequest.UserId,
                        CreateDate = DateTime.Now,
                        UpdateBy = cartRequest.UserId,
                        UpdateDate = DateTime.Now,
                        IsDeleted = false
                    };

                    var result = await cartDAO.CreateProductCart(cart);
                    if (result)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    throw new Exception("Order is null");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteItem(CartRequest cartRequest)
        {
            var cart = new Cart()
            {
                UserId = cartRequest.UserId,
                UserName = cartRequest.UserName,
                ProductID = cartRequest.ProductID,
            };

            var result = await cartDAO.DeleteItem(cart);
            if (result)
            {
                return true;
            }
            return false;
        }

        public async Task<CartResponse> GetCarts(int userId)
        {
            var cart = await cartDAO.GetCartList(userId);
            var productList = new List<ProductRequest>();
            foreach(var cartItem in cart)
            {
                var product = await productDAO.GetProduct(cartItem.ProductID);
                var p = autoMapperService.Map<Product, ProductRequest>(product);
                productList.Add(p);
            }

            var cartResponse = new CartResponse()
            {
                cart = cart,
                product = productList,
            };
            return cartResponse;
        }

        public async Task<bool> UpdateProductCart(CartRequest cartRequest)
        {
            var cart = new Cart()
            {
                UserId = cartRequest.UserId,
                UserName = cartRequest.UserName,
                ProductID = cartRequest.ProductID,
                Quantity = cartRequest.Quantity,
                UpdateBy = cartRequest.UserId,
                UpdateDate = DateTime.Now,
                IsDeleted = false
            };

            var result = await cartDAO.UpdateProductCart(cart);
            if (result)
            {
                return true;
            }
            return false;
        }
    }
}

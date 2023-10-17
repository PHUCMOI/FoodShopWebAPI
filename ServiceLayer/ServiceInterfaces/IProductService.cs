using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.ServiceInterfaces
{
    public interface IProductService
    {
        Task<List<ProductRequest>> SearchProducts(string value, decimal? minPrice, decimal? maxPrice);
        Task<List<ProductRequest>> GetProductList();
        Task<ProductModelView> GetProductByID(int id);
        Task<bool> UpdateAsync(ProductModelView productModelView, int userId);
        Task<bool> Delete(int id);
        Task<bool> Create(ProductModelView product, int userID);
    }
}

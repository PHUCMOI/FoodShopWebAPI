using Fooding_Shop.Models;
using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IProductDAO
    {
        Task<List<ProductRequest>> SearchProducts(string value, decimal? minPrice, decimal? maxPrice);
        Task<List<ProductRequest>> GetListProduct();
        Task<Product> GetProduct(int id);
        Task<bool> UpdateAsync(Product product, int userID);
        Task<bool> Create(Product product);
        bool DeleteAsync(int id);
        Task<List<ProductRequest>> GetProductByCategoryName(string categoryName);
    }
}

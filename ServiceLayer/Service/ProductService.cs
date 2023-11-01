using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.FileProviders;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using Services_Layer.ServiceInterfaces;

namespace Services_Layer.Service
{
    public class ProductService : IProductService
    {
        private readonly PathString requestPath;
        private readonly ICategoryDAO categoryDAO;
        private readonly IProductDAO productDAO;
        private readonly IAutoMapperService autoMapperService;
        public ProductService(IProductDAO productDAO, IAutoMapperService autoMapperService, ICategoryDAO categoryDAO)
        {
            this.productDAO = productDAO;
            this.autoMapperService = autoMapperService;
            this.categoryDAO = categoryDAO;
        }

        public async Task<bool> Create(ProductRequest productRequest, int userID)
        {
            string imgUrl = "https://localhost:44352/Image/" + productRequest.ImgUrl;
            try
            {
                Product product = new Product()
                {
                    ProductId = productRequest.ProductId,
                    ProductName = productRequest.ProductName,
                    ProductCategory = productRequest.ProductCategory,
                    Description = productRequest.Description,
                    Price = productRequest.Price,
                    ImgUrl = imgUrl,
                    CreateBy = userID,
                    CreateDate = DateTime.Now,
                    UpdateBy = userID,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false
                };

                var result = await productDAO.Create(product);
                if (result)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<bool> Delete(int id)
        {
            try
            {
                if(id > 0)
                {
                    bool flag = productDAO.DeleteAsync(id);
                    if (flag)
                    {
                        return Task.FromResult(true);
                    }
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductRequest>> GetProductByCategoryName(string categoryName)
        {
            var productList = await productDAO.GetProductByCategoryName(categoryName);
            return productList;
        }

        public async Task<ProductRequest> GetProductByID(int id)
        {
            var product = await productDAO.GetProduct(id);
            var productDetail = new ProductRequest()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductCategory = product.ProductCategory,
                Price = product.Price,
                Description = product.Description,
                ImgUrl = product.ImgUrl,
            };
            return productDetail;
        }

        public async Task<List<ProductRequest>> GetProductList()
        {
            var productList = await productDAO.GetListProduct();
            return productList; 
        }

        public async Task<List<ProductRequest>> SearchProducts(string value, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                var product = await productDAO.SearchProducts(value, minPrice, maxPrice);
                return product;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateAsync(ProductRequest productRequest, int userId)
        {
            string imgUrl = "https://localhost:44352/Image/" + productRequest.ImgUrl;
            if (productRequest != null)
            {
                var product = new Product()
                {
                    ProductId = productRequest.ProductId,
                    ProductName = productRequest.ProductName,
                    ProductCategory = productRequest.ProductCategory,
                    Price = productRequest.Price,
                    ImgUrl = imgUrl,
                    Description = productRequest.Description,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false
                };
                var reuslt = await productDAO.UpdateAsync(product, userId);
                if (reuslt)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}

using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Models_Layer.ModelRequest;
using Models_Layer.ModelResponse;
using Services_Layer.ServiceInterfaces;

namespace Services_Layer.Service
{
    public class ProductService : IProductService
    {
        private readonly ICategoryDAO categoryDAO;
        private readonly IProductDAO productDAO;
        private readonly IAutoMapperService autoMapperService;
        public ProductService(IProductDAO productDAO, IAutoMapperService autoMapperService, ICategoryDAO categoryDAO)
        {
            this.productDAO = productDAO;
            this.autoMapperService = autoMapperService;
            this.categoryDAO = categoryDAO;
        }

        public async Task<bool> Create(ProductModelView productModelView, int userID)
        {
            try
            {
                Product product = new Product()
                {
                    ProductId = productModelView.ProductRequest.ProductId,
                    ProductName = productModelView.ProductRequest.ProductName,
                    ProductCategory = productModelView.ProductRequest.ProductCategory,
                    Description = productModelView.ProductRequest.Description,
                    Price = productModelView.ProductRequest.Price,
                    ImgUrl = productModelView.ProductRequest.ImgUrl,
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

        public async Task<ProductModelView> GetProductByID(int id)
        {
            var productDetail = await productDAO.GetProduct(id);
            var productCategory = await categoryDAO.GetListCategory();
            var productViewModel = new ProductModelView()
            {
                ProductRequest = productDetail,
                CategoryRequest = productCategory
            };
            return productViewModel;
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

        public async Task<bool> UpdateAsync(ProductModelView productModelView, int userId)
        {
            if (productModelView != null)
            {
                var product = new Product()
                {
                    ProductId = productModelView.ProductRequest.ProductId,
                    ProductName = productModelView.ProductRequest.ProductName,
                    ProductCategory = productModelView.ProductRequest.ProductCategory,
                    Price = productModelView.ProductRequest.Price,
                    ImgUrl = productModelView.ProductRequest.ImgUrl,
                    Description = productModelView.ProductRequest.Description,
                    CreateBy = productModelView.ProductRequest.CreateBy,
                    CreateDate = productModelView.ProductRequest.CreateDate,
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

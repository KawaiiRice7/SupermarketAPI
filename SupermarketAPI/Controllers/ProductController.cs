using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SupermarketAPI.CoreDomain.Models;
using SupermarketAPI.CoreDomain.Services;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace SupermarketAPI.Controllers
{
    [Route("/api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private IMemoryCache _memoryCache;
        private string prefix = "product_";

        public ProductController(IMemoryCache memoryCache)
        {
            _productRepository = Common.ObjectFactory.GetInstance<IProductRepository>();
            _categoryRepository = Common.ObjectFactory.GetInstance<ICategoryRepository>();
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts(string name, string id)
        {
            List<Product> products = null;
            if (_memoryCache.TryGetValue(prefix, out products))
            {
                return products;
            }
            else
            {
                // if name and id are empty, return all products
                products = await _productRepository.GetProductsAsync(name, id);
                _memoryCache.Set($"{prefix}{name}", products, DateTime.Now.AddMinutes(1));
                return products;
            }

        }

        [HttpPost()]
        [Route("/api/[controller]/add/")]
        public async Task CreateProduct(string name, double cost, string newCategory)
        {
            if (!string.IsNullOrEmpty(name) && name.Length < 100 && cost >= 0)
            {
                var product = await _productRepository.GetProductsAsync(name: name);
                var categories = await _categoryRepository.GetCategoriesAsync();

                // If category doesn't exist, add it
                var category = categories.Where(a => a.Name == newCategory).FirstOrDefault();
                if (category == null)
                {
                    await _categoryRepository.CreateCategoryAsync(newCategory);
                    categories = await _categoryRepository.GetCategoriesAsync();
                    category = categories.Where(a => a.Name == newCategory).FirstOrDefault();
                }

                // If product doesn't exist, add it
                if (product.Count == 0)
                {
                    var result = await _productRepository.AddProductAsync(name, cost, category.Id);
                    _memoryCache.Remove(prefix);

                    if (!result)
                    {
                        throw new HttpRequestException($"Failed to add product!");
                    }
                }
                else
                {
                    throw new HttpRequestException($"Product already exists!");
                }
            }
        }

        [HttpPost()]
        [Route("/api/[controller]/update/")]

        public async Task UpdateProduct(string oldName, string newName, double cost)
        {
            if (!string.IsNullOrEmpty(oldName) && ((!string.IsNullOrEmpty(newName) && newName.Length < 100 )|| cost >= 0))
            {
                var product = await _productRepository.GetProductsAsync(name: newName);

                if (product != null)
                {
                    var result = await _productRepository.UpdateProductAsync(oldName, newName, cost);

                    if (!result)
                    {
                        throw new HttpRequestException($"Failed to update product!");
                    }
                    else
                        _memoryCache.Remove(prefix);
                }
                else
                {
                    throw new HttpRequestException($"Invalid request - product doesn't exist!");
                }
            }
            else
            {
                throw new HttpRequestException($"Invalid request - please check the new name/cost!");
            }
        }

        [HttpDelete]
        public async Task DeleteProduct(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length < 100)
            {
                var product = await _productRepository.GetProductsAsync(name: name);

                if (product != null)
                {
                    var result = await _productRepository.DeleteProductAsync(name);

                    if (!result)
                    {
                        throw new HttpRequestException($"Failed to delete product!");
                    }
                }
                else
                {
                    throw new HttpRequestException($"Invalid request - product doesn't exist!");
                }
            }
        }
    }
}

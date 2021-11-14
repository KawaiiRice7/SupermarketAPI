using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SupermarketAPI.CoreDomain.Models;
using SupermarketAPI.CoreDomain.Services;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SupermarketAPI.Controllers
{
    [Route("/api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryService;
        private readonly IMemoryCache _memoryCache;
        private string prefix = "category_";

        public CategoriesController(IMemoryCache memoryCache)
        {
            _categoryService = Common.ObjectFactory.GetInstance<ICategoryRepository>();
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            List<Category> categories = null;

            if (_memoryCache.TryGetValue(prefix, out categories))
            {
                return categories;
            }
            else
            {
                categories = await _categoryService.GetCategoriesAsync();
                _memoryCache.Set("categories", categories, DateTime.Now.AddMinutes(1));
                return categories;
            }
        }

        [HttpPost]
        public async Task CreateCategory(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length < 100)
            {
                await _categoryService.CreateCategoryAsync(name);
                _memoryCache.Remove(prefix);
            }
        }

        [HttpDelete]
        public async Task DeleteCategory(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length < 100)
            {
                await _categoryService.DeleteCategoryAsync(name);
                _memoryCache.Remove(prefix);
            }
        }
    }
}

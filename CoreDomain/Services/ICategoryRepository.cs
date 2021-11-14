using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SupermarketAPI.CoreDomain.Models;

namespace SupermarketAPI.CoreDomain.Services
{
    public interface ICategoryRepository
    {
        Task<bool> CreateCategoryAsync(string name);
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> DeleteCategoryAsync(string name);
    }
}

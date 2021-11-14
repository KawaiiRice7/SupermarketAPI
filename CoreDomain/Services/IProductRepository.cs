using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SupermarketAPI.CoreDomain.Models;

namespace SupermarketAPI.CoreDomain.Services
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(string name = null, string id = null);
        Task<bool> AddProductAsync(string name, double cost, string categoryId);
        Task<bool> UpdateProductAsync(string oldName, string newName, double cost);
        Task<bool> DeleteProductAsync(string name);

    }
}

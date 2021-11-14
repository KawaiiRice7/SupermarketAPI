using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using SupermarketAPI.CoreDomain.Models;
using SupermarketAPI.CoreDomain.Services;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;

namespace SupermarketAPI.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private string _connectionString { get; set; }
        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var query = "SELECT * FROM [dbo].[Category]";
            var productQuery = "SELECT * FROM [dbo].[Product] WHERE CategoryId = @CategoryId";
            using (var connection = new SqlConnection(_connectionString))
            {
                var categories = await connection.QueryAsync<Category>(query);

                foreach (var category in categories)
                {
                    List<Product> products = (List<Product>)await connection.QueryAsync<Product>(productQuery, new { CategoryId = category.Id });
                    category.Products = products;
                }

                return categories.ToList();
            }
        }

        public async Task<bool> CreateCategoryAsync(string name)
        {
            var query = "IF NOT EXISTS(SELECT * FROM [dbo].[Category] WHERE Name = @Name) BEGIN INSERT INTO [dbo].[Category] VALUES (@Id, @Name) END";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Execute(query, new { Id = Guid.NewGuid().ToString(), name }) == 1 ? true : false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(string name)
        {
            var query = "IF EXISTS(SELECT * FROM [dbo].[Category] WHERE Name = @Name) BEGIN DELETE FROM [dbo].[Category] WHERE Name = @Name END";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Execute(query, new { Name = name }) == 1 ? true : false;
            }
        }
    }
}

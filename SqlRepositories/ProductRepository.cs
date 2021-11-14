using System;
using System.Collections.Generic;
using System.Text;
using SupermarketAPI.CoreDomain.Services;
using SupermarketAPI.CoreDomain.Models;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Linq;

namespace SupermarketAPI.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private string _connectionString { get; set; }

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Product>> GetProductsAsync(string name = null, string id = null)
        {
            var query = $"SELECT * FROM [dbo].[Product] { (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(id) ? "WHERE " : string.Empty)}";

            query = $"{query} {(!string.IsNullOrEmpty(name) ? "name = @Name" : string.Empty)} {(!string.IsNullOrEmpty(id) ? "id = @Id" : string.Empty)}";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<Product>(query, new {Id = id, Name = name });

                return result.ToList();
            }
        }
        public async Task<bool> UpdateProductAsync(string oldName, string newName, double cost)
        {
            var query = $"IF EXISTS(SELECT * FROM [dbo].[Product] WHERE name = @OldName )" +
                $"BEGIN UPDATE [dbo].[Product] SET ";
            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(newName))
            {
                conditions.Add("name = @Name");
            }

            if (cost != null)
            {
                conditions.Add("cost = @Cost");
            }

            if (conditions.Count > 0)
            {
                query += string.Join(" , ", conditions);
                query = $"{query} WHERE Name = @OldName END";
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.ExecuteAsync(query, new { Name = newName, Cost = cost, OldName = oldName });

                return result > 0;
            }
        }
        public async Task<bool> AddProductAsync(string name, double cost, string categoryId)
        {
            var query = $"IF NOT EXISTS(SELECT * FROM [dbo].[Product] WHERE Name = @Name) " +
                $"BEGIN INSERT INTO [dbo].[Product] VALUES (@ProductId, @Name, @Cost, @CategoryId) END";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.ExecuteAsync(query, new { ProductId = Guid.NewGuid(), Name = name, Cost = cost, CategoryId = categoryId});

                return result == 1;
            }
        }

        public async Task<bool> DeleteProductAsync(string name)
        {
            var query = $"IF EXISTS(SELECT * FROM [dbo].[Product] WHERE Name = @Name) BEGIN DELETE FROM [dbo].[Product] WHERE Name = @Name END";

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.ExecuteAsync(query, new { Name = name });

                return result > 0;
            }
        }
    }
}

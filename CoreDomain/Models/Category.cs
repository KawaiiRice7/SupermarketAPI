using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketAPI.CoreDomain.Models
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public Category() { }
        public Category(string id, string name, List<Product> products = null)
        {
            Id = id;
            Name = name;
            Products = products;
        }
    }
}

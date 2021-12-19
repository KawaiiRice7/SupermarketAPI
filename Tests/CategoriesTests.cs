using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using SupermarketAPI.CoreDomain.Services;
using SupermarketAPI.DAL.Repositories;
using SupermarketAPI.CoreDomain.Models;
using System.Configuration;
using Moq;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;

namespace Tests
{
    [TestClass]
    public class CategoriesTests
    {
        //private static HttpClient client = new HttpClient();

        [TestInitialize]
        public void Setup()
        {
            string connectionString = ConfigurationManager.AppSettings["DbConnectionString"];

            Common.ObjectFactory.RegisterInstance<ICategoryRepository>(new CategoryRepository(connectionString));
            Common.ObjectFactory.RegisterInstance<IProductRepository>(new ProductRepository(connectionString));
        }

        [TestMethod]
        public async Task Test_CreateCategory_NotExists()
        {
            Assert.IsTrue(1 == 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Virtual_Store_WebApp.Models;
using Xunit;
using Moq;
using System.Threading.Tasks;
using Virtual_Store_WebApp.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Virtual_Store_WebApp.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task IndexActionModelIsComplete()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(p => p.ProductsAsync()).Returns(Task.FromResult(
                (IEnumerable<Product>)new[]
                {
                    new Product()
                    {
                        Id=1,
                        CategoryId=1,
                        Description="dfkjkadj",
                        Image ="981f7436-1700-4ccc-a4f5-05692b709aeesingle-product.jpg",
                        Price=12,
                        ProductName="Big Pants",
                        Specifications="kjdakfjka",
                        Stock=32,
                    }
                }
                ));
            var mock1 = new Mock<IProduct_CategoryRepository>();
            mock1.Setup(pc => pc.Product_CategoriesAsync()).Returns(Task.FromResult((IEnumerable<Product_Category>)new[]
                {
                    new Product_Category()
                    {
                        Id = 1,
                        CategoryName="The Biggest Pants",
                        Description="dfkjkadj",
                        Image ="981f7436-1700-4ccc-a4f5-05692b709aeesingle-product.jpg",
                    }
                }));
                
            var controller = new HomeController(new AppIdentityDbContext(new DbContextOptions<AppIdentityDbContext>(), null), mock1.Object, mock.Object);

            //Act
            if ((controller.Index().Result as ViewResult)?.ViewData.Model!=null)
            {
                var model = (ValueTuple<IEnumerable<Product_Category>, IEnumerable<Product>>)(controller.Index().Result as ViewResult)?.ViewData.Model;

                //Asert
                Assert.Equal(await controller.pcrepository.Product_CategoriesAsync(), model.Item1);
                Assert.Equal(await controller.prepository.ProductsAsync(), model.Item2);
                //Assert.Equal(new List<Product>(), model.Item2);  //for failing test
            }

            

            
        }
    }
}

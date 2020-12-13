﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject.Web.Controllers;
using UnitTestProject.Web.Models;
using UnitTestProject.Web.Repository;
using Xunit;

namespace UdemyRealWorldUnitTest.Test
{

    public class ProductControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;

        private readonly ProductsController _productsController;

        private List<Product> products;
        public ProductControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();

            _productsController = new ProductsController(_mockRepo.Object);

            products = new List<Product>()
            {
                new Product()
                {
                    Color="TestColor",
                     Name ="TestName",
                      Price =4,
                       Stock =6,
                       Id =1
                },
                new Product()
                {
                    Color="TestColor1",
                     Name ="TestName1",
                      Price =5,
                       Stock =5,
                       Id =2
                },
                  new Product()
                {
                    Color="TestColor3",
                     Name ="TestName4",
                      Price =33,
                       Stock =2,
                       Id =3
                }
            };
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _productsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        //herhangi bir parametre almazsa Fact eklenir.
        [Fact]
        public async Task<int> Index_ActionExecutes_ReturnProductList()
        {

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products);
            var result = await _productsController.Index();
            var lists = Assert.IsType<ViewResult>(result).Model as List<Repository<Product>>;
            if (lists != null)
            {
                return 1;
            }

            return 0;

            //_mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products);
            //Assert.IsAssignableFrom<IEnumerable<Product>>(await _productsController.Index());
        }


        [Fact]
        public async void Edit_IdIsNull_ReturnRedirectTıAction()
        {
            var result = await _productsController.Details(null);
            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);



        }
        [Fact]
        public async void Edit_IdNotNul_And_ProductNull_NotFound()
        {
            var result = await _productsController.Details(22);
            var redirect = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, redirect.StatusCode);
        }
    }
}

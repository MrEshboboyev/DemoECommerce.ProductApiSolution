using eCommerce.SharedLibrary.Responses;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Presentation.Controllers;
using System.ComponentModel.DataAnnotations;

namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProduct productInterface;
        private readonly ProductsController productsController;

        public ProductControllerTest()
        {
            // Set up dependencies
            productInterface = A.Fake<IProduct>();

            // Set up System Under Test - SUT
            productsController = new ProductsController(productInterface); 
        }

        // GET ALL PRODUCTS
        [Fact]
        public async Task GetProduct_WhenProductExists_ReturnOkResponseWithProducts()
        {
            // Arrange
            var products = new List<Product>()
            {
                new() {Id = 1, Name = "Product 1", Price = 100.7m, Quantity = 10},
                new() {Id = 2, Name = "Product 2", Price = 120.7m, Quantity = 110}
            };

            // set up fake response for GetAllAsync method
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            // Act
            var result = await productsController.GetProducts();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedProducts = okResult.Value as IEnumerable<ProductDTO>;
            returnedProducts.Should().NotBeNull();
            returnedProducts.Should().HaveCount(2);
            returnedProducts!.First().Id.Should().Be(1);
            returnedProducts!.Last().Id.Should().Be(2);
        }

        [Fact]
        public async Task GetProduct_WhenNoProductsExists_ReturnNotFoundResponse()
        {
            // Arrange
            var products = new List<Product>();

            // set up fake response for GetAllAsync method
            A.CallTo(() => productInterface.GetAllAsync()).Returns(products);

            // Act
            var result = await productsController.GetProducts();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var message = notFoundResult.Value as string;
            message.Should().Be("No products detected in the database");
        }

        // CREATE PRODUCT
        [Fact]
        public async Task CreateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 18.99m);
            productsController.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await productsController.CreateProduct(productDTO);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
        
        [Fact]
        public async Task CreateProduct_WhenCreateIsSuccessful_ReturnOkResponse()
        {
            // Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 18.99m);
            var response = new Response(true, "Created");

            // Act
            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.CreateProduct(productDTO);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult!.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Created");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task CreateProduct_WhenCreateFails_ReturnBadRequestResponse()
        {
            // Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 18.99m);
            var response = new Response(false, "Failed");

            // Act
            A.CallTo(() => productInterface.CreateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.CreateProduct(productDTO);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult!.Should().NotBeNull();
            responseResult!.Message.Should().Be("Failed");
            responseResult!.Flag.Should().BeFalse();
        }


        // UPDATE PRODUCT
        [Fact]
        public async Task UpdateProduct_WhenModelStateIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 18.99m);
            productsController.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await productsController.UpdateProduct(productDTO);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdateProduct_WhenUpdateIsSuccessful_ReturnOkResponse()
        {
            // Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 18.99m);
            var response = new Response(true, "Updated");

            // Act
            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult!.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

            var responseResult = okResult.Value as Response;
            responseResult!.Message.Should().Be("Updated");
            responseResult!.Flag.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateProduct_WhenUpdateFails_ReturnBadRequestResponse()
        {
            // Arrange
            var productDTO = new ProductDTO(1, "Product 1", 34, 18.99m);
            var response = new Response(false, "Failed");

            // Act
            A.CallTo(() => productInterface.UpdateAsync(A<Product>.Ignored)).Returns(response);
            var result = await productsController.UpdateProduct(productDTO);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

            var responseResult = badRequestResult.Value as Response;
            responseResult!.Should().NotBeNull();
            responseResult!.Message.Should().Be("Failed");
            responseResult!.Flag.Should().BeFalse();
        }
    }
}

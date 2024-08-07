﻿using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using ProductApi.Presentation.Controllers;

namespace UnitTest.ProductApi.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly ProductDbContext productDbContext;
        private readonly ProductRepository productRepository;

        public ProductRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductDb").Options;

            productDbContext = new ProductDbContext(options);

            productRepository = new ProductRepository(productDbContext);
        }

        // CREATE PRODUCT
        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExist_ReturnErrorResponse()
        {
            // arrange 
            var existingProduct = new Product { Name = "Existing product"};
            productDbContext.Products.Add(existingProduct);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.CreateAsync(existingProduct);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Existing product already added");
        }

        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_AddProductAndReturnsSuccessResponse()
        {
            // arrange 
            var product = new Product { Name = "Product"};

            // Act
            var result = await productRepository.CreateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product added to database successfully!");
        }

        // DELETE PRODUCT
        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsNotFoundResponse()
        {
            // arrange 
            var product = new Product { Id = 1,  Name = "Product", Quantity = 32, Price = 78.12m };

            // Act
            var result = await productRepository.DeleteAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product not found!");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsFound_ReturnsSuccessResponse()
        {
            // arrange 
            var product = new Product { Name = "Product" };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.DeleteAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product is deleted successfully");
        }

        // GET PRODUCT BY ID
        [Fact]
        public async Task FindByIdAsync_WhenProductIsNotFound_ReturnsNull()
        {
            // arrange 
            var productId = 1;

            // Act
            var result = await productRepository.FindByIdAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductIsFound_ReturnsProduct()
        {
            // arrange 
            var product = new Product { Id = 1, Name = "Product", Price = 43.22m, Quantity = 10 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.FindByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Product");
            //result.Should().Be(product);
        }
    }
}

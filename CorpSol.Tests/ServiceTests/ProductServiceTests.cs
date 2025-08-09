using CorpSol.Models.Domain;
using CorpSol.Services;
using CorpSol.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CorpSol.Tests.ServiceTests
{
    public class ProductServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IAuditService> _mockAuditService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            // Setup mocks
            _mockAuditService = new Mock<IAuditService>();
            _mockConfiguration = new Mock<IConfiguration>();

            // Setup VAT configuration (20% VAT = 0.2)
            _mockConfiguration.Setup(x => x["VatValue"]).Returns("0.2");

            _productService = new ProductService(_context, _mockAuditService.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldCalculateCorrectVAT()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Title = "Product1", Quantity = 2, Price = 10.00m },
                new Product { Id = 2, Title = "Product2", Quantity = 1, Price = 50.00m }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductsAsync();
            var productList = result.ToList();

            // Assert
            Assert.Equal(2, productList.Count);

            // Product 1: Quantity=2, Price=10, VAT=20% => 2 * 10 * 1.2 = 24
            Assert.Equal("Product1", productList[0].ItemName);
            Assert.Equal(2, productList[0].Quantity);
            Assert.Equal(10.00m, productList[0].Price);
            Assert.Equal(24.00m, productList[0].TotalPriceWithVat);

            // Product 2: Quantity=1, Price=50, VAT=20% => 1 * 50 * 1.2 = 60
            Assert.Equal("Product2", productList[1].ItemName);
            Assert.Equal(1, productList[1].Quantity);
            Assert.Equal(50.00m, productList[1].Price);
            Assert.Equal(60.00m, productList[1].TotalPriceWithVat);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldCalculateCorrectVAT()
        {
            // Arrange
            var product = new Product { Id = 1, Title = "Test Product", Quantity = 3, Price = 25.00m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.ItemName);
            Assert.Equal(3, result.Quantity);
            Assert.Equal(25.00m, result.Price);
            // Expected: 3 * 25 * 1.2 = 90
            Assert.Equal(90.00m, result.TotalPriceWithVat);
        }

        [Theory]
        [InlineData(0.0, 100.00)] // 0% VAT: 2 * 50 * 1.0 = 100
        [InlineData(0.1, 110.00)] // 10% VAT: 2 * 50 * 1.1 = 110
        [InlineData(0.2, 120.00)] // 20% VAT: 2 * 50 * 1.2 = 120
        [InlineData(0.25, 125.00)] // 25% VAT: 2 * 50 * 1.25 = 125
        public async Task GetProductByIdAsync_ShouldCalculateVATWithDifferentRates(decimal vatRate, decimal expectedTotal)
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["VatValue"]).Returns(vatRate.ToString());
            var service = new ProductService(_context, _mockAuditService.Object, mockConfig.Object);

            var product = new Product { Id = 1, Title = "Test", Quantity = 2, Price = 50.00m };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await service.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTotal, result.TotalPriceWithVat);
        }

        [Fact]
        public async Task GetProductByIdAsync_NonExistentProduct_ShouldReturnNull()
        {
            // Act
            var result = await _productService.GetProductByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductsAsync_EmptyDatabase_ShouldReturnEmptyList()
        {
            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Constructor_ShouldParseVatValueFromConfiguration()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["VatValue"]).Returns("0.15");

            // Act
            var service = new ProductService(_context, _mockAuditService.Object, mockConfig.Object);

            var product = new Product { Id = 1, Title = "Test", Quantity = 1, Price = 100m };
            _context.Products.Add(product);
            _context.SaveChanges();

            var result = service.GetProductByIdAsync(1).Result;

            // Assert
            // Expected: 1 * 100 * 1.15 = 115
            Assert.Equal(115.00m, result.TotalPriceWithVat);
        }

        [Fact]
        public void Constructor_InvalidVatValue_ShouldThrowException()
        {
            // Arrange
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x["VatValue"]).Returns("invalid");

            // Act & Assert
            Assert.Throws<FormatException>(() =>
                new ProductService(_context, _mockAuditService.Object, mockConfig.Object));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
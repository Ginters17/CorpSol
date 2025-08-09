using CorpSol.Models.Domain;
using CorpSol.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CorpSol.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;
        private readonly decimal _vat;

        public ProductService(AppDbContext context, IAuditService auditService, IConfiguration config)
        {
            _context = context;
            _auditService = auditService;
            _vat = decimal.Parse(config["VatValue"]);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return products.Select(p => new ProductDto
            {
                ItemName = p.Title,
                Quantity = p.Quantity,
                Price = p.Price,
                TotalPriceWithVat = p.Quantity * p.Price * (1 + _vat)
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;

            return new ProductDto
            {
                ItemName = product.Title,
                Quantity = product.Quantity,
                Price = product.Price,
                TotalPriceWithVat = product.Quantity * product.Price * (1 + _vat)
            };
        }

        public async Task<Product> CreateProductAsync(Product product, int userId)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await _auditService.LogProductChangeAsync(
                product.Id,
                userId,
                "Create",
                $"Created product '{product.Title}' with Quantity={product.Quantity} and Price={product.Price}"
            );

            return product;
        }

        public async Task<bool> UpdateProductAsync(Product updatedProduct, int userId)
        {
            var product = await _context.Products.FindAsync(updatedProduct.Id);
            if (product == null)
                return false;


            string changes = $"Title: '{product.Title}' -> '{updatedProduct.Title}', " +
                 $"Quantity: {product.Quantity} -> {updatedProduct.Quantity}, " +
                 $"Price: {product.Price} -> {updatedProduct.Price}";

            product.Title = updatedProduct.Title;
            product.Quantity = updatedProduct.Quantity;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync();

            // Create audit record
            await _auditService.LogProductChangeAsync(
                product.Id,
                userId,
                "Update",
                changes
            );

            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId, int userId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            await _auditService.LogProductChangeAsync(
                product.Id,
                userId,
                "Delete",
                $"Deleted product '{product.Title}' with Quantity={product.Quantity} and Price={product.Price}"
            );

            return true;
        }
    }
}

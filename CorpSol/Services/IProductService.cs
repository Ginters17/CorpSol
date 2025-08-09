using CorpSol.Models.Domain;
using CorpSol.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CorpSol.Services
{
    public interface IProductService
    {
        Task<bool> DeleteProductAsync(int productId, int userId);
        Task<bool> UpdateProductAsync(Product updatedProduct, int userId);
        Task<Product> CreateProductAsync(Product updatedProduct, int userId);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}

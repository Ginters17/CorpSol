using CorpSol.Models.Domain;

namespace CorpSol.Services
{
    public interface IAuditService
    {
        Task LogProductChangeAsync(int productId, int userId, string changeType, string changes);
        Task<IEnumerable<ProductAudit>> GetAuditLogsAsync(DateTime? from, DateTime? to);
    }
}

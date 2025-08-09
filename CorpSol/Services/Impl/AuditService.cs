using CorpSol.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CorpSol.Services.Impl
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogProductChangeAsync(int productId, int userId, string changeType, string changes)
        {
            var audit = new ProductAudit
            {
                ProductId = productId,
                ChangedByUserId = userId,
                ChangedAt = DateTime.UtcNow,
                ChangeType = changeType,
                Changes = changes
            };

            _context.ProductAudits.Add(audit);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductAudit>> GetAuditLogsAsync(DateTime? from, DateTime? to)
        {
            var query = _context.ProductAudits.AsQueryable();

            if (from.HasValue)
                query = query.Where(a => a.ChangedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.ChangedAt <= to.Value);

            var audits = await query
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync();

            return audits;
        }

    }

}

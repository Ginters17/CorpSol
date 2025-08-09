using CorpSol.Services;
using Microsoft.AspNetCore.Mvc;

namespace CorpSol.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var logs = await _auditService.GetAuditLogsAsync(from, to);
            return Ok(logs);
        }
    }
}

using GlassGo.API.Analytics.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlassGo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public AnalyticsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("reports")]
        [Authorize(Policy = "AdminOnly")]
        [SwaggerOperation(
            Summary = "Get all reports (Admin only)",
            Description = "Get all reports",
            OperationId = "GetAllReports")]
        [SwaggerResponse(200, "The reports were retrieved", typeof(IEnumerable<Analytics.Domain.Entities.Report>))]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportService.GetReportsAsync();
            return Ok(reports);
        }
    }
}
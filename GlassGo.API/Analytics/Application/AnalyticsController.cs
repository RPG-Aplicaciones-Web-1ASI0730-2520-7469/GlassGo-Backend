using GlassGo.API.Analytics.Domain.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportService.GetReportsAsync();
            return Ok(reports);
        }
    }
}
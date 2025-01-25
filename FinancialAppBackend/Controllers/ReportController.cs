using Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FinancialAppBackend.Servieces;

namespace FinancialAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService financialService)
        {
            _reportService = financialService;
        }

        [HttpGet("DailyReport")]
        public async Task<ActionResult<DailyReportViewModel>> GetDailyReport([FromQuery] DateTime date)
        {
            var report = await _reportService.GetDailyReportAsync(date);
            return Ok(report);
        }

        [HttpGet("PeriodReport")]
        public async Task<ActionResult<PeriodReportViewModel>> GetPeriodReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var report = await _reportService.GetPeriodReportAsync(startDate, endDate);
            return Ok(report);
        }
    }
}

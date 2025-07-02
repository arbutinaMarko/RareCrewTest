using Microsoft.AspNetCore.Mvc;
using CSharp.Services;
using CSharp.Helpers;

namespace CSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeEntriesController : Controller
    {
        private readonly TimeEntryService _service;

        public TimeEntriesController(TimeEntryService service)
        {
            _service = service;
        }

        [HttpGet("html")]
        public async Task<IActionResult> GetHtml()
        {
            var entries = await _service.GetTimeEntriesAsync();
            var html = HtmlGenerator.GenerateTable(entries);
            return Content(html, "text/html");
        }

        [HttpGet("chart")]
        public async Task<IActionResult> GetChart()
        {
            var entries = await _service.GetTimeEntriesAsync();
            var chartData = ChartGenerator.GeneratePieChart(entries);
            return File(chartData, "image/png");
        }
    }
}

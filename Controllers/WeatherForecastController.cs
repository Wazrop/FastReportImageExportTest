using FastReport;
using FastReport.Export.Html;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Mvc;

namespace FastReportImageExportTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            byte[] report = GenerateReport();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("DownloadPdf")]
        public IActionResult DownloadPdf()
        {
            byte[] report = GenerateReport();

            var stream = new MemoryStream(report);

            string contentType = "application/pdf";

            string fileName = "example.pdf";

            // Return the file
            return File(stream, contentType, fileName);
        }


        [HttpGet("DownloadHtml")]
        public IActionResult DownloadHtml()
        {
            byte[] report = GenerateReportHtml();

            var stream = new MemoryStream(report);

            string contentType = "text/html";

            string fileName = "example.html";

            // Return the file
            return File(stream, contentType, fileName);
        }

        private byte[] GenerateReport()
        {

            Report report = Report.FromFile(".\\Report\\ImageExportTest.frx");
            report.Prepare();
            PDFSimpleExport export = new();
            MemoryStream stream = new();
            report.Export(export, stream);
            stream.Position = 0;
            return stream.ToArray();
        }
        private byte[] GenerateReportHtml()
        {

            Report report = Report.FromFile(".\\Report\\ImageExportTest.frx");
            report.Prepare();
            HTMLExport export = new();
            export.EmbedPictures = true;
            MemoryStream stream = new();
            report.Export(export, stream);
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}
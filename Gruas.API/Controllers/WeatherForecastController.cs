using Gruas.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("GetInvoice")]
        public async Task<IActionResult> GetInvoice()
        {
            PdfFormater pdfFormater = new PdfFormater();
            //var response = await citasRepository.GetCitaById(id);
            //CreateCitaResponse citaResponse = response.result;
            /*
            if (!response.response)//
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            */
            var pdfBytes = pdfFormater.Formato_Invoice();

            return File(pdfBytes, "application/pdf", $"Invoice.pdf");
            
        }
    }
}
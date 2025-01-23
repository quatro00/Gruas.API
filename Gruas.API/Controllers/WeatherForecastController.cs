using Gruas.API.Helpers;
using Gruas.API.Models.Stripe;
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

        [HttpGet]
        [Route("TestPago")]
        public async Task<IActionResult> TestPago()
        {
            PaymentLinkRequest request = new PaymentLinkRequest() { Amount = 1000, Currency="mxn" };

            try
            {
                // Crear un producto y precio si no existe uno fijo
                var productService = new Stripe.ProductService();
                var priceService = new Stripe.PriceService();

                var product = await productService.CreateAsync(new Stripe.ProductCreateOptions
                {
                    Name = "Servicio Personalizado", // Producto genérico para servicios
                });

                var price = await priceService.CreateAsync(new Stripe.PriceCreateOptions
                {
                    UnitAmount = request.Amount, // Monto en centavos
                    Currency = request.Currency,
                    Product = product.Id,
                });

                // Crear el enlace de pago
                var paymentLinkService = new Stripe.PaymentLinkService();
                var paymentLink = await paymentLinkService.CreateAsync(new Stripe.PaymentLinkCreateOptions
                {
                    LineItems = new List<Stripe.PaymentLinkLineItemOptions>
            {
                new Stripe.PaymentLinkLineItemOptions
                {
                    Price = price.Id,
                    Quantity = 1,
                }
            },
                });

                return Ok(new { url = paymentLink.Url }); // URL lista para enviar al cliente
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            return Ok();
        }
    }
}
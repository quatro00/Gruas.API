using Gruas.API.Helpers;
using Gruas.API.Models.DTO.Pagos;
using Gruas.API.Repositories.Implementation;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IPagoRepository pagoRepository;
        public PagosController(IPagoRepository pagoRepository)
        {
            this.pagoRepository = pagoRepository;
        }
        [HttpGet]
        [Route("GetServiciosPorPagar/{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetServiciosPorPagar([FromRoute] Guid id)
        {
            var response = await pagoRepository.GetServiciosPorPagar(id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }


        [HttpPost]
        [Route("RegistrarPagoServicios")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistrarPagoServicios([FromBody] InsPagoServicios_Request model)
        {
            var response = await pagoRepository.RegistrarPagoServicios(model, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("GetPagosMensuales")]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> GetPagosMensuales()
        {
            int mes = DateTime.Now.Month;
            int anio = DateTime.Now.Year;

            var response = await pagoRepository.GetPagosMensuales(anio,mes, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

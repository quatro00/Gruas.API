using Gruas.API.Helpers;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Models.DTO.Servicio;
using Gruas.API.Repositories.Implementation;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {

        private readonly IServicioRepository servicioRepository;
        public ServicioController(IServicioRepository servicioRepository)
        {
            this.servicioRepository = servicioRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("GetSerrvicios")]
        public async Task<IActionResult> GetSerrvicios(int? estatusServicioId)
        {
            var response = await servicioRepository.GetServicios(estatusServicioId);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Authorize]
        [Route("RegistraServicio")]
        public async Task<IActionResult> RegistraServicio(RegistraServicio_Request request)
        {
            var response = await servicioRepository.InsServicio(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPost]
        [Authorize]
        [Route("SolicitarCotizaciones")]
        public async Task<IActionResult> SolicitarCotizaciones(SolicitarCotizacion_Request request)
        {
            var response = await servicioRepository.SolicitarCotizaciones(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPost]
        [Authorize]
        [Route("ColocarEnPropuesta")]
        public async Task<IActionResult> ColocarEnPropuesta(ColocarEnPropuesta_Request request)
        {
            var response = await servicioRepository.ColocarEnPropuesta(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPost]
        [Authorize]
        [Route("TerminarServicio")]
        public async Task<IActionResult> TerminarServicio(TerminarServicio_Request request)
        {
            var response = await servicioRepository.TerminarServicio(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPost]
        [Authorize]
        [Route("AsignarGrua")]
        public async Task<IActionResult> AsignarGrua(AsignarGrua_Request request)
        {
            var response = await servicioRepository.AsignarGrua(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPost]
        [Authorize]
        [Route("CancelarServicio")]
        public async Task<IActionResult> CancelarServicio(CancelarServicio_Request request)
        {
            var response = await servicioRepository.CancelarServicio(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpGet]
        [Authorize]
        [Route("GetServicio")]
        public async Task<IActionResult> GetServicio(Guid servicioId)
        {
            var response = await servicioRepository.GetServicio(servicioId);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

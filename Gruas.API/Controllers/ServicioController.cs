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
    }
}

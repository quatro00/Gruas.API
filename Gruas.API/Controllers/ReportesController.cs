using Gruas.API.Helpers;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Models.DTO.Reportes;
using Gruas.API.Repositories.Implementation;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesRepository reportesRepository;
        public ReportesController(IReportesRepository reportesRepository)
        {
            this.reportesRepository = reportesRepository;
        }
        [HttpPost]
        [Route("GetPagos")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetPagos([FromBody] GetPagos_Request model)
        {
            var response = await reportesRepository.GetPagos(model);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetServicios")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetServicios([FromBody] GetServicios_Request model)
        {
            var response = await reportesRepository.GetServicios(model);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetProveedores")]
        public async Task<IActionResult> GetProveedores([FromBody] GetProveedores_Request model)
        {
            var response = await reportesRepository.GetProveedores(model);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetGruas")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetGruas([FromBody] GetGruas_Request model)
        {
            var response = await reportesRepository.GetGruas(model);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetServiciosProveedorMensuales")]
        [Authorize(Roles = "Administrador,Colaborador")]
        public async Task<IActionResult> GetServiciosProveedorMensuales([FromBody] GetServiciosProveedorMensuales_Request model)
        {
            var response = await reportesRepository.GetServiciosProveedorMensuales(model, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetPagosProveedor")]
        [Authorize(Roles = "Administrador,Colaborador")]
        public async Task<IActionResult> GetPagosProveedor([FromBody] GetPagosProveedor_Request model)
        {
            var response = await reportesRepository.GetPagosProveedor(model, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetServiciosProveedor")]
        [Authorize(Roles = "Administrador,Colaborador")]
        public async Task<IActionResult> GetServiciosProveedor([FromBody] GetServiciosProveedor_Request model)
        {
            var response = await reportesRepository.GetServiciosProveedor(model, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

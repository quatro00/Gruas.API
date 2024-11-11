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
        [Authorize]
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
        [Authorize]
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
    }
}

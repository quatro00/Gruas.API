using Gruas.API.Helpers;
using Gruas.API.Models.DTO.Landscape;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandscapeController : ControllerBase
    {
        [HttpPost]
        [Route("SolicitaCotizacion")]
        public async Task<IActionResult> SolicitaCotizacion([FromBody] SolicitarCotizacionLandscape_Request model)
        {
            return Ok();
            /*
            var response = await gruaRepository.Create(model, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
            */
        }
    }
}

using Gruas.API.Repositories.Implementation;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogoRepository catalogoRepository;
        public CatalogoController(ICatalogoRepository catalogoRepository)
        {
            this.catalogoRepository = catalogoRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("GetTipoGrua")]
        public async Task<IActionResult> GetTipoGrua()
        {
            var response = await catalogoRepository.GetTipoGrua();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

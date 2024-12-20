using Gruas.API.Helpers;
using Gruas.API.Models.Grua;
using Gruas.API.Repositories.Implementation;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruaController : ControllerBase
    {
        private readonly IGruaRepository gruaRepository;
        public GruaController(IGruaRepository gruaRepository)
        {
            this.gruaRepository = gruaRepository;
        }
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get()
        {
            var response = await gruaRepository.Get();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await gruaRepository.Get(id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] CreateGrua_Request model)
        {
            var response = await gruaRepository.Create(model, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGrua_Request model)
        {
            var response = await gruaRepository.Update(model, id, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

using Gruas.API.Helpers;
using Gruas.API.Models.DTO.Reportes;
using Gruas.API.Models.DTO.Usuarios;
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
    public class UsuariosController : ControllerBase
    {

        private readonly IUsuariosRepository usuariosRepository;
        public UsuariosController(IUsuariosRepository usuariosRepository)
        {
            this.usuariosRepository = usuariosRepository;
        }

        [HttpPost]
        [Route("CreateUsuarioProveedor")]
        


        public async Task<IActionResult> CreateUsuarioProveedor([FromBody] CreateUsuario_Request model)
        {
            var response = await usuariosRepository.CreateUsuarioProveedor(model, User.GetId());

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Route("GetUsuarios")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetUsuarios([FromBody] GetUsuarios_Request model)
        {
            var response = await usuariosRepository.Get(model);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador, Colaborador")]
        [Route("GetPerfil")]
        public async Task<IActionResult> GetPerfil()
        {
            var response = await usuariosRepository.GetPerfil(Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

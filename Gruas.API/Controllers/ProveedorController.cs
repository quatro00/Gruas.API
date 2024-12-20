using Gruas.API.Helpers;
using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorRepository proveedorRepository;
        public ProveedorController(IProveedorRepository proveedorRepository)
        {
            this.proveedorRepository = proveedorRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [Route("GetProveedores")]
        public async Task<IActionResult> GetProveedores()
        {
            var response = await proveedorRepository.GetProveedores();

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [Route("GetProveedor/{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await proveedorRepository.GetProveedor(id);

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [Route("InsProveedor")]
        public async Task<IActionResult> InsProveedor(InsProvedor_Request request)
        {
            var response = await proveedorRepository.InsProveedor(request, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPost]
        

        [Route("ActivarDesactivarProveedor")]
        public async Task<IActionResult> ActivarDesactivarProveedor(ActivarDesactivarProveedor_Request model)
        {
            var response = await proveedorRepository.ActivarDesactivarProveedor(model.id, model.activo, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }
            return Ok(response.result);
        }

        [HttpPut]
        [Route("UpdateProveedor/{id:Guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateProveedor([FromRoute] Guid id, [FromBody] UpdProvedor_Request request)
        {
            var response = await proveedorRepository.UpdProveedor(request, id, Guid.Parse(User.GetId()));

            if (!response.response)
            {
                ModelState.AddModelError("error", response.message);
                return ValidationProblem(ModelState);
            }

            return Ok(response.result);
        }
    }
}

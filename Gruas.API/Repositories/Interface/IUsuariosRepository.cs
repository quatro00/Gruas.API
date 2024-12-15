using Gruas.API.Models.Grua;
using Gruas.API.Models;
using Gruas.API.Models.DTO.Usuarios;

namespace Gruas.API.Repositories.Interface
{
    public interface IUsuariosRepository
    {
        Task<ResponseModel> CreateUsuarioProveedor(CreateUsuario_Request model, string usuarioId);
        Task<ResponseModel> Update(UpdateUsuario_Request model, Guid id, string usuarioId);
        Task<ResponseModel> Get(GetUsuarios_Request model);
        Task<ResponseModel> Get(Guid id);
    }
}

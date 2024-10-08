using Gruas.API.Models;
using Gruas.API.Models.DTO.Proveedor;

namespace Gruas.API.Repositories.Interface
{
    public interface IProveedorRepository
    {
        public Task<ResponseModel> GetProveedores();
        public Task<ResponseModel> GetProveedor(Guid id);
        public Task<ResponseModel> InsProveedor(InsProvedor_Request model, Guid usuarioId);
        public Task<ResponseModel> UpdProveedor(UpdProvedor_Request model, Guid id, Guid usuarioId);
        public Task<ResponseModel> ActivarDesactivarProveedor(Guid id, bool activo, Guid usuarioId);

    }
}

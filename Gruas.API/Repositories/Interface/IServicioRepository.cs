using Gruas.API.Models.DTO.Proveedor;
using Gruas.API.Models;
using Gruas.API.Models.DTO.Servicio;

namespace Gruas.API.Repositories.Interface
{
    public interface IServicioRepository
    {
        public Task<ResponseModel> InsServicio(RegistraServicio_Request model, Guid usuarioId);
        public Task<ResponseModel> GetServicios(int? estatusServicioId);
        public Task<ResponseModel> GetServicio(Guid id);
    }
}

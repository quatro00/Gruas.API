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
        public Task<ResponseModel> AsignarGrua(AsignarGrua_Request request, Guid usuarioId);
        public Task<ResponseModel> CancelarServicio(CancelarServicio_Request request, Guid usuarioId);
        public Task<ResponseModel> SolicitarCotizaciones(SolicitarCotizacion_Request request, Guid usuarioId);
        public Task<ResponseModel> ColocarEnPropuesta(ColocarEnPropuesta_Request request, Guid usuarioId);
        public Task<ResponseModel> TerminarServicio(TerminarServicio_Request request, Guid usuarioId);
        public Task<ResponseModel> GetServiciosDisponibles(Guid usuarioId);
        public Task<ResponseModel> EnviarCotizacionProveedor(EnviarCotizacionProveedor_Request model, Guid usuarioId);
        public Task<ResponseModel> ModificarCotizacionProveedor(ModificarCotizacionProveedor_Request model, Guid usuarioId);
    }
}

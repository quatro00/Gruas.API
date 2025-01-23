using Gruas.API.Models;
using Gruas.API.Models.DTO.Pagos;

namespace Gruas.API.Repositories.Interface
{
    public interface IPagoRepository
    {
        Task<ResponseModel> GetServiciosPorPagar(Guid proveedorId);
        Task<ResponseModel> RegistrarPagoServicios(InsPagoServicios_Request model, Guid usuarioId);
        Task<ResponseModel> GetPagosMensuales(int anio, int mes, Guid proveedorId);
    }
}

using Gruas.API.Models;
using Gruas.API.Models.DTO.Reportes;

namespace Gruas.API.Repositories.Interface
{
    public interface IReportesRepository
    {
        public Task<ResponseModel> GetPagos(GetPagos_Request model);
        public Task<ResponseModel> GetServicios(GetServicios_Request model);
        public Task<ResponseModel> GetProveedores(GetProveedores_Request model);
        public Task<ResponseModel> GetGruas(GetGruas_Request model);
    }
}

using Gruas.API.Models;

namespace Gruas.API.Repositories.Interface
{
    public interface ICatalogoRepository
    {
        public Task<ResponseModel> GetTipoGrua();
        public Task<ResponseModel> GetEstatusServicio();
        public Task<ResponseModel> GetEstatusPago();
        public Task<ResponseModel> GetEstados();
    }
}

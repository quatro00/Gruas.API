using Gruas.API.Models;

namespace Gruas.API.Repositories.Interface
{
    public interface ICatalogoRepository
    {
        public Task<ResponseModel> GetTipoGrua();
    }
}

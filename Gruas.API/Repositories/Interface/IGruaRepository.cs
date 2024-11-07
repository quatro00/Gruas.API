using Gruas.API.Models;
using Gruas.API.Models.Grua;

namespace Gruas.API.Repositories.Interface
{
    public interface IGruaRepository
    {
        Task<ResponseModel> Create(CreateGrua_Request model, string usuarioId);
        Task<ResponseModel> Update(UpdateGrua_Request model, Guid id, string usuarioId);
        Task<ResponseModel> Get();
        Task<ResponseModel> Get(Guid id);
    }
}

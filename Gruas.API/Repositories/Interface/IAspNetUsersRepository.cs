using Gruas.API.Models;
using Gruas.API.Models.DTO.Auth;

namespace Gruas.API.Repositories.Interface
{
    public interface IAspNetUsersRepository
    {
        Task<ResponseModel> GetUserById(Guid id);
        Task<ResponseModel> ForgotPassword(ForgotPasswordRequestDto model, string token);
    }
}

using Microsoft.AspNetCore.Identity;

namespace Gruas.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
        string CreateRestoreToken(IdentityUser user);
    }
}

using Microsoft.AspNetCore.Identity;

namespace carGooBackend.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}

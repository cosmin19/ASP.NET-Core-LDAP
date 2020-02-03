using ApiLdapIntegration.Models;

namespace ApiLdapIntegration.Services
{
    public interface IAuthenticationService
    {
        AuthResult Login(string userName, string password);
    }
}

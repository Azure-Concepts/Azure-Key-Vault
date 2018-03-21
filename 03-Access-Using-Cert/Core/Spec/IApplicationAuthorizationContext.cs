using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace KeyVault.Core.Spec
{
    public interface IApplicationAuthorizationContext
    {
        Task<string> GetTokenUsingCertificate(string authority, string resource, string scope);
    }
}
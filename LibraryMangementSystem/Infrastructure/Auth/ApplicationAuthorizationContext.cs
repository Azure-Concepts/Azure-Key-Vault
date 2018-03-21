using LMS.Shared.Spec;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace LMS.Auth
{
    public class ApplicationAuthorizationContext : IApplicationAuthorizationContext
    {
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider;
        public ApplicationAuthorizationContext(AzureServiceTokenProvider azureServiceTokenProvider)
        {
            _azureServiceTokenProvider = azureServiceTokenProvider;
        }

        public string GetCurrentUserName()
        {
            return ClaimsPrincipal.Current != null ? ClaimsPrincipal.Current.Identity.Name : "worker";
        }

        public Task<string> GetTokenForKeyVault(string authority, string resource, string scope)
        {
            var keyVaultTokenCallback = new KeyVaultClient.AuthenticationCallback(_azureServiceTokenProvider.KeyVaultTokenCallback);
            return keyVaultTokenCallback(authority, resource, scope);
        }
    }
}
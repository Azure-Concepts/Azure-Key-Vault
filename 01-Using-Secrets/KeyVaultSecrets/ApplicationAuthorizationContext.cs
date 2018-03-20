using KeyVault.Core.Spec;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace KeyVault.Core
{
    /// <summary>
    /// Utility to get authentication token from AAD
    /// </summary>
    public class ApplicationAuthorizationContext: IApplicationAuthorizationContext
    {
        private readonly string _azureTenantId;
        private readonly string _azureAdClientId;
        private readonly string _azureAdClientSecret;
        
        public ApplicationAuthorizationContext(string tenantId, string azureAdClientId, string azureAdClientSecret)
        {
            _azureTenantId = tenantId;
            _azureAdClientId = azureAdClientId;
            _azureAdClientSecret = azureAdClientSecret;
        }
        /// <summary>
        /// Gets AAD token by authenticating using Client ID and Client Secret
        /// </summary>
        /// <param name="authority">Authority Address</param>
        /// <param name="resource">Resource ID of the AAD application</param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public async Task<string> GetTokenUsingClientSecretAsync(string authority, string resource, string scope)
        {
            LoggerCallbackHandler.UseDefaultLogging = false;
            var authContext = new AuthenticationContext(authority);
            var authResult = await authContext.AcquireTokenAsync(resource, new ClientCredential(_azureAdClientId, _azureAdClientSecret));
            return authResult.AccessToken;
        }
    }
}
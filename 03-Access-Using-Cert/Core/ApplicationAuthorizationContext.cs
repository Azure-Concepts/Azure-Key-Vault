using KeyVault.Core.Spec;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Security.Cryptography.X509Certificates;

namespace KeyVault.Core
{
    /// <summary>
    /// Utility to get authentication token from AAD
    /// </summary>
    public class ApplicationAuthorizationContext: IApplicationAuthorizationContext
    {
        private readonly string _azureTenantId;
        private readonly string _azureAdClientId;
        private readonly string _azureAdClientAppCertificateThumbprint;
        private readonly StoreLocation _certificateStoreLocation;
        private readonly StoreName _certificateStoreName;
        private readonly ICertificateService _certificateService;
        
        public ApplicationAuthorizationContext(ICertificateService certificateService, string tenantId, string azureAdClientId, string azureAdClientAppCertificateThumbprint, StoreLocation storeLocation, StoreName storeName)
        {
            _azureTenantId = tenantId;
            _azureAdClientId = azureAdClientId;
            _azureAdClientAppCertificateThumbprint = azureAdClientAppCertificateThumbprint;
            _certificateStoreLocation = storeLocation;
            _certificateStoreName = storeName;
            _certificateService = certificateService;
        }

        /// <summary>
        /// Gets AAD token by authenticating using Client ID and Service Principal certificate
        /// </summary>
        /// <param name="authority">Authority Address</param>
        /// <param name="resource">Resource ID of the AAD application</param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public async Task<string> GetTokenUsingCertificate(string authority, string resource, string scope)
        {
            LoggerCallbackHandler.UseDefaultLogging = false;
            var authContext = new AuthenticationContext(authority);
            var certificate = _certificateService.FindCertificateByThumbprint(_azureAdClientAppCertificateThumbprint, _certificateStoreLocation, _certificateStoreName);
            var certificateCredentials = new ClientAssertionCertificate(_azureAdClientId, certificate);
            var authResult = await authContext.AcquireTokenAsync(resource, certificateCredentials);
            return authResult.AccessToken;
        }
    }
}
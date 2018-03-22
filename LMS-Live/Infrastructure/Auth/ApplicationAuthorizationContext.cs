using LMS.Shared.Spec;
using System.Security.Claims;
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
            return ClaimsPrincipal.Current != null ? ClaimsPrincipal.Current.Identity.Name : "unknown-user";
        }
    }
}
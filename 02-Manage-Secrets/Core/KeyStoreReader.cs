using KeyVault.Core.Spec;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using System.Collections.Generic;

namespace KeyVault.Core
{
    public class KeyStoreReader: IKeyStoreReader
    {
        private readonly KeyVaultSettings _keyVaultSettings;
        private readonly IApplicationAuthorizationContext _applicationAuthorizationContext;
        private readonly KeyVaultClient _keyVaultClient;
        private readonly string _vaultUri;
        private const string KEY_VAULT_URI_FORMAT = "https://{0}.vault.azure.net";

        public KeyStoreReader(KeyVaultSettings keyVaultSettings, IApplicationAuthorizationContext applicationAuthorizationContext)
        {
            _keyVaultSettings = keyVaultSettings;
            _vaultUri = string.Format(KEY_VAULT_URI_FORMAT, _keyVaultSettings.VaultName);
            _applicationAuthorizationContext = applicationAuthorizationContext;
            _keyVaultClient = new KeyVaultClient(_applicationAuthorizationContext.GetTokenUsingClientSecretAsync);
        }

        public KeyStoreReader(KeyVaultSettings keyVaultSettings)
            :this(keyVaultSettings, new ApplicationAuthorizationContext(keyVaultSettings.AADTenantId, keyVaultSettings.KeyVaultClientId, keyVaultSettings.KeyVaultClientSecret))
        {   
        }

        public async Task<Dictionary<string, string>> GetAllSecrets()
        {
            var secrets = new Dictionary<string, string>();
            var secretsInStore = await _keyVaultClient.GetSecretsAsync(_vaultUri);
            foreach (var secret in secretsInStore)
            {
                secrets.Add(secret.Identifier.Name, await GetSecretAsync(secret.Identifier.Name));
            }
            return secrets;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {   
            try
            {
                var secret = await _keyVaultClient.GetSecretAsync(_vaultUri, secretName);
                return secret.Value;
            }
            catch (KeyVaultErrorException error)
            {
                if (error.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;
                throw;
            }
        }
    }
}
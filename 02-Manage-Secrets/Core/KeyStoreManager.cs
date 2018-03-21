using KeyVault.Core.Spec;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using System.Collections.Generic;

namespace KeyVault.Core
{
    public class KeyStoreManager: IKeyStoreManager
    {
        private readonly KeyVaultSettings _keyVaultSettings;
        private readonly IApplicationAuthorizationContext _applicationAuthorizationContext;
        private readonly KeyVaultClient _keyVaultClient;
        private readonly string _vaultUri;
        private const string KEY_VAULT_URI_FORMAT = "https://{0}.vault.azure.net";

        public KeyStoreManager(KeyVaultSettings keyVaultSettings, IApplicationAuthorizationContext applicationAuthorizationContext)
        {
            _keyVaultSettings = keyVaultSettings;
            _vaultUri = string.Format(KEY_VAULT_URI_FORMAT, _keyVaultSettings.VaultName);
            _applicationAuthorizationContext = applicationAuthorizationContext;
            _keyVaultClient = new KeyVaultClient(_applicationAuthorizationContext.GetTokenUsingClientSecretAsync);
        }

        public KeyStoreManager(KeyVaultSettings keyVaultSettings)
            : this(keyVaultSettings, new ApplicationAuthorizationContext(keyVaultSettings.AADTenantId, keyVaultSettings.KeyVaultClientId, keyVaultSettings.KeyVaultClientSecret))
        {
        }

        public async Task AddSecret(string secretName, string secretValue, Dictionary<string, string> tags = null)
        {
            await _keyVaultClient.SetSecretAsync(_vaultUri, secretName, secretValue, tags);
        }

        public async Task DeleteSecret(string secretName)
        {
            await _keyVaultClient.DeleteSecretAsync(_vaultUri, secretName);
        }
    }
}
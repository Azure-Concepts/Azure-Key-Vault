namespace KeyVault.Core
{
    public class KeyVaultSettings
    {
        public string VaultName { get; set; }
        public string KeyVaultClientId { get; set; }
        public string KeyVaultClientSecret { get; set; }
        public string AADTenantId { get; set; }

        public KeyVaultSettings() { }
    }
}
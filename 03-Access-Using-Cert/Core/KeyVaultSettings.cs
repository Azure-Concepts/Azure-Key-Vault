namespace KeyVault.Core
{
    public class KeyVaultSettings
    {
        public string VaultName { get; set; }
        public string KeyVaultClientId { get; set; }
        public string KeyVaultAppCertificateThumbprint { get; set; }
        public int CertificateStoreLocation { get; set; }
        public int CertificateStoreName { get; set; }
        public string AADTenantId { get; set; }

        public KeyVaultSettings() { }
    }
}
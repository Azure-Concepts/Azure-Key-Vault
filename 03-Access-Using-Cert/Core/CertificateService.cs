using System;
using KeyVault.Core.Spec;
using System.Security.Cryptography.X509Certificates;

namespace KeyVault.Core
{
    /// <summary>
    /// Service to get find certificates
    /// </summary>
    public class CertificateService: ICertificateService
    {
        public X509Certificate2 FindCertificateByThumbprint(string certificateThumbprint, StoreLocation certificateStoreLocation = StoreLocation.CurrentUser, StoreName certificateStoreName = StoreName.My)
        {
            var certificateStore = new X509Store(certificateStoreName, certificateStoreLocation);
            certificateStore.Open(OpenFlags.ReadOnly);
            if (certificateStore.Certificates == null || certificateStore.Certificates.Count == 0)
                throw new Exception("No certificate present in the store");

            var certificateCollection = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, validOnly: true);
            if (certificateCollection == null || certificateCollection.Count == 0)
                throw new Exception($"No certificate with thumprint {certificateThumbprint} present in the given store.");

            return certificateCollection[0];
        }
    }
}
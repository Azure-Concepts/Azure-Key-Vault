using System.Security.Cryptography.X509Certificates;

namespace KeyVault.Core.Spec
{
    public interface ICertificateService
    {
        X509Certificate2 FindCertificateByThumbprint(string certificateThumbprint, StoreLocation certificateStoreLocation = StoreLocation.CurrentUser, StoreName certificateStoreName = StoreName.My);
    }
}
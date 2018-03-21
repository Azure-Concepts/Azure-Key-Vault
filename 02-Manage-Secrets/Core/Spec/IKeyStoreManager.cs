using System.Threading.Tasks;
using System.Collections.Generic;

namespace KeyVault.Core.Spec
{
    public interface IKeyStoreManager
    {
        Task AddSecret(string secretName, string secretValue, Dictionary<string, string> tags = null);
        Task DeleteSecret(string secretName);
    }
}
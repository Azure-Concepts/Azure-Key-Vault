using System.Threading.Tasks;
using System.Collections.Generic;

namespace LMS.Shared.Spec
{
    public interface IKeyStoreReader
    {
        Task<Dictionary<string, string>> GetAllSecrets();
        Task<string> GetSecretAsync(string secretName);
    }
}
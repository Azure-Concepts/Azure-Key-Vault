using System.Threading.Tasks;

namespace LMS.Shared.Spec
{
    public interface IApplicationAuthorizationContext
    {
        Task<string> GetTokenForKeyVault(string authority, string resource, string scope);
        string GetCurrentUserName();
    }
}
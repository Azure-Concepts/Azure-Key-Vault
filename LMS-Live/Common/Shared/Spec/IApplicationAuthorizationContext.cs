using System.Threading.Tasks;

namespace LMS.Shared.Spec
{
    public interface IApplicationAuthorizationContext
    {   
        string GetCurrentUserName();
    }
}
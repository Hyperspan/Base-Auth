using Hyperspan.Base.Shared.Config;
using Microsoft.AspNetCore.Identity;

namespace Hyperspan.Auth.Domain.DatabaseModals
{
    public class ApplicationRole<T> : IdentityRole<T>, IBaseEntity<T> where T : IEquatable<T>
    {

    }
}

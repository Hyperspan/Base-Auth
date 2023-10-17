using Base.Shared.Config;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.DatabaseModals
{
    public class ApplicationRole<T> : IdentityRole<T>, IBaseEntity<T> where T : IEquatable<T>
    {

    }
}

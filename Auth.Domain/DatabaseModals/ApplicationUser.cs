using Hyperspan.Auth.Shared.Enums;
using Hyperspan.Base.Shared.Config;
using Microsoft.AspNetCore.Identity;

namespace Hyperspan.Auth.Domain.DatabaseModals;

public class ApplicationUser<T> : IdentityUser<T>, IBaseEntity<T> where T : IEquatable<T>
{
    public RegistrationStages RegistrationStage { get; set; } = RegistrationStages.None;

}
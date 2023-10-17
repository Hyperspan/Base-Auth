using Auth.Shared.Enums;
using Base.Shared.Config;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.DatabaseModals;

public class ApplicationUser<T> : IdentityUser<T>, IBaseEntity<T> where T : IEquatable<T>
{
    public RegistrationStages RegistrationStage { get; set; } = RegistrationStages.None;

}
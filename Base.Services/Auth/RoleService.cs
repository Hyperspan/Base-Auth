using Auth.Domain.DatabaseModals;
using Auth.Services;
using Microsoft.AspNetCore.Identity;

namespace Base.Services.Auth
{
    public class RoleService : RoleService<Guid>, IRoleService
    {
        public RoleService(
            RoleManager<ApplicationRole<Guid>> roleManager,
            UserManager<ApplicationUser<Guid>> userManager
            )
            : base(roleManager, userManager)
        {
        }
    }
}

using Auth.Domain.DatabaseModals;
using Auth.Services;
using Microsoft.AspNetCore.Identity;

namespace Base.Services.Auth
{
    public class UserService : UserService<Guid>, IUserService
    {
        public UserService(UserManager<ApplicationUser<Guid>> userManager) 
            : base(userManager)
        {
        }

        public override async Task SendRegistrationEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public override async Task<string> GenerateJwtTokenAsync(ApplicationUser<Guid> user)
        {
            throw new NotImplementedException();
        }

        public override async Task MobileVerification(ApplicationUser<Guid> userDetails)
        {
            throw new NotImplementedException();
        }

        public override async Task EmailVerification(ApplicationUser<Guid> userDetails)
        {
            throw new NotImplementedException();
        }
    }
}

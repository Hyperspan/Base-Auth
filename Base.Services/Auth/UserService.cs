using Auth.Domain.DatabaseModals;
using Auth.Services;
using Base.Shared.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RamsonDevelopers.UtilEmail;

namespace Base.Services.Auth
{
    public class UserService : UserService<Guid>, IUserService
    {

        public UserService(
            UserManager<ApplicationUser<Guid>> userManager,
            IEmailService emailService,
            IOptions<AppConfiguration> options)
            : base(userManager, emailService, options)
        { }

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

using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Auth.Domain.DatabaseModals;
using Auth.Services;
using Auth.Tests.Helpers;
using Base.Shared;
using Microsoft.AspNetCore.Identity;
using Moq;
using RamsonDevelopers.UtilEmail;

namespace Auth.Tests
{
    [TestFixture]
    public class UserServiceInt
    {
        private readonly IEmailService _emailService;

        public UserServiceInt()
        {
            var mock = new Mock<IEmailService>();
            mock.Setup((x) => x.SendEMailAsync(It.IsAny<SendEmailRequest>()))
                .ReturnsAsync(new MailMessage());

            _emailService = mock.Object;
        }


        [Test]
        public async Task RegisterUser_ExistingError()
        {
            var userManagerMoq = new Mock<UserManager<ApplicationUser<int>>>(Mock.Of<IUserStore<ApplicationUser<int>>>(), null, null, null, null, null, null, null, null);
            userManagerMoq.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => new ApplicationUser<int>());


            var service = new UserService<int>(userManagerMoq.Object, _emailService);

            var data = await service.RegisterUser(userDetails: RegisterHelpers<int>.UserDetails);
            Assert.IsFalse(data.Succeeded);
            Assert.AreEqual(data.ErrorCode, BaseErrorCodes.EmailTaken);
        }

        [Test]
        public async Task RegisterUser_CreateUser_IdentityError_Null()
        {
            var userManagerMoq = new Mock<UserManager<ApplicationUser<int>>>(Mock.Of<IUserStore<ApplicationUser<int>>>(), null, null, null, null, null, null, null, null);

            userManagerMoq.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => null);

            userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser<int>>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser<int> user, string password) => null);


            var service = new UserService<int>(userManagerMoq.Object, _emailService);

            var error = await service.RegisterUser(RegisterHelpers<int>.UserDetails);

            Assert.AreEqual(error.ErrorCode, BaseErrorCodes.IdentityError);
        }

        [Test]
        public async Task RegisterUser_CreateUser_IdentityError_Error()
        {
            var userManagerMoq = new Mock<UserManager<ApplicationUser<int>>>(Mock.Of<IUserStore<ApplicationUser<int>>>(), null, null, null, null, null, null, null, null);

            userManagerMoq.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => null);

            userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser<int>>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser<int> user, string password) =>
                {
                    var error = new IdentityResult();
                    _ = error.Errors.Append(new IdentityError { Code = "", Description = "" });
                    return error;
                });


            var service = new UserService<int>(userManagerMoq.Object, _emailService);
            var data = await service.RegisterUser(RegisterHelpers<int>.UserDetails);
            Assert.AreEqual(data.ErrorCode, BaseErrorCodes.IdentityError);
        }

        [Test]
        public void RegisterUser_CreateUser_RegistrationEmail()
        {
            var userManagerMoq = new Mock<UserManager<ApplicationUser<int>>>(Mock.Of<IUserStore<ApplicationUser<int>>>(), null, null, null, null, null, null, null, null);

            userManagerMoq.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => null);

            userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser<int>>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser<int> user, string password) => new IdentityResultSuccess());


            var service = new UserService<int>(userManagerMoq.Object, _emailService);

            Assert.CatchAsync<NotImplementedException>(async () => await service.RegisterUser(RegisterHelpers<int>.UserDetails));
        }
    }

    internal class IdentityResultSuccess : IdentityResult
    {
        public IdentityResultSuccess()
        {
            Succeeded = true;
        }
    }
}


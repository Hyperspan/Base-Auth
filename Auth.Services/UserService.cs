using Auth.Domain.DatabaseModals;
using Auth.Interfaces;
using Auth.Shared.Enums;
using Auth.Shared.Requests;
using Auth.Shared.Responses;
using Base.Shared;
using Base.Shared.Modals;
using Microsoft.AspNetCore.Identity;

namespace Auth.Services
{
    public abstract class UserService<T> : IUserService<T> where T :
                                  IEquatable<T>
    {
        private readonly UserManager<ApplicationUser<T>> _userManager;

        protected UserService(UserManager<ApplicationUser<T>> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Register User in the application and send verification email if provided.
        /// </summary>
        /// <param name="userDetails">The basic details of the user whose account is been added</param>
        /// <param name="requireEmailVerification">If the user requires to verify their account before login in.</param>
        /// <returns>Newly created user and the registration stage where the user is right now.</returns>
        public virtual async Task<ApiResponseModal<RegisterResponse>> RegisterUser(RegisterUserRequest userDetails, bool requireEmailVerification = false)
        {
            try
            {
                // TODO: Validate request object
                // Check for existing users
                var existingUser = await _userManager.FindByEmailAsync(userDetails.Email);
                if (existingUser != null)
                {
                    // Return if is an existing user
                    return await ApiResponseModal<RegisterResponse>.FatalAsync(
                        new ApiErrorException("User with same email address already exists."),
                        BaseErrorCodes.EmailTaken);
                }

                // create user's record
                var applicationUser = new ApplicationUser<T>
                {
                    UserName = userDetails.UserName,
                    NormalizedUserName = userDetails.UserName.ToUpper(),
                    Email = userDetails.Email,
                    NormalizedEmail = userDetails.UserName.ToUpper(),
                    EmailConfirmed = !requireEmailVerification,
                    PhoneNumber = userDetails.MobileNo,
                    PhoneNumberConfirmed = !requireEmailVerification,
                    // Check if requireEmailVerification is true
                    RegistrationStage = requireEmailVerification
                        ? RegistrationStages.Registered  // set registration stage to 'Registered' if requireEmailVerification is true
                        : RegistrationStages.Completed   // set registration stage to 'Completed' if requireEmailVerification is false
                };


                var userResponse = await _userManager.CreateAsync(applicationUser, userDetails.Password);

                if (userResponse is null or { Errors: { } })
                {
                    throw new ApiErrorException(BaseErrorCodes.IdentityError);
                }

                // if true then send confirm email
                await SendRegistrationEmail(applicationUser.Email);
                // Return newly created user back
                var response = new RegisterResponse
                {
                    Email = userDetails.Email,
                    RegistrationStage = applicationUser.RegistrationStage,
                };

                return await ApiResponseModal<RegisterResponse>.SuccessAsync(response);

            }
            catch (ApiErrorException e)
            {
                return await ApiResponseModal<RegisterResponse>.FatalAsync(e, BaseErrorCodes.UnknownSystemException);
            }
        }

        /// <summary>
        /// Generate Authentication Token and log user in.
        /// </summary>
        /// <param name="loginUserRequest">Login Credentials</param>
        /// <param name="requireEmailVerification">Does the user require Email verification to be done to log in.</param>
        /// <returns></returns>
        public virtual async Task<ApiResponseModal<LoginResponse>> UserLogin(LoginUserRequest loginUserRequest, bool requireEmailVerification = false)
        {
            try
            {
                // Check if user is an existing member of the system.
                var user = await _userManager.FindByNameAsync(loginUserRequest.UserName);
                // Throw exception of UserNotFound if user is null.
                if (user == null)
                {
                    throw new ApiErrorException(BaseErrorCodes.UserNotFound);
                }

                if (requireEmailVerification && !user.EmailConfirmed)
                {
                    throw new ApiErrorException(BaseErrorCodes.EmailNotVerified);
                }

                // Check if the user's registration stage is complete
                if (user.RegistrationStage != RegistrationStages.Completed)
                {
                    throw user.RegistrationStage switch
                    {
                        RegistrationStages.EmailVerification => new ApiErrorException(BaseErrorCodes.EmailNotVerified),
                        RegistrationStages.MobileVerification =>
                            new ApiErrorException(BaseErrorCodes.MobileNotVerified),
                        RegistrationStages.None => new ApiErrorException(BaseErrorCodes.MobileNotVerified),
                        _ => throw new ArgumentOutOfRangeException(BaseErrorCodes.UnknownSystemException)
                    };
                }

                // Check credentials provided by user
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginUserRequest.Password);
                if (!passwordCheck) throw new ApiErrorException(BaseErrorCodes.IncorrectCredentials);

                // Create User's Token required to authenticate
                var token = await GenerateJwtTokenAsync(user);

                // Return Login Response
                var response = new LoginResponse
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Token = token
                };

                return await ApiResponseModal<LoginResponse>.SuccessAsync(response);
            }
            catch (ApiErrorException e)
            {
                return await ApiResponseModal<LoginResponse>.FatalAsync(e);
            }
            catch (Exception e)
            {
                return await ApiResponseModal<LoginResponse>.FatalAsync(e, BaseErrorCodes.UnknownSystemException);
            }
        }
        public virtual Task<ApiResponseModal> ChangePassword(object userDetails)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ApiResponseModal> ForgetPassword(object userDetails)
        {
            throw new NotImplementedException();
        }

        //public virtual Task ChangeEmail(object userDetails)
        //{
        //    throw new NotImplementedException();
        //}
        //public virtual Task ChangePhone(object userDetails)
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Abstract method to Send Emails after registration to verify
        /// </summary>
        /// <returns>Task</returns>
        public abstract Task SendRegistrationEmail(string emailAddress);

        /// <summary>
        /// Abstract Method to Generate Jwt Token based on roles and claims assigned to user
        /// </summary>
        /// <param name="user">The User whose token should be generated.</param>
        /// <returns>the Jwt Token</returns>
        public abstract Task<string> GenerateJwtTokenAsync(ApplicationUser<T> user);

        public abstract Task MobileVerification(ApplicationUser<T> userDetails);
        public abstract Task EmailVerification(ApplicationUser<T> userDetails);
    }
}

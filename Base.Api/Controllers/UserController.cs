﻿using Auth.Shared.Requests;
using Auth.Shared.Responses;
using Base.Services.Auth;
using Base.Shared.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Base.Api.Controllers
{
    [ApiController]
    [Route("api/account/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            => _userService = userService;


        [HttpPost("register")]
        public async Task<ApiResponseModal<RegisterResponse>> RegisterUserAsync(RegisterUserRequest request)
            => await _userService.RegisterUser(request, true);

        [HttpPost("Login")]
        public async Task<ApiResponseModal<LoginResponse>> LoginUserAsync(LoginUserRequest request)
            => await _userService.UserLogin(request, true);



        [HttpPost("ChangePassword")]
        public async Task<ApiResponseModal> ChangePassword(object userDetails)
            => await _userService.ChangePassword(userDetails);

        [HttpPost("ForgetPassword")]
        public async Task<ApiResponseModal> ForgetPassword(object userDetails)
            => await _userService.ForgetPassword(userDetails);

    }
}

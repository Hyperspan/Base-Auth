﻿using Hyperspan.Auth.Domain.DatabaseModals;
using Hyperspan.Auth.Shared.Requests;
using Hyperspan.Base.Services.Auth;
using Hyperspan.Base.Shared.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyperspan.Base.Api.Controllers
{
    [ApiController]
    [Route("api/account/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        //[HttpPost]
        //public async Task<ApiResponseModal> CreateRoleAsync(CreateRoleRequest request)
        //    => await _roleService.CreateRoleAsync(request);

        [HttpGet]
        public async Task<ApiResponseModal<List<ApplicationRole<Guid>>>> ListAllRolesAsync()
            => await _roleService.ListAllRolesAsync();

        [HttpPost("assign-role")]
        [Authorize]
        public async Task<ApiResponseModal<ApplicationRole<Guid>>> AssignUserRole(AssignUserRoleRequest<Guid> request)
            => await _roleService.AssignUserRole(request);

        [HttpPost("remove-role")]
        public async Task<ApiResponseModal> RemoveRole(RemoveUserRoleRequest<Guid> request)
            => await _roleService.RemoveRole(request);

        [HttpGet("get-role-userid/{userId:guid}")]
        public async Task<ApiResponseModal<List<ApplicationRole<Guid>>>> GetUserRoles(Guid userId)
            => await _roleService.GetUserRoles(userId);

    }
}

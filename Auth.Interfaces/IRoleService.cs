using Auth.Domain.DatabaseModals;
using Auth.Shared.Requests;
using Base.Shared.Modals;

namespace Auth.Interfaces
{
    public interface IRoleService<T> where T : IEquatable<T>
    {
        Task<ApiResponseModal> CreateRoleAsync(CreateRoleRequest request);
        Task<ApiResponseModal<List<ApplicationRole<T>>>> ListAllRolesAsync();
        Task<ApiResponseModal<ApplicationRole<T>>> AssignUserRole(AssignUserRoleRequest<T> request);
        Task<ApiResponseModal> RemoveRole(RemoveUserRoleRequest<T> request);
        Task<ApiResponseModal<List<ApplicationRole<T>>>> GetUserRoles(T userId);
    }
}

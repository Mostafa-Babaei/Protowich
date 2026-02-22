using Application.Common;
using Application.Common.Models;
using Application.Features.Auth.DTOs;
using Application.Features.Users.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.AuthenticationInterface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ApiResult<object>> VerifyOtpAsync(VerifyOtpRequest model);
        Task<ApiResult<object>> LoginAsync(string email, string password);
        Task<ApiResult<object>> GenerateTokensAsync(User user);
        Task<ApiResult<object>> RefreshTokenAsync(string refreshToken);
        Task<UserListDto?> GetUserDetails(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> VerifyResetCodeAsync(string email, string code);
        Task<ApiResult<string>> ResetPasswordAsync(string email, string code, string newPassword);
        Task<ApiResult<string>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<UserRolesDto> GetUserRolesAsync(Guid userId);
        Task<ApiResult<string>> UpdateUserRolesAsync(Guid userId, List<int> roleIds);
        Task<ApiResult<string>> UpsertUserAsync(UserUpsertDto dto);
        Task<ApiResult<PagedResult<UserListDto>>> GetAllUsersAsync(int page = 1, int pageSize = 10, string? keyword = null);
        Task<ApiResult<string>> DeleteUserAsync(Guid userId);
        Task<ApiResult<string>> ToggleActiveAsync(Guid userId);
        Task<ApiResult<string>> UpdateProfileAsync(Guid userId, ProfileUpdateDto dto);
        Task<ApiResult<string>> ChangePasswordAsync(ChangeUserPasswordDto dto);
        Task<ApiResult<List<Company>>> GetAllCompany();
        Task<ApiResult<string>> SetCompanyForUser(Guid userId,int companyId);

    }
}

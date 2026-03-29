using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Models;
using Application.Features.Auth.DTOs;
using Application.Features.Menu.Dtos;
using Application.Features.Users.DTOs;
using Application.Interfaces;
using Application.Interfaces.AuthenticationInterface;
using Application.Resources;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMenuItemRepository menuRepository;
        private readonly ISmsService smsService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<ValidationMessages> _localizer;
        private readonly IConfiguration _config;

        public UserRepository(AppDbContext context,
            IMenuItemRepository menuRepository,
            ISmsService smsService,
            IMapper mapper,
            IStringLocalizer<ValidationMessages> localizer,
            IConfiguration config) : base(context)
        {
            _context = context;
            this.menuRepository = menuRepository;
            this.smsService = smsService;
            this.mapper = mapper;
            _config = config;
            _localizer = localizer;

        }

        // 🔹 متد کمکی تولید JWT
        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT Key is missing. Configure Jwt:Key in appsettings.");

            var issuer = _config["Jwt:Issuer"] ?? "Protowich";
            var audience = _config["Jwt:Audience"] ?? "ProtowichClients";
            if (!int.TryParse(_config["Jwt:AccessTokenExpirationMinutes"], out int accessTokenExpirationMinutes) || accessTokenExpirationMinutes <= 0)
                accessTokenExpirationMinutes = 60;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim("display_name", user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };
            var permissions = (
                from ur in _context.UserRoles
                join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                join p in _context.Permissions on rp.PermissionId equals p.Id
                where ur.UserId == user.Id
                select p.Code
            ).Distinct().ToList();

            claims.AddRange(permissions.Select(code => new Claim("Permission", code)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(accessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResult<object>> GenerateTokensAsync(User user)
        {
            try
            {
                var accessToken = GenerateJwtToken(user);
                var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.Now.AddDays(1);
                var roles = await GetRoleOfUserAsync(user.Id);
                var menus = await GetMenuItemsAsync(user.Id);

                if (!int.TryParse(_config["Jwt:AccessTokenExpirationMinutes"], out int accessTokenExpirationMinutes))
                    accessTokenExpirationMinutes = 1440;
                if (!user.LoginWithSms)
                {

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return ApiResult<object>.Success(new
                    {
                        requiresOtp = false,
                        accessToken = accessToken,
                        expiresIn = accessTokenExpirationMinutes,
                        menus = menus,
                        roles = roles,
                        user = user
                    }, _localizer["Auth.LoginSuccessful"]);
                }

                // ✅ اگر LoginWithSms فعال است ولی موبایل ندارد
                if (string.IsNullOrWhiteSpace(user.Mobile))
                    return ApiResult<object>.Error("شماره موبایل برای ورود پیامکی ثبت نشده است");

                // ✅ OTP Flow
                user.OtpCode = GenerateOtpCode(6);
                user.OtpExpiresAt = DateTime.Now.AddMinutes(30);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                //ارسال پیامک
                //await _smsService.SendOtpAsync(user.Mobile, code);
                await smsService.SendTextAsync(user.Mobile!, $"کد ورود: {user.OtpCode}");
                string otpToken = GenerateOtpToken(user);
                return ApiResult<object>.Success(new
                {
                    requiresOtp = true,
                    otpToken = otpToken,
                    maskedMobile = MaskMobile(user.Mobile, 4)
                }, _localizer["Auth.LoginSuccessful"]);

            }
            catch (Exception ex)
            {
                return ApiResult<object>.Error("خطا در تولید توکن", developerMessage: ex.Message);
            }

        }
        private string GenerateOtpToken(User user)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT Key is missing. Configure Jwt:Key in appsettings.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("otp", "true")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "Protowich",
                audience: _config["Jwt:Audience"] ?? "ProtowichClients",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string MaskMobile(string? mobile, int visibleDigits = 4)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return string.Empty;

            // فقط عددها را نگه دار (برای +98 و ...)
            var digits = new string(mobile.Where(char.IsDigit).ToArray());

            if (digits.Length <= visibleDigits)
                return new string('*', digits.Length);

            var maskedLength = digits.Length - visibleDigits;
            return new string('*', maskedLength) + digits.Substring(maskedLength);
        }

        private static string GenerateOtpCode(int length = 6)
        {
            var max = (int)Math.Pow(10, length) - 1;
            return RandomNumberGenerator.GetInt32(0, max).ToString().PadLeft(length, '0');
        }

        public async Task<ApiResult<object>> VerifyOtpAsync(VerifyOtpRequest model)
        {
            Guid userId;

            try
            {
                userId = GetUserIdFromOtpToken(model.OtpToken);
            }
            catch
            {
                return ApiResult<object>.Error(_localizer["Auth.InvalidOrExpiredCode"]);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return ApiResult<object>.Error(_localizer["User.NotFound"]);

            if (user.OtpCode != model.Code)
                return ApiResult<object>.Error(_localizer["Auth.InvalidOrExpiredCode"]);

            if (user.OtpExpiresAt < DateTime.Now)
                return ApiResult<object>.Error(_localizer["Auth.InvalidOrExpiredCode"]);

            // ✅ OTP مصرف شد
            user.OtpCode = null;
            user.OtpExpiresAt = null;

            var accessToken = GenerateJwtToken(user);
            var roles = await GetRoleOfUserAsync(user.Id);
            var menus = await GetMenuItemsAsync(user.Id);

            int.TryParse(_config["Jwt:AccessTokenExpirationMinutes"], out int exp);

            await _context.SaveChangesAsync();

            return ApiResult<object>.Success(new
            {
                requiresOtp = false,
                accessToken,
                expiresIn = exp * 60,
                roles,
                menus,
                user
            }, _localizer["Auth.LoginSuccessful"]);
        }

        private Guid GetUserIdFromOtpToken(string otpToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(otpToken);

            var userIdClaim = jwt.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new SecurityTokenException("Invalid OTP token");

            return Guid.Parse(userIdClaim.Value);
        }

        public async Task<ApiResult<object>> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
                return ApiResult<object>.Error(_localizer["Validation.RefreshToken.Invalid"]);

            if (user.RefreshTokenExpiry < DateTime.Now)
                return ApiResult<object>.Error(_localizer["Auth.InvalidOrExpiredCode"]);

            return await GenerateTokensAsync(user);
        }


        // 🔹 متد مشترک لاگین
        private async Task<ApiResult<object>> LoginInternalAsync(string userName, string password, Func<User, Task<bool>> condition = null!)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return ApiResult<object>.Error(_localizer["Auth.InvalidCredentials"]);

            var inputHash = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
            );

            if (user.Password != inputHash)
                return ApiResult<object>.Error(_localizer["Auth.InvalidCredentials"]);

            if (condition != null && !await condition(user))
                return ApiResult<object>.Error(_localizer["Common.AccessDenied"]);

            user.LastLogin = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var tokens = await GenerateTokensAsync(user);
            //var token = GenerateJwtToken(user);
            return tokens;

        }

        // 🔹 Login عمومی
        public async Task<ApiResult<object>> LoginAsync(string userName, string password)
        {
            return await LoginInternalAsync(userName, password);
        }
        public async Task<List<MenuItemDto>> GetMenuItemsAsync(Guid userId)
        {
            // 1) نقش‌های کاربر
            var roleIds = await _context.Set<UserRole>()
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .Distinct()
                .ToListAsync();

            if (roleIds.Count == 0)
                return new List<MenuItemDto>();

            // 2) منوهای نقش‌ها (فقط آی‌دی‌ها)
            // intersection: منویی که تعداد نقش‌های مرتبطش == تعداد roleIds باشد
            var sharedMenuIds = await _context.Set<MenuRole>()
                .AsNoTracking()
                .Where(mr => roleIds.Contains(mr.RoleId))
                .GroupBy(mr => mr.MenuItemId)
                .Where(g => g.Select(x => x.RoleId).Distinct().Count() == roleIds.Count)
                .Select(g => g.Key)
                .ToListAsync();

            if (sharedMenuIds.Count == 0)
                return new List<MenuItemDto>();

            // 3) دریافت منوها
            var menus = await _context.Set<MenuItem>()
                .AsNoTracking()
                .Where(m => sharedMenuIds.Contains(m.Id))
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.DisplayOrder)
                .ToListAsync();

            return mapper.Map<List<MenuItem>, List<MenuItemDto>>(menus);
        }

        private List<MenuItemDto> GetMenuItems(List<Role> listOfRole, Guid? userId = null)
        {
            //var menuItems = menuRepository.GetAllAsync().Result.ToList();
            //var menusDto = mapper.Map<List<MenuItem>, List<MenuItemDto>>(menuItems);
            //return menusDto;
            var list = new List<MenuItemDto>();
            if (!listOfRole.Any())
                return list;
            var roleIds = listOfRole.Select(e => e.Id).ToList();
            if (roleIds.Count == 0)
                return list;

            // 2) منوهای نقش‌ها (فقط آی‌دی‌ها)
            // intersection: منویی که تعداد نقش‌های مرتبطش == تعداد roleIds باشد
            var sharedMenuIds = _context.Set<MenuRole>()
                .AsNoTracking()
                .Where(mr => roleIds.Contains(mr.RoleId))
                .GroupBy(mr => mr.MenuItemId)
                .Where(g => g.Select(x => x.RoleId).Distinct().Count() == roleIds.Count)
                .Select(g => g.Key)
                .ToList();

            if (sharedMenuIds.Count == 0)
                return new List<MenuItemDto>();

            // 3) دریافت منوها
            var menus = _context.Set<MenuItem>()
                .AsNoTracking()
                .Where(m => sharedMenuIds.Contains(m.Id))
                .OrderBy(m => m.Section)
                .ThenBy(m => m.DisplayOrder)
                .ToList();

            return mapper.Map<List<MenuItem>, List<MenuItemDto>>(menus);
        }
        public async Task<List<RoleDto>> GetRoleOfUserAsync(Guid userId)
        {
            var roles = await _context.Set<UserRole>()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Role)              // یا RoleId اگر فقط آی‌دی می‌خوای
                .Distinct()
                .ToListAsync();

            return mapper.Map<List<Role>, List<RoleDto>>(roles);
        }
        //private List<Role> GetRoleOfUser(Guid? userId = null)
        //{
        //    List<Role> roles = new List<Role>(); ;
        //    var rolesItem = _context.UserRoles.Where(e => e.UserId == userId).ToList();
        //    if (!rolesItem.Any())
        //        return roles;
        //    List<int> roleIds = rolesItem.Select(e => e.RoleId).ToList();
        //    roles = _context.Roles.Where(e => roleIds.Contains(e.Id)).ToList();
        //    return roles;
        //}

        public async Task<UserListDto?> GetUserDetails(Guid id)
        {

            var u = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (u == null)
                return null;

            // 1️⃣ کاربر

            var user = new UserListDto
            {
                Id = u.Id,
                Name = u.FirstName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.UserName,
                Mobile = u.Mobile,
                LoginWithSms = u.LoginWithSms,
                Phone = u.Phone,
                Email = u.Email,
                LastLogin = u.LastLogin,
                IsActive = u.IsActive,
                Roles = new List<RoleDto>() // فعلاً خالی
            };

            // 2️⃣ رول‌ها از جدول UserRoles
            var roles = await _context.UserRoles
                .Include(e => e.Role)
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => new
                {
                    ur.UserId,
                    Role = new RoleDto
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        DisplayName = ur.Role.DisplayName
                    }
                })
                .ToListAsync();

            user.Roles = roles.Select(x => x.Role).ToList();
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);


        public async Task<bool> VerifyResetCodeAsync(string email, string code)
        {
            // 1️⃣ پیدا کردن کاربر
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            // 2️⃣ بررسی کد
            if (string.IsNullOrEmpty(user.ResetCode))
                return false;

            if (user.ResetCode != code)
                return false;



            return true;
        }

        /// <summary>
        /// بازنشانی رمز عبور
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> ResetPasswordAsync(string email, string code, string newPassword)
        {
            // 1️⃣ بررسی کاربر
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return ApiResult<string>.Error(_localizer["User.NotFound"]);

            // 2️⃣ بررسی صحت کد
            if (user.ResetCode != code)
                return ApiResult<string>.Error(_localizer["Auth.InvalidResetRequest"]);

            // 3️⃣ هش کردن رمز جدید
            var hashedPassword = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(newPassword))
            );

            // 4️⃣ به‌روزرسانی اطلاعات کاربر
            user.Password = hashedPassword;
            user.ResetCode = null; // ✅ کد فقط یک‌بار مصرف

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return ApiResult<string>.Success(_localizer["Auth.PasswordResetSuccess"]);
        }



        /// <summary>
        /// بروزرسانی پروفایل کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return ApiResult<string>.Error("User not found.");

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                user.FirstName = request.FirstName.Trim();

            if (!string.IsNullOrWhiteSpace(request.LastName))
                user.LastName = request.LastName.Trim();

            if (!string.IsNullOrWhiteSpace(request.Mobile))
                user.Mobile = request.Mobile.Trim();

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Mobile = request.Email.Trim();

            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = userId.ToString();

            await _context.SaveChangesAsync();

            return ApiResult<string>.Success("Profile updated successfully.");
        }

        /// <summary>
        /// تغییر رمز عبور
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            // بررسی رمز فعلی
            var hashedPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(currentPassword)));
            if ((hashedPassword != user.Password))
                return false;

            var hashedNewPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(newPassword)));

            user.Password = hashedNewPassword;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// دریافت نقش‌های یک کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserRolesDto> GetUserRolesAsync(Guid userId)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.User.Id == userId)
                .Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    DisplayName = ur.Role.DisplayName
                })
                .ToListAsync();

            return new UserRolesDto { UserId = userId, Roles = roles };
        }

        /// <summary>
        /// بروزرسانی نقش‌های یک کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> UpdateUserRolesAsync(Guid userId, List<int> roleIds)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                    return ApiResult<string>.Error(_localizer["User.NotFound"]);

                // حذف نقش‌های قبلی
                var oldRoles = _context.UserRoles.Where(x => x.User.Id == userId);
                _context.UserRoles.RemoveRange(oldRoles);
                await _context.SaveChangesAsync();

                // افزودن نقش‌های جدید
                foreach (var rid in roleIds.Distinct())
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        User = user,
                        RoleId = rid
                    });
                }

                await _context.SaveChangesAsync();

                // ✅ Commit
                await transaction.CommitAsync();

                return ApiResult<string>.Success(_localizer["User.Updated"]);
            }
            catch (Exception ex)
            {
                // ❌ Rollback
                await transaction.RollbackAsync();
                return ApiResult<string>.Error($"Failed to update roles: {ex.Message}");
            }
        }

        /// <summary>
        /// ایجاد یا بروزرسانی کاربر
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> UpsertUserAsync(UserUpsertDto dto)
        {
            var now = DateTime.Now;

            var hashedPassword = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(dto.Password ?? "123456"))
            );

            if (dto.Id == null || dto.Id == Guid.Empty)
            {
                // ✅ Create

                if (await ExistsAsync(e => e.UserName == dto.UserName))
                    return ApiResult<string>.Error(_localizer["Registration.EmailAlreadyRegistered"]);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    Mobile = dto.Mobile,
                    LoginWithSms = dto.LoginWithSms,
                    UserName = dto.UserName,
                    Password = hashedPassword,
                    IsActive = true,
                    CreatedAt = now,
                    IsSystemAdmin = true,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // نقش‌ها
                await UpdateUserRolesAsync(user.Id, dto.RoleIds);
                return ApiResult<string>.Success(data: user.Id.ToString(), _localizer["User.Created"]);
            }
            else
            {
                // ✅ Update
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (user == null)
                    return ApiResult<string>.Error(_localizer["User.NotFound"]);

                //user.Email = dto.Email;
                user.FirstName = dto.FirstName;
                user.Mobile = dto.Mobile;
                user.LoginWithSms = dto.LoginWithSms;

                if (dto.Password is not null)
                    user.Password = hashedPassword;
                user.UpdatedAt = now;

                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    user.Password = Convert.ToBase64String(
                        SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
                    );
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                await UpdateUserRolesAsync(user.Id, dto.RoleIds);

                return ApiResult<string>.Success(data: user.Id.ToString(), _localizer["User.Updated"]);
            }
        }

        /// <summary>
        /// دریافت لیست کاربران با صفحه‌بندی و فیلتر کلیدواژه
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<ApiResult<PagedResult<UserListDto>>> GetAllUsersAsync(
            int page = 1,
            int pageSize = 10,
            string? keyword = null)
        {
            var query = _context.Users
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(u =>
                    u.Email.Contains(keyword) ||
                    (u.FirstName + " " + u.LastName).Contains(keyword));

            var totalCount = await query.CountAsync();

            // 1️⃣ کاربران
            var users = await query
                .OrderBy(u => u.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    Name = u.FirstName.Trim(),
                    Username = u.UserName,
                    Mobile = u.Mobile,
                    LoginWithSms = u.LoginWithSms,
                    Phone = u.Phone,
                    Email = u.Email,
                    LastLogin = u.LastLogin,
                    IsActive = u.IsActive,
                    Roles = new List<RoleDto>() // فعلاً خالی
                })
                .ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();

            // 2️⃣ رول‌ها از جدول UserRoles
            var roles = await _context.UserRoles
                .Include(e => e.Role)
                .Where(ur => userIds.Contains(ur.UserId))
                .Select(ur => new
                {
                    ur.UserId,
                    Role = new RoleDto
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        DisplayName = ur.Role.DisplayName
                    }
                })
                .ToListAsync();

            // 3️⃣ مپ در حافظه
            var roleLookup = roles
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Role).ToList());

            foreach (var user in users)
                if (roleLookup.TryGetValue(user.Id, out var userRoles))
                    user.Roles = userRoles;

            var result = new PagedResult<UserListDto>(users, totalCount, page, pageSize);
            return ApiResult<PagedResult<UserListDto>>.Success(result);
        }


        /// <summary>
        /// حذف کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResult<string>> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return ApiResult<string>.Error(_localizer["User.NotFound"]);

            //System administrator cannot be deleted.
            //Der Systemadministrator kann nicht gelöscht werden.
            if (user.IsSystemAdmin ?? false)
                return ApiResult<string>.Error(_localizer["User.SystemAdminDeleteNotAllowed"]);

            // حذف نقش‌ها
            if (user.UserRoles != null && user.UserRoles.Any())
            {
                _context.UserRoles.RemoveRange(user.UserRoles);
            }

            // حذف خود کاربر
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return ApiResult<string>.Success(_localizer["User.Deleted"]);
        }

        public async Task<ApiResult<string>> ToggleActiveAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return ApiResult<string>.Error(_localizer["User.NotFound"]);

            //System administrator cannot be deleted.
            //Der Systemadministrator kann nicht gelöscht werden.
            //if (user.IsSystemAdmin ?? false)
            //    return ApiResult<string>.Error(_localizer["User.SystemAdminDeleteNotAllowed"]);
            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
            return ApiResult<string>.Success(_localizer["User.Updated"]);
        }


        // -------------------------------
        // UPDATE PROFILE
        // -------------------------------
        public async Task<ApiResult<string>> UpdateProfileAsync(Guid userId, ProfileUpdateDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return ApiResult<string>.Error(_localizer["User.NotFound"]);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Mobile = dto.Mobile;
            user.Phone = dto.PhoneNumber;

            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = userId.ToString();

            await _context.SaveChangesAsync();

            return ApiResult<string>.Success(_localizer["User.ProfileUpdated"]);
        }

        // -------------------------------
        // CHANGE PASSWORD
        // -------------------------------
        public async Task<ApiResult<string>> ChangePasswordAsync(ChangeUserPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (user == null)
                return ApiResult<string>.Error(_localizer["User.NotFound"]);


            var hashedNew = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword))
            );

            user.Password = hashedNew;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return ApiResult<string>.Success(_localizer["User.PasswordChanged"]);
        }

        public async Task<ApiResult<List<Company>>> GetAllCompany()
        {
            var companies = await _context.Companies.ToListAsync();
            return ApiResult<List<Company>>.Success(companies);
        }

        public async Task<ApiResult<string>> SetCompanyForUser(Guid userId, int companyId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                return ApiResult<string>.Error("کاربر یافت نشد");


            if (!_context.Companies.Any(e => e.Id == companyId))
                return ApiResult<string>.Error("شرکت یافت نشد");

            user.CompanyId = companyId;
            await SaveChangesAsync();
            return ApiResult<string>.Error("شرکت کاربر تغییر کرد");

        }
    }
}

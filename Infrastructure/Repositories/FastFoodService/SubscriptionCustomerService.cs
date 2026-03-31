using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Application.Interfaces.FastFoodInterface;
using Domain.Entities.FastFood;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubscriptionCustomerService : GenericRepository<SubscriptionCustomer>, ISubscriptionCustomerService
    {
        private readonly AppDbContext _db;

        public SubscriptionCustomerService(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<ApiResult<PagedResult<SubscriptionCustomerListItemDto>>> GetPagedAsync(
            int page, int pageSize, string? keyword = null, CancellationToken ct = default)
        {
            try
            {
                keyword = keyword?.Trim();
                var q = _db.Set<SubscriptionCustomer>().AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    q = q.Where(x =>
                        x.FirstName.Contains(keyword) ||
                        x.LastName.Contains(keyword) ||
                        x.Mobile.Contains(keyword) ||
                        x.SubscriptionCode.Contains(keyword));
                }

                var total = await q.CountAsync(ct);
                var items = await q
                    .OrderByDescending(x => x.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new SubscriptionCustomerListItemDto
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Mobile = x.Mobile,
                        Phone = x.Phone,
                        Address = x.Address,
                        Description = x.Description,
                        SubscriptionCode = x.SubscriptionCode,
                        IsActive = x.IsActive
                    })
                    .ToListAsync(ct);

                return ApiResult<PagedResult<SubscriptionCustomerListItemDto>>.Success(new PagedResult<SubscriptionCustomerListItemDto>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total,
                    Items = items
                });
            }
            catch (Exception ex)
            {
                return ApiResult<PagedResult<SubscriptionCustomerListItemDto>>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<int>> CreateAsync(SubscriptionCustomerUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                var validate = ValidateDto(dto);
                if (!validate.IsSuccess) return ApiResult<int>.Error(validate.Message ?? "اطلاعات نامعتبر است.");
                for (var attempt = 0; attempt < 5; attempt++)
                {
                    var entity = new SubscriptionCustomer
                    {
                        FirstName = dto.FirstName.Trim(),
                        LastName = dto.LastName.Trim(),
                        Mobile = dto.Mobile.Trim(),
                        Phone = dto.Phone?.Trim(),
                        Address = dto.Address.Trim(),
                        Description = dto.Description?.Trim(),
                        SubscriptionCode = await GetNextSubscriptionCodeAsync(ct),
                        IsActive = dto.IsActive,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    _db.Set<SubscriptionCustomer>().Add(entity);

                    try
                    {
                        await _db.SaveChangesAsync(ct);
                        return ApiResult<int>.Success(entity.Id, "مشترک با موفقیت ثبت شد.");
                    }
                    catch (DbUpdateException)
                    {
                        _db.Entry(entity).State = EntityState.Detached;
                    }
                }

                return ApiResult<int>.Error("ثبت مشترک انجام نشد. مجدد تلاش کنید.");
            }
            catch (Exception ex)
            {
                return ApiResult<int>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> UpdateAsync(int id, SubscriptionCustomerUpsertDto dto, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");
                var validate = ValidateDto(dto);
                if (!validate.IsSuccess) return ApiResult<string>.Error(validate.Message ?? "اطلاعات نامعتبر است.");

                var entity = await _db.Set<SubscriptionCustomer>().FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("مشترک یافت نشد.");

                entity.FirstName = dto.FirstName.Trim();
                entity.LastName = dto.LastName.Trim();
                entity.Mobile = dto.Mobile.Trim();
                entity.Phone = dto.Phone?.Trim();
                entity.Address = dto.Address.Trim();
                entity.Description = dto.Description?.Trim();
                entity.IsActive = dto.IsActive;
                entity.UpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success("اطلاعات مشترک ویرایش شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        public async Task<ApiResult<string>> SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
        {
            try
            {
                if (id <= 0) return ApiResult<string>.Error("شناسه نامعتبر است.");
                var entity = await _db.Set<SubscriptionCustomer>().FirstOrDefaultAsync(x => x.Id == id, ct);
                if (entity == null) return ApiResult<string>.Error("مشترک یافت نشد.");

                entity.IsActive = isActive;
                entity.UpdatedAt = DateTime.Now;
                await _db.SaveChangesAsync(ct);
                return ApiResult<string>.Success(isActive ? "اشتراک فعال شد." : "اشتراک غیرفعال شد.");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Error("خطای سرور", developerMessage: ex.Message);
            }
        }

        private static ApiResult<string> ValidateDto(SubscriptionCustomerUpsertDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName)) return ApiResult<string>.Error("نام الزامی است.");
            if (string.IsNullOrWhiteSpace(dto.LastName)) return ApiResult<string>.Error("نام خانوادگی الزامی است.");
            if (string.IsNullOrWhiteSpace(dto.Mobile)) return ApiResult<string>.Error("موبایل الزامی است.");
            if (string.IsNullOrWhiteSpace(dto.Address)) return ApiResult<string>.Error("آدرس الزامی است.");
            return ApiResult<string>.Success(string.Empty);
        }

        private async Task<string> GetNextSubscriptionCodeAsync(CancellationToken ct)
        {
            var lastCode = await _db.Set<SubscriptionCustomer>()
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => x.SubscriptionCode)
                .FirstOrDefaultAsync(ct);

            if (int.TryParse(lastCode, out var lastValue) && lastValue >= 0)
                return (lastValue + 1).ToString();

            var allCodes = await _db.Set<SubscriptionCustomer>()
                .AsNoTracking()
                .Select(x => x.SubscriptionCode)
                .ToListAsync(ct);

            var maxNumeric = 0;
            foreach (var code in allCodes)
            {
                if (int.TryParse(code, out var parsed) && parsed > maxNumeric)
                    maxNumeric = parsed;
            }

            return (maxNumeric + 1).ToString();
        }
    }
}

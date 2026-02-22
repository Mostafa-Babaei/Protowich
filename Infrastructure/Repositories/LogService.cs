using Application.Interfaces;
using Domain.Entities.Logging;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Services
{
    public class LogService :   GenericRepository<ErrorLog>, ILogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task LogErrorAsync(ErrorLog log)
        {
            try
            {
                _context.Set<ErrorLog>().Add(log);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // اگر حتی لاگ خطا هم شکست بخورد، هیچ Exception جدیدی پرتاب نمی‌کنیم
            }
        }
    }
}

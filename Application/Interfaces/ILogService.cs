using Application.Common;
 
using Domain.Entities.Logging;

namespace Application.Interfaces
{
    public interface ILogService : IGenericRepository<ErrorLog>
    {
        Task LogErrorAsync(ErrorLog log);
    }
}

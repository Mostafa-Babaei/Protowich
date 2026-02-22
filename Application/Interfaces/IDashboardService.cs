using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Dashboard;

namespace Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardOverviewDto> GetOverviewAsync(ClaimsPrincipal user, CancellationToken ct);
    }
}

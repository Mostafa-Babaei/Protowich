using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboard
{
    public sealed class DashboardOverviewDto
    {
        public ServerTimeDto ServerTime { get; set; } = new();
        public WelcomeDto Welcome { get; set; } = new();

        public VisitorsStatsDto? VisitorsStats { get; set; }
        public UsersStatsDto? UsersStats { get; set; }

        public SpecializedServicesDto? Services { get; set; }

        public SmsBalanceDto? Sms { get; set; }

        public AttendanceTodayDto? AttendanceToday { get; set; }
    }

    public sealed class ServerTimeDto
    {
        public string Time { get; set; } = ""; // "HH:mm"
        public string Date { get; set; } = ""; // "yyyy-MM-dd"
    }

    public sealed class WelcomeDto
    {
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? RoleTitle { get; set; }
    }

    public sealed class VisitorsStatsDto
    {
        public int Total { get; set; }
        public int Today { get; set; }
        public int Last30Days { get; set; }
    }

    public sealed class UsersStatsDto
    {
        public int TotalUsers { get; set; }
    }

    public sealed class SpecializedServicesDto
    {
        public ServiceStatsDto BrainMap { get; set; } = new();
        public ServiceStatsDto PsychTest { get; set; } = new();
    }

    public sealed class ServiceStatsDto
    {
        public int Total { get; set; }
        public int Today { get; set; }
        public int Last30Days { get; set; }
    }

    public sealed class SmsBalanceDto
    {
        public double Balance { get; set; } // اگر خطا بود null
    }

    public sealed class AttendanceTodayDto
    {
        public string Status { get; set; } = "unknown"; // not_checked_in | working | done | unknown
        public string? CheckIn { get; set; }  // "HH:mm"
        public string? CheckOut { get; set; } // "HH:mm"
        public decimal? WorkHours { get; set; }
    }

}

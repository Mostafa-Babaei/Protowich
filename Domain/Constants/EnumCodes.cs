namespace Domain.Constants
{
    public static class EnumCodes
    {

        // ===================== Enum Category =====================
        public static class EnumCategory
        {
            public const int Gender = 1;
            public const int CareLevel = 2;
            public const int WorkTimeCalculationType = 3;
            public const int Relationship = 4;
            public const int MissionStatus = 5;
            public const int WeekDay = 6;
            public const int LeaveReason = 7;
            public const int ArchiveStatus = 8;
            public const int Title = 9;
            public const int Salutation = 10;
            public const int MessageType = 11;
            public const int SystemMessageSubType = 12;
            public const int PreRegistrationStatus = 13;
            public const int LeaveStatus = 14;
            public const int EmployeeType = 15;
            public const int CaregiverGenderPreference = 16;
            public const int ScheduleDuration = 17;
            public const int TodoStatus = 17;
        }

        // ===================== Gender =====================
        public static class Gender
        {
            public const string Male = "1";
            public const string Female = "2";
            public const string Other = "3";
        }

        // ===================== Care Level =====================
        public static class CareLevel
        {
            public const string Pflegegrad1 = "1";
            public const string Pflegegrad2 = "2";
            public const string Pflegegrad3 = "3";
            public const string Pflegegrad4 = "4";
            public const string Pflegegrad5 = "5";
        }

        // ===================== Work Time Calculation Type =====================
        public static class WorkTimeCalculationType
        {
            public const string ByMission = "1";
            public const string ByWorkStartEnd = "2";
            public const string ByMissionWithExtra = "3";
        }

        // ===================== Relationship =====================
        public static class Relationship
        {
            public const string Mother = "1";
            public const string Father = "2";
            public const string Spouse = "3";
            public const string Sibling = "4";
            public const string Child = "5";
        }

        // ===================== Mission Status =====================
        public static class MissionStatus
        {
            /// <summary>پیش‌نویس</summary>
            public const string Draft = "1";

            /// <summary>در حال انجام (شروع‌شده)</summary>
            public const string InProgress = "2";

            /// <summary>تکمیل‌شده</summary>
            public const string Completed = "3";

            /// <summary>لغوشده</summary>
            public const string Cancelled = "4";

            ///// <summary>برنامه‌ریزی‌شده</summary>
            //public const string Planned = "5";

            ///// <summary>در انتظار امضا</summary>
            //public const string AwaitingSignature = "6";

            ///// <summary>امضا‌شده</summary>
            //public const string Signed = "7";

            ///// <summary>دارای تأخیر</summary>
            //public const string Delayed = "8";
        }

        // ===================== Week Day =====================
        public static class WeekDay
        {
            public const string Monday = "1";
            public const string Tuesday = "2";
            public const string Wednesday = "3";
            public const string Thursday = "4";
            public const string Friday = "5";
            public const string Saturday = "6";
            public const string Sunday = "7";
        }

        // ===================== Leave Reason =====================
        public static class LeaveReason
        {
            public const string Vacation = "1";
            public const string Personal = "2";
            public const string SickLeave = "3";
        }

        // ===================== Archive Status =====================
        public static class ArchiveStatus
        {
            public const string Active = "0";
            public const string Leave = "1";
            public const string PassedAway = "2";
        }
        
        // ===================== Archive Status =====================
        public static class Title
        {
            public const string Dr = "1";
            public const string Prof = "2";
            public const string Prof_Dr = "3";
            public const string Dipl_Ing = "4";
            public const string B_Sc = "5";
            public const string M_Sc = "6";
            public const string Ph_D = "7";
        }

        // ===================== Archive Status =====================
        public static class Salutation
        {
            public const string Herr = "1";
            public const string Frau = "2";
        }

        // ===================== Message Type =====================
        public static class MessageType
        {
            public const string System = "1";      // پیام سیستمی
            public const string User = "2";        // پیام کاربر
            public const string Request = "3";     // درخواست‌ها (مثل مرخصی)
            public const string Alert = "4";       // هشدارها
            public const string Info = "5";        // پیام‌های اطلاع‌رسانی
        }

        // ===================== System Message SubType =====================
        public static class SystemMessageSubType
        {
            public const string MissionStarted = "1";       // شروع مأموریت
            public const string MissionDelayed = "2";       // تأخیر مأموریت
            public const string MissionEnded = "3";         // پایان مأموریت
            public const string AbsenceRequest = "4";       // درخواست مرخصی
            public const string BudgetExceeded = "5";       // هشدار اتمام بودجه
            public const string GeneralNotification = "6";  // اطلاع‌رسانی عمومی
            public const string SystemError = "7";          // خطای سیستمی
            public const string MissionApproved = "8";      // تأیید مأموریت
            public const string MissionRejected = "9";      // رد مأموریت
        }

        // ===================== Leave Status =====================
        public static class LeaveStatus
        {
            public const string Pending = "0";     // در انتظار تأیید
            public const string Approved = "1";    // تأیید شده
            public const string Rejected = "2";    // رد شده
            public const string Cancelled = "3";    // لغو شده
        }

        // ===================== Todo Status =====================
        public static class TodoStatus
        {
            public const string Open = "1";     //
            public const string InProgress = "2";    //
            public const string Done = "3";    //
        }

    }
}

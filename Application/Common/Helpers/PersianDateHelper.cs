using System.Globalization;

public static class PersianDateHelper
{
    private static readonly PersianCalendar _pc = new PersianCalendar();
    public static (DateTime from, DateTime toExclusive) GetCurrentJalaliMonthRange(DateTime now)
    {
        var pc = new PersianCalendar();

        var jy = pc.GetYear(now);
        var jm = pc.GetMonth(now);

        var from = pc.ToDateTime(jy, jm, 1, 0, 0, 0, 0);

        // ماه بعد
        int ny = jy;
        int nm = jm + 1;
        if (nm == 13) { nm = 1; ny++; }

        var to = pc.ToDateTime(ny, nm, 1, 0, 0, 0, 0);
        return (from.Date, to.Date);
    }
    public static string FaToEnDigits(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return s;

        return s
            .Replace('۰', '0').Replace('۱', '1').Replace('۲', '2').Replace('۳', '3').Replace('۴', '4')
            .Replace('۵', '5').Replace('۶', '6').Replace('۷', '7').Replace('۸', '8').Replace('۹', '9')
            .Replace('٠', '0').Replace('١', '1').Replace('٢', '2').Replace('٣', '3').Replace('٤', '4')
            .Replace('٥', '5').Replace('٦', '6').Replace('٧', '7').Replace('٨', '8').Replace('٩', '9');
    }
    public static string ToPersianDate(DateTime? date)
    {
        if (date == null)
            return string.Empty;

        return $"{_pc.GetYear(date.Value):0000}/" +
               $"{_pc.GetMonth(date.Value):00}/" +
               $"{_pc.GetDayOfMonth(date.Value):00}";
    }

    public static string ToPersianDateTime(DateTime? date)
    {

        if (date == null)
            return string.Empty;
        return $"{ToPersianDate(date)} " +
               $"{date.Value.Hour:00}:{date.Value.Minute:00}";
    }
    public static DateTime ParseJalaliDate(string? jalali)
    {
        if (string.IsNullOrWhiteSpace(jalali))
            throw new ArgumentException("Jalali date is required.");

        jalali = FaToEnDigits(jalali)?.Trim() ?? "";
        var s = jalali.Trim().Replace("-", "/");
        var parts = s.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3) throw new ArgumentException($"Invalid Jalali date: {jalali}");

        int jy = int.Parse(parts[0]);
        int jm = int.Parse(parts[1]);
        int jd = int.Parse(parts[2]);

        var pc = new PersianCalendar();
        // ساعت صفر
        return pc.ToDateTime(jy, jm, jd, 0, 0, 0, 0);
    }
}

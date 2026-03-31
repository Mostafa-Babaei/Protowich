function faToEnJs(str) {
    if (!str) return "";
    const fa = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹', '٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩'];
    const en = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    let s = String(str);
    for (let i = 0; i < fa.length; i++) s = s.replaceAll(fa[i], en[i]);
    return s;
}

/**
 * ورودی: "1404-11-08" یا "1404/11/08" (از persian-datepicker)
 * خروجی: "2026-01-28" (YYYY-MM-DD میلادی)
 */
function jalaliInputToGregorianIso(jalaliStr) {
    jalaliStr = faToEnJs((jalaliStr || "").trim());
    if (!jalaliStr) return null;

    // پشتیبانی از / و -
    const parts = jalaliStr.includes("/") ? jalaliStr.split("/") : jalaliStr.split("-");
    if (parts.length !== 3) return null;

    const jy = parseInt(parts[0], 10);
    const jm = parseInt(parts[1], 10);
    const jd = parseInt(parts[2], 10);
    if (!jy || !jm || !jd) return null;

    // persian-date: تاریخ جلالی -> میلادی
    const g = new persianDate([jy, jm, jd]).toCalendar("gregorian").format("YYYY-MM-DD");
    return g;
}

// =========================
// UI Helper (SweetAlert2 + fallback)
// =========================
window.Ui = (function () {
    function hasSwal() {
        return typeof window.Swal !== "undefined" && typeof window.Swal.fire === "function";
    }

    function normalizeIcon(type) {
        const t = String(type || "").toLowerCase();
        if (["success", "error", "warning", "info", "question"].includes(t)) return t;
        if (t === "danger") return "error";
        return "info";
    }

    function pickMessage(input, fallback) {
        // input می‌تواند string، Error، یا response api باشد
        if (!input) return fallback || "عملیات انجام نشد";
        if (typeof input === "string") return input;

        // ApiResult / Error object
        return (
            input.message ||
            input.title ||
            input.error ||
            input.data?.message ||
            input.data?.title ||
            input.data?.error ||
            fallback ||
            "عملیات انجام نشد"
        );
    }

    async function show(opts) {
        const o = opts || {};
        const icon = normalizeIcon(o.type || o.icon);
        const title = o.title || "";
        const text = pickMessage(o.text ?? o.message ?? o.data, o.fallback);

        if (hasSwal()) {
            const sw = {
                icon,
                title: title || (icon === "success" ? "موفق" : icon === "error" ? "خطا" : "پیام"),
                text,
                allowOutsideClick: o.allowOutsideClick ?? true,
                showConfirmButton: o.showConfirmButton ?? true,
                confirmButtonText: o.confirmText || "باشه",
                timer: o.timer || undefined,
                timerProgressBar: !!o.timer,
            };

            if (o.html != null) {
                delete sw.text;
                sw.html = o.html;
            }

            return await Swal.fire(sw);
        }

        // fallback
        if (window.toast) {
            toast(icon === "error" ? "error" : icon, title ? `${title}: ${text}` : text);
            return { isConfirmed: true };
        }

        alert(title ? `${title}\n${text}` : text);
        return { isConfirmed: true };
    }

    async function confirm(opts) {
        const o = opts || {};
        const title = o.title || "تأیید";
        const text = pickMessage(o.text ?? o.message, "آیا مطمئن هستید؟");

        if (hasSwal()) {
            const res = await Swal.fire({
                icon: normalizeIcon(o.type || o.icon) || "question",
                title,
                text,
                showCancelButton: true,
                confirmButtonText: o.confirmText || "بله",
                cancelButtonText: o.cancelText || "انصراف",
                reverseButtons: true,
                focusCancel: true,
                allowOutsideClick: false
            });
            return res.isConfirmed;
        }

        return window.confirm(text);
    }

    // شورتکات‌ها
    const success = (message, title, timer = 1500) => show({ type: "success", title, message, timer, showConfirmButton: false });
    const error = (message, title) => show({ type: "error", title, message });
    const warn = (message, title) => show({ type: "warning", title, message });
    const info = (message, title) => show({ type: "info", title, message });

    return { show, confirm, success, error, warn, info };
})();

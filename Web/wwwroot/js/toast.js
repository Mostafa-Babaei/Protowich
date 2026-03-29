(function () {
    if (!window.toastr) return;

    toastr.options = {
        closeButton: true,
        progressBar: true,
        newestOnTop: true,
        positionClass: "toast-top-left", // RTL-friendly
        timeOut: 3500,
        extendedTimeOut: 1500,
        showDuration: 200,
        hideDuration: 200,
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    };

    // 🔹 Wrapper سازگار با کدت
    window.toast = function (type, message, title) {
        switch (type) {
            case "success":
                toastr.success(message, title);
                break;
            case "error":
                toastr.error(message, title || "خطا");
                break;
            case "warning":
                toastr.warning(message, title);
                break;
            case "info":
            default:
                toastr.info(message, title);
                break;
        }
    };
})();

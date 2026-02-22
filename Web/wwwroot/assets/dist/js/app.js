
// function toast(type, message) {
//     Swal.fire({
//         toast: true,
//         position: "top-end",
//         showConfirmButton: false,
//         timer: 2500,
//         timerProgressBar: true,
//         icon: type, // success, error, warning, info
//         title: message
//     });
// }
function toast(type, message) {
    // SweetAlert2
    if (typeof Swal !== "undefined") {
        Swal.fire({
            icon: type,
            title: type === "success" ? "موفق" : "خطا",
            text: message,
            timer: 2500,
            showConfirmButton: false
        });
        return;
    }

    // SweetAlert v1
    if (typeof swal !== "undefined") {
        swal({
            title: type === "success" ? "موفق" : "خطا",
            text: message,
            icon: type,
            button: "باشه"
        });
        return;
    }

    // fallback
    alert(message);
}

window.confirmDialog = function ({
    title = "آیا مطمئن هستید؟",
    message = "",
    confirmText = "بله",
    cancelText = "انصراف",
    type = "warning"
} = {}) {

    // SweetAlert2
    if (typeof Swal !== "undefined" && typeof Swal.fire === "function") {
        return Swal.fire({
            title,
            text: message,
            icon: type,
            showCancelButton: true,
            confirmButtonText: confirmText,
            cancelButtonText: cancelText,
            reverseButtons: true
        }).then(r => r.isConfirmed);
    }

    // SweetAlert (old & new)
    if (typeof swal !== "undefined" && typeof swal === "function") {
        try {
            const ret = swal({
                title,
                text: message,
                icon: type,
                buttons: {
                    cancel: { text: cancelText, visible: true },
                    confirm: { text: confirmText, visible: true }
                },
                dangerMode: type === "warning"
            });

            if (ret && typeof ret.then === "function") return ret;

            // very old callback-style
            return new Promise(resolve => {
                swal({
                    title,
                    text: message,
                    icon: type,
                    buttons: [cancelText, confirmText],
                    dangerMode: type === "warning"
                }, ok => resolve(!!ok));
            });

        } catch (e) {
            return Promise.resolve(confirm(message || title));
        }
    }

    // fallback
    return Promise.resolve(confirm(message || title));
};


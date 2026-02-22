// includes/visitors/main.js

/**
 * نمایش پیام toast
 */
function showToast(type, message) {
    const icons = {
        success: 'check-circle',
        error: 'exclamation-circle',
        warning: 'exclamation-triangle',
        info: 'info-circle'
    };

    if (typeof Toastify !== 'undefined') {
        Toastify({
            text: message,
            duration: 3000,
            gravity: "top",
            position: "left",
            backgroundColor: type === 'success' ? "#28a745" :
                type === 'error' ? "#dc3545" :
                    type === 'warning' ? "#ffc107" : "#17a2b8",
            stopOnFocus: true,
            className: "toast-notification",
            avatar: `<i class="fas fa-${icons[type]}"></i>`
        }).showToast();
    } else if (typeof Swal !== 'undefined') {
        Swal.fire({
            icon: type,
            title: message,
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
        });
    } else {
        alert(message);
    }
}

// مستعار برای backward compatibility
const toast = showToast;

/**
 * تبدیل تاریخ به شمسی
 */
function toPersian(dateString) {
    if (!dateString) return "";
    try {
        if (typeof persianDate !== 'undefined') {
            return new persianDate(new Date(dateString)).format("YYYY/MM/DD");
        }
        return new Date(dateString).toLocaleDateString('fa-IR');
    } catch (e) {
        return dateString;
    }
}

/**
 * رندر تاریخ‌های جلالی
 */
function renderJalaliDates(container = document) {
    $(container).find('.jalali-date').each(function () {
        const gDate = $(this).data('date');
        if (!gDate) return;
        $(this).text(toPersian(gDate));
    });
}

/**
 * بارگذاری اولیه
 */
$(document).ready(function () {
    // بارگذاری مراجعین
    loadVisitors();

    // رفرش جدول بعد از بسته شدن مدال‌ها
    $(document).on('hidden.bs.modal', '#visitorVisitsModal, #linksModal, #linkModal', function () {
        loadVisitors();
    });

    // رویدادهای جستجو
    $('#btnSearch').on('click', () => loadVisitors(1));
    $('#txtSearch').on('keyup', (e) => {
        if (e.key === 'Enter') loadVisitors(1);
    });

    console.log('Visitors module loaded successfully');
});
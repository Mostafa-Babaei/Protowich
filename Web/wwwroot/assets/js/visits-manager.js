// includes/visitors/visits-manager.js

/**
 * باز کردن مدال مراجعات
 */
function openVisitsModal(visitorId) {
    currentVisitorId = visitorId;
    $('#visitorVisitsModal').modal('show');
    loadVisits();
    loadFollowups();

    if ($.fn.persianDatepicker) {
        $('#visit_date').persianDatepicker({
            format: 'YYYY-MM-DD',
            autoClose: true,
            initialValue: false
        });
        $('#next_visit_date').persianDatepicker({
            format: 'YYYY-MM-DD',
            autoClose: true,
            initialValue: false
        });
    }
}

/**
 * بارگذاری مراجعات
 */
function loadVisits() {
    fetch(`${BASE_URL}/public/ajax/visitor_visits.php?visitor_id=${currentVisitorId}`)
        .then(res => res.json())
        .then(data => {
            const tbody = document.getElementById('visitsTableBody');
            tbody.innerHTML = '';

            if (!Array.isArray(data) || data.length === 0) {
                tbody.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center text-muted">
                        مراجعه‌ای ثبت نشده است
                    </td>
                </tr>`;
                return;
            }

            let html = '';
            data.forEach((row, index) => {
                html += `
                <tr>
                    <td>${index + 1}</td>
                    <td>${row.visit_date_jalali || '-'}</td>
                    <td>${row.treatment_status_label ?? '-'}</td>
                    <td>${row.description ?? '-'}</td>
                    <td>
                        <div class="btn-group btn-group-sm">
                            <button class="btn btn-danger" onclick="deleteVisit(${row.id})" title="حذف">
                                <i class="fa fa-trash"></i>
                            </button>
                            <button class="btn btn-secondary" 
                                    onclick="openTreatmentStatusModal(${row.id}, '${row.treatment_status}', '${row.description}')"
                                    title="وضعیت درمان">
                                <i class="fa fa-stethoscope"></i>
                            </button>
                        </div>
                    </td>
                </tr>`;
            });

            tbody.innerHTML = html;
        });
}

/**
 * افزودن مراجعه
 */
function addVisit() {
    const visitDate = document.getElementById('visit_date').value;
    if (!visitDate) {
        showToast("error", 'تاریخ مراجعه را وارد کنید');
        return;
    }

    const desc = document.getElementById('visit_description').value;

    fetch(`${BASE_URL}/public/ajax/visitor_visits.php`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            visitor_id: currentVisitorId,
            visit_date: visitDate,
            treatment_status: $('#visit_status').val(),
            description: $('#visit_description').val()
        })
    })
        .then(res => res.json())
        .then(res => {
            if (res.success) {
                document.getElementById('visit_description').value = '';
                loadVisits();
                showToast("success", 'تاریخ مراجعه با موفقیت ثبت شد');
                $('#visitorVisitsModal').modal('hide');
            } else {
                showToast("error", res.message || 'خطا در ثبت مراجعه');
            }
        });
}

/**
 * حذف مراجعه
 */
function deleteVisit(id) {
    Swal.fire({
        title: 'حذف تاریخ مراجعه',
        text: 'آیا از حذف این تاریخ مراجعه اطمینان دارید؟ این عملیات قابل بازگشت نیست.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، حذف شود',
        cancelButtonText: 'انصراف',
        confirmButtonColor: '#d33',
        reverseButtons: true
    }).then((result) => {
        if (!result.isConfirmed) return;

        fetch(`${BASE_URL}/public/ajax/visitor_visits.php`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ visit_id: id })
        })
            .then(res => res.json())
            .then(res => {
                if (res.success) {
                    showToast('success', 'تاریخ مراجعه با موفقیت حذف شد');
                    loadVisits();
                } else {
                    showToast('error', res.message);
                }
            })
            .catch(() => {
                showToast('error', 'خطا در ارتباط با سرور');
            });
    });
}

/**
 * ارسال یادآوری
 */
function sendReminder(visitId) {
    Swal.fire({
        title: 'ارسال پیامک یادآوری',
        text: 'آیا از ارسال پیامک یادآوری برای این مراجعه اطمینان دارید؟',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'بله، ارسال شود',
        cancelButtonText: 'انصراف',
        reverseButtons: true
    }).then((result) => {
        if (!result.isConfirmed) return;

        fetch(`${BASE_URL}/public/ajax/send_visit_reminder.php`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ visit_id: visitId })
        })
            .then(res => res.json())
            .then(res => {
                if (res.success) {
                    showToast('success', 'پیامک یادآوری ارسال شد');
                } else {
                    showToast('error', res.message);
                }
            })
            .catch(() => {
                showToast('error', 'خطا در ارتباط با سرور');
            });
    });
}
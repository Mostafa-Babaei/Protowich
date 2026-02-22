//followups-manager.js

/**
 * بارگذاری پیگیری‌ها
 */
function loadFollowups() {
    fetch(`${BASE_URL}/public/ajax/visitor_followups.php?visitor_id=${currentVisitorId}`)
        .then(res => res.json())
        .then(rows => {
            const tbody = document.getElementById('followupsTableBody');
            tbody.innerHTML = '';

            if (!Array.isArray(rows) || rows.length === 0) {
                tbody.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center text-muted">
                        مراجعه بعدی ثبت نشده است
                    </td>
                </tr>`;
                return;
            }

            let html = '';
            rows.forEach((r, i) => {
                const statusBadge =
                    r.status === 'sent' ? 'success' :
                        r.status === 'cancelled' ? 'danger' :
                            r.status === 'scheduled' ? 'warning' :
                                'secondary';

                const statusTitle =
                    r.status === 'sent' ? 'مراجعه شد' :
                        r.status === 'cancelled' ? 'لغو' :
                            r.status === 'scheduled' ? 'در انتظار' :
                                'نامشخص';

                html += `
                <tr>
                    <td>${i + 1}</td>
                    <td>${r.followup_date_jalali}</td>
                    <td>
                        <span class="badge bg-${statusBadge}">
                            ${statusTitle}
                        </span>
                    </td>
                    <td>
                        ${r.status === 'scheduled'
                        ? `<button class="btn btn-sm btn-danger"
                                       onclick="cancelFollowup(${r.id})">
                                <i class="fa fa-times"></i>
                              </button>`
                        : ''
                    }
                    </td>
                </tr>`;
            });

            tbody.innerHTML = html;
        });
}

/**
 * لغو پیگیری
 */
function cancelFollowup(id) {
    Swal.fire({
        title: 'لغو مراجعه بعدی',
        text: 'آیا از لغو این مراجعه اطمینان دارید؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، لغو شود',
        cancelButtonText: 'انصراف'
    }).then(r => {
        if (!r.isConfirmed) return;

        fetch(`${BASE_URL}/public/ajax/visitor_followups.php`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id })
        })
            .then(res => res.json())
            .then(j => {
                if (j.success) {
                    showToast('success', 'مراجعه بعدی لغو شد');
                    loadFollowups();
                } else {
                    showToast('error', j.message);
                }
            });
    });
}

/**
 * ذخیره مراجعه بعدی
 */
function saveNextVisit() {
    const visitDate = document.getElementById('next_visit_date').value;

    if (!visitDate) {
        showToast("error", 'تاریخ مراجعه را وارد کنید');
        return;
    }

    fetch(`${BASE_URL}/public/ajax/visitor_next_visit.php`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            visitor_id: currentVisitorId,
            next_visit_date: visitDate
        })
    })
        .then(r => r.json())
        .then(j => {
            if (j.success) {
                loadFollowups();
                showToast('success', 'مراجعه بعدی ثبت و پیامک یادآوری ثبت شد');
            } else {
                showToast('error', j.message);
            }
        });
}
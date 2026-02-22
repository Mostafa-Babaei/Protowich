// includes/visitors/treatment-status.js

/**
 * باز کردن مدال وضعیت درمان
 */
function openTreatmentStatusModal(visitId, currentStatus, description) {
    $('#tsVisitId').val(visitId);
    $('#tsStatus').val(currentStatus || '');
    $('#tsDescription').val(description || '');
    $('#treatmentStatusModal').modal('show');
}

/**
 * ذخیره وضعیت درمان
 */
function saveTreatmentStatus() {
    fetch(`${BASE_URL}/public/ajax/visitor_treatment_status.php`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            visit_id: $('#tsVisitId').val(),
            status: $('#tsStatus').val(),
            description: $('#tsDescription').val()
        })
    })
        .then(r => r.json())
        .then(j => {
            if (j.success) {
                $('#treatmentStatusModal').modal('hide');
                showToast('success', 'وضعیت درمان ثبت شد');
                loadVisits();
            } else {
                showToast('error', j.message);
            }
        });
}
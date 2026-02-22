// includes/visitors/links-manager.js

/**
 * نمایش لینک‌های مراجعه‌کننده
 */
function showLinks(visitorId, name) {
    if (!CAN_EDIT_LINKS) {
        showToast('error', 'شما دسترسی لازم برای مشاهده لینک را ندارید');
        return;
    }

    currentVisitorId = visitorId;
    currentVisitorName = name;
    $('#linksTitle').text('لینک‌های مرتبط ' );
    $('#linksContent').html('در حال بارگذاری...');
    $('#linksModal').modal('show');

    $.get(`${BASE_URL}/public/ajax/visitor_links_list.php`, {
        visitor_id: visitorId
    }, function (html) {
        $('#linksContent').html(html);
        renderJalaliDates('#linksContent');
    });
}

/**
 * باز کردن مدال افزودن/ویرایش لینک
 */
function openLinkModal(visitorId) {
    if (!CAN_EDIT_LINKS) {
        showToast('error', 'شما دسترسی لازم برای مشاهده لینک را ندارید');
        return;
    }

    isEditingLink = false;
    loadLinkTypes();
    document.getElementById('linkId').value = '';
    document.getElementById('linkVisitorId').value = visitorId;
    document.getElementById('linkType').value = '';
    document.getElementById('linkUrl').value = '';
    $('#linkModal').modal('show');
}

/**
 * ویرایش لینک
 */
function editLink(id, title, url) {
    if (!CAN_EDIT_LINKS) {
        showToast('error', 'شما دسترسی لازم برای مشاهده لینک را ندارید');
        return;
    }

    isEditingLink = true;
    $('#linksModal').modal('hide');

    setTimeout(() => {
        document.getElementById('linkId').value = id;
        document.getElementById('linkVisitorId').value = '';
        document.getElementById('linkTitle').value = title;
        document.getElementById('linkUrl').value = url;
        $('#linkModal').modal('show');
    }, 300);
}

/**
 * بارگذاری انواع لینک
 */
function loadLinkTypes() {
    if (LINK_TYPES.length) return;

    fetch(`${BASE_URL}/public/ajax/visitor_link_types.php`)
        .then(r => r.json())
        .then(j => {
            if (!j.success) return;
            LINK_TYPES = j.data;
            const select = document.getElementById('linkType');
            select.innerHTML = '<option value="">انتخاب نوع لینک</option>';
            LINK_TYPES.forEach(t => {
                select.innerHTML += `<option value="${t.id}">${t.title}</option>`;
            });
        });
}

/**
 * ذخیره لینک
 */
function saveVisitorLink() {
    const linkId = document.getElementById('linkId').value;
    const visitorId = document.getElementById('linkVisitorId').value;
    const linkTypeId = document.getElementById('linkType').value;
    const url = document.getElementById('linkUrl').value.trim();

    // اعتبارسنجی
    if (!linkTypeId) {
        showToast('error', 'لطفاً نوع لینک را انتخاب کنید');
        return;
    }

    if (!url) {
        showToast('error', 'آدرس لینک الزامی است');
        return;
    }

    const isEdit = linkId !== '';
    const apiUrl = isEdit ?
        `${BASE_URL}/public/ajax/visitor_links_update.php` :
        `${BASE_URL}/public/ajax/visitor_links_add.php`;

    const payload = isEdit ? {
        id: linkId,
        link_type_id: linkTypeId,
        url: url
    } : {
        visitor_id: visitorId,
        link_type_id: linkTypeId,
        url: url
    };

    fetch(apiUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
        .then(r => r.json())
        .then(j => {
            if (!j.success) {
                showToast('error', j.message || 'خطا در ثبت لینک');
                return;
            }

            $('#linkModal').modal('hide');
            showToast('success', isEdit ? 'لینک با موفقیت ویرایش شد' : 'لینک با موفقیت ثبت شد');

            setTimeout(() => {
                showLinks(currentVisitorId, currentVisitorName);
            }, 300);
        })
        .catch(err => {
            console.error(err);
            showToast('error', 'خطا در ارتباط با سرور');
        });
}

/**
 * حذف لینک
 */
function deleteLink(id) {
    if (!CAN_EDIT_LINKS) {
        showToast('error', 'شما دسترسی لازم برای مشاهده لینک را ندارید');
        return;
    }

    Swal.fire({
        title: 'حذف لینک؟',
        text: 'آیا مطمئن هستید؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، حذف شود',
        cancelButtonText: 'انصراف'
    }).then(result => {
        if (!result.isConfirmed) return;

        fetch(`${BASE_URL}/public/ajax/visitor_links_delete.php`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id })
        })
            .then(r => r.json())
            .then(j => {
                if (j.success) {
                    showToast('success', 'لینک حذف شد');
                    $('#linksModal').modal('hide');
                    showLinks(currentVisitorId, currentVisitorName);
                } else {
                    showToast('error', j.message || 'خطا');
                }
            });
    });
}

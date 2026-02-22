// includes/visitors/visitors-crud.js

/**
 * افزودن مراجعه‌کننده جدید
 */
function addVisitor() {
    const form = document.getElementById("visitorForm");
    const data = {
        full_name: form.full_name.value,
        mobile: form.mobile.value,
        mobile2: form.mobile2.value,
        national_id: form.national_id.value
    };

    // اعتبارسنجی
    if (!data.full_name || !data.mobile) {
        showToast('error', 'نام و موبایل الزامی هستند');
        return;
    }

    fetch("visitor_add.php", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    })
        .then(res => res.text())
        .then(text => {
            let json;
            try {
                json = JSON.parse(text);
            } catch (e) {
                console.error("JSON Parse Error:", text);
                return showToast("error", "پاسخ سرور نامعتبر است");
            }

            if (json.success) {
                $("#visitorModal").modal("hide");
                loadVisitors(currentPage);
                showToast("success", json.message);
                form.reset();
            } else {
                showToast("error", json.message || "خطایی رخ داد");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            showToast("error", "خطا در ارتباط با سرور");
        });
}

/**
 * ویرایش مراجعه‌کننده
 */
function editVisitor(id) {
    fetch("visitor_get.php?id=" + id)
        .then(res => res.text())
        .then(text => {
            try {
                const json = JSON.parse(text);

                if (!json.success) {
                    return showToast("error", json.message || "خطا در دریافت اطلاعات");
                }

                let f = document.getElementById("editVisitorForm");
                f.id.value = json.data.id;
                f.full_name.value = json.data.full_name;
                f.mobile.value = json.data.mobile;
                f.mobile2.value = json.data.mobile2 || "";
                f.national_id.value = json.data.national_id;

                $("#editVisitorModal").modal("show");
            } catch (e) {
                console.error("Response was not JSON:", text);
                showToast("error", "خطای غیرمنتظره در سرور");
            }
        });
}

/**
 * بروزرسانی مراجعه‌کننده
 */
function updateVisitor() {
    const f = document.getElementById("editVisitorForm");
    const data = {
        id: f.id.value,
        full_name: f.full_name.value,
        mobile: f.mobile.value,
        mobile2: f.mobile2.value,
        national_id: f.national_id.value
    };

    fetch("visitor_edit.php", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    })
        .then(res => res.json())
        .then(json => {
            if (json.success) {
                $("#editVisitorModal").modal("hide");
                showToast("success", "ویرایش با موفقیت انجام شد");
                loadVisitors(currentPage);
            } else {
                showToast("error", json.message);
            }
        });
}

/**
 * حذف مراجعه‌کننده
 */
function deleteVisitor(id) {
    Swal.fire({
        title: 'حذف مراجعه‌کننده؟',
        text: "آیا مطمئن هستید؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، حذف شود',
        cancelButtonText: 'انصراف'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch("visitor_delete.php", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ id })
            })
                .then(res => res.json())
                .then(json => {
                    if (json.success) {
                        showToast("success", "با موفقیت حذف شد");
                        loadVisitors(currentPage);
                    } else {
                        showToast("error", json.message || "خطا در حذف");
                    }
                });
        }
    });
}

// اتصال رویدادها
document.addEventListener('DOMContentLoaded', function () {
    const btnSaveVisitor = document.getElementById("btnSaveVisitor");
    const btnUpdateVisitor = document.getElementById("btnUpdateVisitor");

    if (btnSaveVisitor) {
        btnSaveVisitor.onclick = addVisitor;
    }

    if (btnUpdateVisitor) {
        btnUpdateVisitor.onclick = updateVisitor;
    }
});
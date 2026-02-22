// includes/visitors/visitors-table.js

/**
 * بارگذاری لیست مراجعین
 */
function loadVisitors(page = 1) {
    currentPage = page;
    const search = document.getElementById("txtSearch").value;

    fetch(`visitors_list_data.php?page=${page}&search=${encodeURIComponent(search)}`)
        .then(res => res.json())
        .then(json => {
            renderTable(json.data);
            renderPagination(json.total, page);
        })
        .catch(error => {
            showToast('error', 'خطا در دریافت اطلاعات');
            console.error('Error loading visitors:', error);
        });
}

/**
 * رندر جدول مراجعین
 */
function renderTable(rows) {
    let html = `
    <table class="table table-hover text-nowrap">
        <thead class="table-dark">
            <tr>
                <th>شماره پرونده</th>
                <th>نام</th>
                <th>موبایل</th>
                <th>کد ملی</th>
                <th>وضعیت درمان</th>
                <th>مراجعات</th>
                <th>لینک ها</th>
                <th>عملیات</th>
            </tr>
        </thead>
        <tbody>`;

    rows.forEach((r, i) => {
        const mobileCombined = r.mobile2 ? `${r.mobile} - ${r.mobile2}` : r.mobile;

        html += `
        <tr>
            <td>${r.doc_number}</td>
            <td>${r.full_name}</td>
            <td>${mobileCombined}</td>
            <td>${r.national_id}</td>
            <td>
                <span class="badge 
                    ${r.treatment_status === 'completed' ? 'bg-success' :
                r.treatment_status === 'cancelled' ? 'bg-danger' :
                    r.treatment_status === 'in_treatment' ? 'bg-warning' :
                        'bg-secondary'}">
                    ${r.treatment_status_label}
                </span>
            </td>
            <td>
                ${r.visits_count > 0
                ? `<button class="btn btn-sm btn-info" onclick="openVisitsModal(${r.id})">
                        ${r.visits_count} <i class="fa fa-calendar mx-2"></i>
                      </button>`
                : `<button class="btn btn-sm btn-info" onclick="openVisitsModal(${r.id})">
                        <i class="fa fa-calendar mx-2"></i>
                      </button>`
            }
            </td>
            <td>
                ${r.links_count > 0
                ? `<button class="btn btn-sm btn-info" onclick="showLinks(${r.id}, '${r.full_name}')">
                        ${r.links_count} <i class="fa fa-link mx-2"></i>
                      </button>`
                : ''
            }
            </td>
            <td>
                <div class="btn-group btn-group-sm" role="group">
                    ${CAN_EDIT_VISITORS
                ? `<button class="btn btn-warning" onclick="editVisitor(${r.id})" title="ویرایش">
                            <i class="fa fa-edit"></i>
                          </button>`
                : ''
            }
                    ${CAN_DELETE_VISITORS
                ? `<button class="btn btn-danger" onclick="deleteVisitor(${r.id})" title="حذف">
                            <i class="fa fa-trash"></i>
                          </button>`
                : ''
            }
                    ${CAN_EDIT_LINKS
                ? `<button class="btn btn-info" onclick="openLinkModal(${r.id})" title="افزودن لینک">
                            <i class="fa fa-link"></i>
                          </button>`
                : ''
            }
                </div>
            </td>
        </tr>`;
    });

    html += "</tbody></table>";
    document.getElementById("tableContainer").innerHTML = html;
}

/**
 * رندر صفحه‌بندی
 */
function renderPagination(total, page) {
    const limit = 10;
    const pages = Math.ceil(total / limit);

    if (pages <= 1) {
        document.getElementById("pagination").innerHTML = "";
        return;
    }

    let html = "";
    const prevPage = page > 1 ? page - 1 : 1;
    const nextPage = page < pages ? page + 1 : pages;
    const start = Math.max(1, page - 3);
    const end = Math.min(pages, page + 3);

    html += `
        <li class="page-item ${page === 1 ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="loadVisitors(1)">«</a>
        </li>
        <li class="page-item ${page === 1 ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="loadVisitors(${prevPage})">‹</a>
        </li>`;

    for (let p = start; p <= end; p++) {
        html += `
            <li class="page-item ${p === page ? 'active' : ''}">
                <a class="page-link" href="#" onclick="loadVisitors(${p})">${p}</a>
            </li>`;
    }

    html += `
        <li class="page-item ${page === pages ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="loadVisitors(${nextPage})">›</a>
        </li>
        <li class="page-item ${page === pages ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="loadVisitors(${pages})">»</a>
        </li>`;

    document.getElementById("pagination").innerHTML = html;
}
// در بخش <script> داشبورد اضافه شود:

// ساعت زنده
function updateLiveClock() {
    const now = new Date();
    const timeString = now.toLocaleTimeString('fa-IR');
    const dateString = now.toLocaleDateString('fa-IR');

    $('#liveClock').text(timeString);
    $('#liveDate').text(dateString);
}
setInterval(updateLiveClock, 1000);
updateLiveClock();

// ثبت ورود
function checkIn() {
    Swal.fire({
        title: 'ثبت ورود',
        html: `
            <div class="text-center">
                <i class="fas fa-sign-in-alt fa-3x text-success mb-3"></i>
                <p>آیا می‌خواهید ورود خود را ثبت کنید؟</p>
                <div class="alert alert-info">
                    <i class="fas fa-clock"></i> ساعت سیستم: <span id="currentTime">${new Date().toLocaleTimeString('fa-IR')}</span>
                </div>
                <div class="form-group">
                    <label for="checkinNote">یادداشت (اختیاری)</label>
                    <textarea id="checkinNote" class="form-control" rows="2" placeholder="مثال: جلسه با تیم..."></textarea>
                </div>
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'بله، ثبت کن',
        cancelButtonText: 'انصراف',
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#6c757d',
        reverseButtons: true,
        preConfirm: () => {
            return {
                note: $('#checkinNote').val()
            };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${BASE_URL}/public/ajax/attendance.php`,
                method: 'POST',
                data: {
                    action: 'checkin',
                    note: result.value.note
                },
                beforeSend: () => {
                    Swal.showLoading();
                },
                success: (response) => {
                    if (response.success) {
                        Swal.fire({
                            title: 'موفق',
                            text: 'ورود شما با موفقیت ثبت شد',
                            icon: 'success',
                            confirmButtonText: 'باشه'
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire('خطا', response.message || 'خطایی رخ داد', 'error');
                    }
                },
                error: () => {
                    Swal.fire('خطا', 'خطا در ارتباط با سرور', 'error');
                }
            });
        }
    });
}

// ثبت خروج
function checkOut() {
    Swal.fire({
        title: 'ثبت خروج',
        html: `
            <div class="text-center">
                <i class="fas fa-sign-out-alt fa-3x text-danger mb-3"></i>
                <p>کار امروز رو تموم کردید؟</p>
                <div class="alert alert-warning">
                    <i class="fas fa-clock"></i> ساعت سیستم: <span id="currentTime">${new Date().toLocaleTimeString('fa-IR')}</span>
                </div>
                <div class="form-group">
                    <label for="checkoutNote">خلاصه فعالیت‌های امروز (اختیاری)</label>
                    <textarea id="checkoutNote" class="form-control" rows="3" placeholder="پروژه‌های انجام شده..."></textarea>
                </div>
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'بله، خروج ثبت شود',
        cancelButtonText: 'انصراف',
        confirmButtonColor: '#dc3545',
        cancelButtonColor: '#6c757d',
        reverseButtons: true,
        preConfirm: () => {
            return {
                note: $('#checkoutNote').val()
            };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${BASE_URL}/public/ajax/attendance.php`,
                method: 'POST',
                data: {
                    action: 'checkout',
                    note: result.value.note
                },
                beforeSend: () => {
                    Swal.showLoading();
                },
                success: (response) => {
                    if (response.success) {
                        Swal.fire({
                            title: 'موفق',
                            html: `
                                <div class="text-center">
                                    <i class="fas fa-check-circle fa-3x text-success mb-3"></i>
                                    <p>خروج شما ثبت شد</p>
                                    <div class="alert alert-success">
                                        <h5>خلاصه امروز:</h5>
                                        <p>${response.summary}</p>
                                    </div>
                                </div>
                            `,
                            confirmButtonText: 'متشکرم'
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire('خطا', response.message || 'خطایی رخ داد', 'error');
                    }
                }
            });
        }
    });
}

// گزارش حضور و غیاب
function showAttendanceReport() {
    window.open('attendance_report.php', '_blank');
}

// ثبت استراحت
function showBreakModal() {
    Swal.fire({
        title: 'ثبت زمان استراحت',
        html: `
            <div class="form-group">
                <label>نوع استراحت</label>
                <select class="form-control" id="breakType">
                    <option value="coffee">قهوه/چای</option>
                    <option value="lunch">ناهار</option>
                    <option value="prayer">نماز</option>
                    <option value="other">سایر</option>
                </select>
            </div>
            <div class="form-group">
                <label>مدت زمان (دقیقه)</label>
                <input type="number" id="breakDuration" class="form-control" value="15" min="1" max="120">
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'ثبت استراحت',
        cancelButtonText: 'انصراف'
    }).then((result) => {
        if (result.isConfirmed) {
            // ثبت در localStorage یا ارسال به سرور
            const breakLog = {
                type: $('#breakType').val(),
                duration: $('#breakDuration').val(),
                timestamp: new Date().toISOString()
            };

            let breaks = JSON.parse(localStorage.getItem('dailyBreaks') || '[]');
            breaks.push(breakLog);
            localStorage.setItem('dailyBreaks', JSON.stringify(breaks));

            Swal.fire('موفق', 'زمان استراحت ثبت شد', 'success');
        }
    });
}
// دریافت زمان سرور و به‌روزرسانی صفحه
function getServerTime() {
    $.ajax({
        url: `${BASE_URL}/public/ajax/attendance.php`,
        method: 'POST',
        data: { action: 'get_current_time' },
        success: function (response) {
            if (response.success) {
                // نمایش زمان سرور
                const serverTime = new Date(response.time);
                updateServerClock(serverTime);

                // بروزرسانی پیش‌نمایش زمان ثبت
                updateTimePreviews(serverTime);
            }
        }
    });
}

// به‌روزرسانی ساعت سرور در صفحه
function updateServerClock(serverTime) {
    const timeStr = serverTime.toLocaleTimeString('fa-IR', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    });

    const dateStr = serverTime.toLocaleDateString('fa-IR', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });

    $('#serverClock').text(timeStr);
    $('#serverDate').text(dateStr);
}

// بروزرسانی پیش‌نمایش زمان‌های ثبت
function updateTimePreviews(serverTime) {
    const timeOnly = serverTime.toLocaleTimeString('fa-IR', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    });

    $('#checkinTimePreview').text(timeOnly);
    $('#checkoutTimePreview').text(timeOnly);
}

// محاسبه زمان سپری شده از ورود
// function updateElapsedTime() {
//     const checkinTime = '<?= $todayRecord['check_in'] ?? null ?>';
//     if (checkinTime) {
//         $.ajax({
//             url: `${BASE_URL}/public/ajax/attendance.php`,
//             method: 'POST',
//             data: { action: 'get_current_time' },
//             success: function (response) {
//                 if (response.success) {
//                     const serverTime = new Date(response.time);
//                     const checkinTimestamp = new Date(checkinTime);
//                     const elapsedMs = serverTime - checkinTimestamp;

//                     const hours = Math.floor(elapsedMs / 3600000);
//                     const minutes = Math.floor((elapsedMs % 3600000) / 60000);

//                     $('#elapsedTime').text(`${hours} ساعت و ${minutes} دقیقه`);
//                 }
//             }
//         });
//     }
// }

// ثبت ورود با زمان سرور
function checkInWithServerTime() {
    // ابتدا زمان سرور رو بگیریم
    $.ajax({
        url: `${BASE_URL}/public/ajax/attendance.php`,
        method: 'POST',
        data: { action: 'get_current_time' },
        success: function (timeResponse) {
            if (timeResponse.success) {
                const serverTime = new Date(timeResponse.time);
                const timeStr = serverTime.toLocaleTimeString('fa-IR', {
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: false
                });

                const dateStr = serverTime.toLocaleDateString('fa-IR');

                Swal.fire({
                    title: 'ثبت ورود',
                    html: `
                        <div class="text-center">
                            <i class="fas fa-sign-in-alt fa-3x text-success mb-3"></i>
                            <p>آیا می‌خواهید ورود خود را ثبت کنید؟</p>
                            <div class="alert alert-success">
                                <i class="fas fa-server mr-2"></i>
                                <strong>زمان سرور:</strong><br>
                                ${dateStr} ساعت ${timeStr}
                            </div>
                            <div class="form-group mt-3">
                                <label for="checkinNote">یادداشت (اختیاری)</label>
                                <textarea id="checkinNote" class="form-control" rows="2" 
                                          placeholder="توضیحات اضافه..."></textarea>
                            </div>
                        </div>
                    `,
                    showCancelButton: true,
                    confirmButtonText: 'بله، ثبت کن',
                    cancelButtonText: 'انصراف',
                    confirmButtonColor: '#28a745',
                    cancelButtonColor: '#6c757d',
                    reverseButtons: true,
                    preConfirm: () => {
                        return {
                            note: $('#checkinNote').val()
                        };
                    }
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: `${BASE_URL}/public/ajax/attendance.php`,
                            method: 'POST',
                            data: {
                                action: 'checkin',
                                note: result.value.note
                            },
                            beforeSend: () => {
                                Swal.showLoading();
                            },
                            success: (response) => {
                                if (response.success) {
                                    Swal.fire({
                                        title: 'موفق',
                                        text: 'ورود شما با موفقیت ثبت شد',
                                        icon: 'success',
                                        confirmButtonText: 'باشه'
                                    }).then(() => {
                                        location.reload();
                                    });
                                } else {
                                    Swal.fire('خطا', response.message || 'خطایی رخ داد', 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
    });
}

// ثبت خروج با زمان سرور
function checkOutWithServerTime() {
    // ابتدا زمان سرور رو بگیریم
    $.ajax({
        url: `${BASE_URL}/public/ajax/attendance.php`,
        method: 'POST',
        data: { action: 'get_current_time' },
        success: function (timeResponse) {
            if (timeResponse.success) {
                const serverTime = new Date(timeResponse.time);
                const timeStr = serverTime.toLocaleTimeString('fa-IR', {
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: false
                });

                Swal.fire({
                    title: 'ثبت خروج',
                    html: `
                        <div class="text-center">
                            <i class="fas fa-sign-out-alt fa-3x text-danger mb-3"></i>
                            <p>کار امروز رو تموم کردید؟</p>
                            <div class="alert alert-success">
                                <i class="fas fa-server mr-2"></i>
                                <strong>زمان سرور:</strong> ${timeStr}
                            </div>
                            <div class="form-group mt-3">
                                <label for="checkoutNote">خلاصه فعالیت‌های امروز (اختیاری)</label>
                                <textarea id="checkoutNote" class="form-control" rows="3" 
                                          placeholder="پروژه‌های انجام شده..."></textarea>
                            </div>
                        </div>
                    `,
                    showCancelButton: true,
                    confirmButtonText: 'بله، خروج ثبت شود',
                    cancelButtonText: 'انصراف',
                    confirmButtonColor: '#dc3545',
                    cancelButtonColor: '#6c757d',
                    reverseButtons: true,
                    preConfirm: () => {
                        return {
                            note: $('#checkoutNote').val()
                        };
                    }
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: `${BASE_URL}/public/ajax/attendance.php`,
                            method: 'POST',
                            data: {
                                action: 'checkout',
                                note: result.value.note
                            },
                            beforeSend: () => {
                                Swal.showLoading();
                            },
                            success: (response) => {
                                if (response.success) {
                                    Swal.fire({
                                        title: 'موفق',
                                        text: 'خروج شما با موفقیت ثبت شد',
                                        icon: 'success',
                                        confirmButtonText: 'باشه'
                                    }).then(() => {
                                        location.reload();
                                    });
                                } else {
                                    Swal.fire('خطا', response.message || 'خطایی رخ داد', 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
    });
}

// ثبت استراحت
function showBreakModal() {
    Swal.fire({
        title: 'ثبت زمان استراحت',
        html: `
            <div class="form-group">
                <label>مدت زمان استراحت (دقیقه)</label>
                <input type="number" id="breakDuration" class="form-control" value="15" min="1" max="120">
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: 'ثبت',
        cancelButtonText: 'انصراف'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire('موفق', 'زمان استراحت ثبت شد', 'success');
        }
    });
}
// CSS اختصاصی
const style = document.createElement('style');
style.textContent = `
    .attendance-card {
        border-radius: 20px;
        overflow: hidden;
    }
    
    .status-badge .badge {
        font-size: 1.1rem;
        border-radius: 50px;
    }
    
    .btn-checkin, .btn-checkout {
        transition: all 0.3s ease;
        box-shadow: 0 5px 15px rgba(0,0,0,0.2);
    }
    
    .btn-checkin:hover {
        transform: translateY(-3px);
        box-shadow: 0 8px 25px rgba(40, 167, 69, 0.3);
    }
    
    .btn-checkout:hover {
        transform: translateY(-3px);
        box-shadow: 0 8px 25px rgba(220, 53, 69, 0.3);
    }
    
    .day-cell {
        padding: 10px 5px;
        border-radius: 10px;
        margin: 2px;
        transition: all 0.3s ease;
        cursor: pointer;
    }
    
    .day-cell:hover {
        transform: scale(1.1);
        z-index: 1;
    }
    
    .day-header {
        font-weight: bold;
        padding: 10px 5px;
    }
    
    .day-number {
        font-weight: bold;
        font-size: 1.2rem;
    }
    
    .day-status {
        font-size: 0.8rem;
        margin-top: 5px;
    }
    
    .bg-success-light { background-color: rgba(40, 167, 69, 0.1); }
    .bg-warning-light { background-color: rgba(255, 193, 7, 0.1); }
    .bg-danger-light { background-color: rgba(220, 53, 69, 0.1); }
    .bg-info-light { background-color: rgba(23, 162, 184, 0.1); }
    .bg-primary-light { background-color: rgba(0, 123, 255, 0.1); }
    
    .timeline-item {
        padding: 8px 0;
        border-bottom: 1px solid #eee;
    }
    
    .timeline-badge {
        width: 30px;
        height: 30px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-left: 15px;
        color: white;
    }
    
    .stat-box {
        padding: 15px;
        border-radius: 10px;
        background: #f8f9fa;
    }
    
    .legend-item {
        display: inline-flex;
        align-items: center;
        margin: 0 10px;
    }
`;
document.head.appendChild(style);
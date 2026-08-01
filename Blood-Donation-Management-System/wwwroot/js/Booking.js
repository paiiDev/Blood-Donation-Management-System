document.addEventListener('DOMContentLoaded', function () {
    const btnNext = document.getElementById('btnNext');
    const step1 = document.getElementById('step1');
    const step2 = document.getElementById('step2');
    const centerSelect = document.getElementById('centerSelect');
    const dateSelect = document.getElementById('dateSelect');
    const timeSelect = document.getElementById('timeSelect');
    const phoneInput = document.getElementById('phone');
    const emailInput = document.getElementById('email');

    dateSelect.min = new Date().toISOString().split('T')[0];

    function escapeHtml(str) {
        return String(str).replace(/[&<>"']/g, function (m) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m];
        });
    }

    btnNext.addEventListener('click', async function () {
        if (centerSelect.value !== "" && dateSelect.value !== "" && timeSelect.value !== "") {

            let originalText = btnNext.innerHTML;
            btnNext.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i> စစ်ဆေးနေပါသည်...';
            btnNext.disabled = true;

            try {
                let url = `/Booking/CheckAvailability?centerId=${centerSelect.value}&date=${dateSelect.value}`;
                let response = await fetch(url);
                let result = await response.json();

                if (result.isAvailable) {
                    step1.classList.add('d-none');
                    step2.classList.remove('d-none');
                } else {
                    Swal.fire({ icon: 'error', title: 'မရနိုင်ပါ', text: result.message, confirmButtonColor: '#d32f2f' });
                }
            } catch (error) {
                console.error("Error connecting to server:", error);
                Swal.fire({ icon: 'error', title: 'ှServer Error', text: 'စနစ်ချို့ယွင်းမှု ဖြစ်ပေါ်နေပါသည်။', confirmButtonColor: '#d32f2f' });
            } finally {
                btnNext.innerHTML = originalText;
                btnNext.disabled = false;
            }

        } else {
            Swal.fire({ icon: 'warning', title: 'သတိပေးချက်', text: 'ကျေးဇူးပြု၍ နေရာ၊ ရက်စွဲနှင့် အချိန်ကို အပြည့်အစုံ ရွေးချယ်ပေးပါ။', confirmButtonColor: '#d32f2f' });
        }
    });

    var mmPhoneRegex = /^(09|\+?959)\d{7,9}$/;
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    phoneInput.addEventListener('blur', function () {
        var val = this.value.trim();
        if (val && !mmPhoneRegex.test(val)) {
            Swal.fire({ icon: 'error', title: 'ဖုန်းနံပါတ် မှားယွင်းနေပါသည်', text: 'မြန်မာဖုန်းနံပါတ် (09xxxxxxxxx) ဖြင့် ထည့်သွင်းပေးပါ။', confirmButtonColor: '#d32f2f' });
        }
    });

    emailInput.addEventListener('blur', function () {
        var val = this.value.trim();
        if (val && !emailRegex.test(val)) {
            Swal.fire({ icon: 'error', title: 'အီးမေးလ် မှားယွင်းနေပါသည်', text: 'မှန်ကန်သော အီးမေးလ်လိပ်စာတစ်ခု ထည့်သွင်းပေးပါ။', confirmButtonColor: '#d32f2f' });
        }
    });

    const btnConfirm = document.getElementById('btnConfirm');
    let isSubmitting = false;

    btnConfirm.addEventListener('click', async function (e) {
        e.preventDefault();
        if (isSubmitting) return;

        let payload = {
            CenterId: parseInt(document.getElementById('centerSelect').value),
            AppointmentDate: document.getElementById('dateSelect').value,
            TimeSlot: document.getElementById('timeSelect').value,
            FullName: document.getElementById('fullName').value,
            BloodGroup: document.getElementById('bloodGroup').value,
            Phone: phoneInput.value.trim(),
            Email: emailInput.value.trim()
        };


        if (!payload.FullName || !payload.Phone || !payload.Email || !payload.BloodGroup) {
            Swal.fire({ icon: 'warning', title: 'သတိပေးချက်', text: 'ကျေးဇူးပြု၍ ကိုယ်ရေးအချက်အလက်များကို အပြည့်အစုံ ဖြည့်ပေးပါ။', confirmButtonColor: '#d32f2f' });
            return;
        }

        if (!mmPhoneRegex.test(payload.Phone)) {
            Swal.fire({ icon: 'error', title: 'ဖုန်းနံပါတ် မှားယွင်းနေပါသည်', text: 'မြန်မာဖုန်းနံပါတ် (09xxxxxxxxx) ဖြင့် ထည့်သွင်းပေးပါ။', confirmButtonColor: '#d32f2f' });
            return;
        }

        if (payload.Email && !emailRegex.test(payload.Email)) {
            Swal.fire({ icon: 'error', title: 'အီးမေးလ် မှားယွင်းနေပါသည်', text: 'မှန်ကန်သော အီးမေးလ်လိပ်စာတစ်ခု ထည့်သွင်းပေးပါ။', confirmButtonColor: '#d32f2f' });
            return;
        }


        let originalText = btnConfirm.innerHTML;
        btnConfirm.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i> အတည်ပြုနေပါသည်...';
        btnConfirm.disabled = true;
        isSubmitting = true;

        try {

            let response = await fetch('/Booking/CreateBooking', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });


            let result = await response.json();
            if (result.isSuccess) {
                let bookingNumber = result.bookingNumber || 'BK-000000';
                let donorEmail = escapeHtml(payload.Email || '');

                Swal.fire({
                    icon: 'success',
                    title: 'စာရင်းသွင်းမှု အောင်မြင်ပါသည်!',
                    html:
                        '<div class="text-center">' +
                            '<p style="color:#555; font-size:15px; line-height:1.7; margin-top:6px;">' +
                                'လူကြီးမင်း၏ သွေးလှူဒါန်းရန် ရက်ချိန်းရယူမှုကို <b style="color:#d32f2f;">လက်ခံရရှိပြီးဖြစ်ပါသည်</b>။<br>' +
                                'တစ်စုံတစ်ယောက်၏ အသက်ကို ကယ်တင်ပေးနိုင်မည့် မွန်မြတ်သော ဆုံးဖြတ်ချက်အတွက် ဂုဏ်ယူပါသည်။' +
                            '</p>' +
                            '<div style="background:linear-gradient(135deg,#d32f2f,#8e0000); color:#fff; border-radius:14px; padding:18px 24px; margin:18px auto; max-width:320px; box-shadow:0 10px 28px rgba(211,47,47,.35);">' +
                                '<div style="font-size:11px; letter-spacing:2.5px; opacity:.85; text-transform:uppercase;">Booking Number</div>' +
                                '<div style="font-size:24px; font-weight:800; letter-spacing:1.5px; margin-top:5px; font-family:Consolas, monospace;">' + bookingNumber + '</div>' +
                                '<div style="font-size:11px; opacity:.8; margin-top:6px;"><i class="fa-solid fa-circle-check me-1"></i> အတည်ပြုဆဲ</div>' +
                            '</div>' +
                            '<div style="background:#fff5f5; border:1px solid #ffd6d6; border-radius:10px; padding:12px 16px; margin:0 auto; max-width:340px;">' +
                                '<p style="color:#8e0000; font-size:13.5px; line-height:1.6; margin:0;">' +
                                    '<i class="fa-regular fa-envelope me-1"></i>' +
                                    'သင့် Booking Number အပါအဝင် အသေးစိတ်အချက်အလက်များကို <b style="word-break:break-all;">' + donorEmail + '</b> သို့ အီးမေးလ်မှတစ်ဆင့် ပို့ဆောင်ပေးပါမည်။' +
                                '</p>' +
                            '</div>' +
                        '</div>',
                    showConfirmButton: true,
                    confirmButtonText: '<i class="fa-solid fa-handshake me-2"></i>အားလုံး အဆင်ပြေပါသည်',
                    confirmButtonColor: '#d32f2f',
                    allowOutsideClick: false,
                    backdrop: 'rgba(0,0,0,0.55)'
                }).then(() => {
                    window.location.reload();
                });
            } else {
                Swal.fire({ icon: 'error', title: 'မအောင်မြင်ပါ', text: result.message, confirmButtonColor: '#d32f2f' });
            }

        } catch (error) {
            console.error("AJAX Error:", error);
            Swal.fire({ icon: 'error', title: 'ဆာဗာအမှား', text: 'ဆာဗာနှင့် ဆက်သွယ်ရာတွင် အမှားအယွင်းရှိပါသည်။ ခေတ္တစောင့်ဆိုင်း၍ ပြန်လည်ကြိုးစားပါ။', confirmButtonColor: '#d32f2f' });
        } finally {

            btnConfirm.innerHTML = originalText;
            btnConfirm.disabled = false;
            isSubmitting = false;
        }
    });

    const btnBack = document.getElementById('btnBack');

    btnBack.addEventListener('click', function () {
        document.getElementById('fullName').value = "";
        document.getElementById('bloodGroup').value = "";
        phoneInput.value = "";
        emailInput.value = "";

        document.getElementById('step2').classList.add('d-none');
        document.getElementById('step1').classList.remove('d-none');
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const btnNext = document.getElementById('btnNext');
    const step1 = document.getElementById('step1');
    const step2 = document.getElementById('step2');
    const centerSelect = document.getElementById('centerSelect');
    const dateSelect = document.getElementById('dateSelect');
    const phoneInput = document.getElementById('phone');
    const emailInput = document.getElementById('email');

    btnNext.addEventListener('click', async function () {
        if (centerSelect.value !== "" && dateSelect.value !== "") {

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
            Swal.fire({ icon: 'warning', title: 'သတိပေးချက်', text: 'ကျေးဇူးပြု၍ နေရာနှင့် ရက်စွဲကို အပြည့်အစုံ ရွေးချယ်ပေးပါ။', confirmButtonColor: '#d32f2f' });
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

    btnConfirm.addEventListener('click', async function (e) {
        e.preventDefault();

        let payload = {
            CenterId: parseInt(document.getElementById('centerSelect').value),
            AppointmentDate: document.getElementById('dateSelect').value,
            TimeSlot: document.getElementById('timeSelect').value,
            FullName: document.getElementById('fullName').value,
            BloodGroup: document.getElementById('bloodGroup').value,
            Phone: phoneInput.value.trim(),
            Email: emailInput.value.trim()
        };


        if (!payload.FullName || !payload.Phone || !payload.BloodGroup) {
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
                Swal.fire({ icon: 'success', title: 'အောင်မြင်ပါသည်', text: result.message, confirmButtonColor: '#d32f2f' }).then(() => {
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

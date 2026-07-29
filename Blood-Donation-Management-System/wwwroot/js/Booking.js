document.addEventListener('DOMContentLoaded', function () {
    const btnNext = document.getElementById('btnNext');
    const step1 = document.getElementById('step1');
    const step2 = document.getElementById('step2');
    const centerSelect = document.getElementById('centerSelect');
    const dateSelect = document.getElementById('dateSelect');

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
                    alert(result.message);
                }
            } catch (error) {
                console.error("Error connecting to server:", error);
                alert("စနစ်ချို့ယွင်းမှု ဖြစ်ပေါ်နေပါသည်။");
            } finally {
                btnNext.innerHTML = originalText;
                btnNext.disabled = false;
            }

        } else {
            alert("ကျေးဇူးပြု၍ နေရာနှင့် ရက်စွဲကို အပြည့်အစုံ ရွေးချယ်ပေးပါ။");
        }
    });
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
        Phone: document.getElementById('phone').value,
        Email: document.getElementById('email').value
    };

    
    if (!payload.FullName || !payload.Phone || !payload.BloodGroup) {
        alert("ကျေးဇူးပြု၍ ကိုယ်ရေးအချက်အလက်များကို အပြည့်အစုံ ဖြည့်ပေးပါ။");
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

   
        let result = await response.json(); if (result.success) {
            // အောင်မြင်ပါက Alert ပြပြီး Page ကို အစမှ ပြန်စမည် (သို့မဟုတ် Success Page သို့ သွားနိုင်သည်)
            alert(result.message);
            window.location.reload();
        } else {
            // ကျရှုံးပါက Service မှ ပြန်ပေးသော Error Message ကို ပြမည်
            alert("Error: " + result.message);
        }

    } catch (error) {
        console.error("AJAX Error:", error);
        alert("ဆာဗာနှင့် ဆက်သွယ်ရာတွင် အမှားအယွင်းရှိပါသည်။ ခေတ္တစောင့်ဆိုင်း၍ ပြန်လည်ကြိုးစားပါ။");
    } finally {
       
        btnConfirm.innerHTML = originalText;
        btnConfirm.disabled = false;
    }
});



const btnBack = document.getElementById('btnBack');

btnBack.addEventListener('click', function () {
    document.getElementById('fullName').value = "";
    document.getElementById('bloodGroup').value = "";
    document.getElementById('phone').value = "";
    document.getElementById('email').value = "";

    document.getElementById('step2').classList.add('d-none');
    document.getElementById('step1').classList.remove('d-none');
});
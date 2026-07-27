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
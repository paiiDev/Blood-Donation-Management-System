(function () {
    function logout() {
        Swal.fire({
            title: 'ထွက်မည်မှာ သေချာပါသလား?',
            text: 'စနစ်မှ ထွက်လိုပါက အတည်ပြုပါ။',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'ဟုတ်ကဲ့၊ ထွက်မည်',
            cancelButtonText: 'မထွက်တော့ပါ',
            confirmButtonColor: '#d32f2f',
            cancelButtonColor: '#6c757d'
        }).then(function (result) {
            if (!result.isConfirmed) return;

            var token = document.getElementById('antiforgeryToken').value;
            var body = new URLSearchParams();
            body.append('__RequestVerificationToken', token);

    fetch('/Auth/Logout', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: body.toString()
            })
                .then(function (response) { return response.json(); })
                .then(function (data) {
                    if (data.success) {
                        return Swal.fire({
                            icon: 'success',
                            title: 'ထွက်ပြီးပါပြီ',
                            text: 'နောက်တစ်ကြိမ် ပြန်လည်ကြိုဆိုရန် စောင့်မျှော်ပါတယ်။',
                            confirmButtonText: 'အဆင်ပြေပါသည်',
                            confirmButtonColor: '#d32f2f',
                            allowOutsideClick: false
                        });
                    }
                    throw new Error('Logout failed');
                })
                .then(function () {
                    window.location.href = '/Auth/Login';
                })
                .catch(function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'အမှားတစ်ခု ဖြစ်ပွားခဲ့သည်',
                        text: 'ထွက်ရာတွင် ပြဿနာတစ်ခု ဖြစ်ပွားခဲ့ပါသည်။ ထပ်မံကြိုးစားပါ။',
                        confirmButtonColor: '#d32f2f'
                    });
                });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        var logoutBtns = document.querySelectorAll('[data-logout]');
        for (var i = 0; i < logoutBtns.length; i++) {
            logoutBtns[i].addEventListener('click', function (e) {
                e.preventDefault();
                logout();
            });
        }
    });
})();

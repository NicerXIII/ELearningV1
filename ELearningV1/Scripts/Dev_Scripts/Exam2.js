var FirstCountDownDate = "";
$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
           // getAllUrlParams();

            $.ajax({
                type: "POST",
                url: "/Exam/GetDateEnrolled",
                data: {},
                success: function (response) {
                    LoadingData();
                    if (response._res == "Expired") {
                        window.location.href = "../Home/Index";
                    }
                    else {
                        FirstCountDownDate = response._res;
                    }
                    EndLoading();
                },
                error: function (response) { alert(response._res); }
            });
        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }

    });
});
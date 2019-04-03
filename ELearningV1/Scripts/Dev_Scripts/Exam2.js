$(document).ready(function () {
    LoadingData();
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            EndLoading();
        },
        error: function (response) {
            EndLoading();
            window.location.href = '/LogIn/Index';
        }

    });
});
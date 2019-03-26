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

var x = setInterval(function () {
    // Get todays date and time
    var now = new Date().getTime();
    var countDownDate = "";
    if (FirstCountDownDate == "Expired") {

    }
    else {
        countDownDate = new Date(FirstCountDownDate).getTime();
    }

    // Find the distance between now and the count down date
    var distance = countDownDate - now;

    // Time calculations for days, hours, minutes and seconds
    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Output the result in an element with id="demo"
    document.getElementById("demo").innerHTML = days + "d " + hours + "h "
        + minutes + "m " + seconds + "s ";

    // If the count down is over, write some text
    if (distance < 0) {
        clearInterval(x);
        document.getElementById("demo").innerHTML = "EXPIRED";
    }
}, 1000);

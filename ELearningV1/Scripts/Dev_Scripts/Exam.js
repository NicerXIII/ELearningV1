// Set the date we're counting down to
var FirstCountDownDate = "";//new Date("March 21, 2019 00:00:00").getTime();

$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            getAllUrlParams();

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

// Update the count down every 1 second
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

var answers = new Array();
var Arrayid = new Array();

function getAllUrlParams() {
    var data = "";
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
        loadExamData(value);
        data = value;
    });
    return data;
}

var loadExamData = function (CourseID) {
    $.ajax({
        type: "POST",
        url: "Exam/Exam",
        data: { "CourseID": CourseID },
        success: function (response) {
            loadData(CourseID);
        }
    });
}

var loadData = function (CourseID) {
    debugger
    var dExam = document.getElementById("divEXAM");
    var dVideo = document.getElementById("divVIDEO");
    var dPDF = document.getElementById("divPDF");
    var dScoring = document.getElementById("divScoring");

    var iFrameVideo = document.getElementById("videoTuitorial");

    var btnExam = document.getElementById("btnNext");
    var btnVideo = document.getElementById("btnProceedVideo");
    var btnPDF = document.getElementById("btnProceedPDF");

    var CourseSec = $("#CourSecID").val();
    var OrderNo = $("#SectionOrderNo").val();
    if (OrderNo === "") {
        OrderNo = 0;
    }

    $.ajax({
        type: "POST",
        url: "/Exam/GetDataToLoad",
        data: { "CourseID": CourseID, "PreviousOrderSec": OrderNo },
        success: function (response) {
            debugger
            $("#SectionOrderNo").val(response._currentSectionOrder);

            if (response.res === "Video") {
                $.ajax({
                    type: "POST",
                    url: "/Exam/loadVideo",
                    data: { "CourseID": CourseID },
                    success: function (responseVideo) {
                        debugger
                        LoadingData();
                        dVideo.style.display = "block";
                        btnVideo.style.display = "block";

                        dPDF.style.display = "none";
                        dExam.style.display = "none";
                        dScoring.style.display = "none";

                        btnExam.style.display = "none";
                        btnPDF.style.display = "none";

                        var text = "Save";
                        document.getElementById("btnNext").innerHTML = text;

                        $("#boxTitleVideo").text(responseVideo.resTitleVideo);
                        document.getElementById("videoTuitorial").src = responseVideo.resVideo;
                        EndLoading();
                    }
                });
            }

            else if (response.res === "PDF") {
                $.ajax({
                    type: "POST",
                    url: "/Exam/loadPDF",
                    data: { "CourseID": CourseID },
                    success: function (responsePDF) {
                        debugger
                        LoadingData();
                        iFrameVideo.pause();

                        $("#boxTitlePDF").text(responsePDF.resTitlePDF);
                        document.getElementById("framePDF").src = responsePDF.resPDF;

                        dPDF.style.display = "block";
                        btnPDF.style.display = "block";

                        dVideo.style.display = "none";
                        dExam.style.display = "none";
                        dScoring.style.display = "none";

                        btnExam.style.display = "none";
                        btnVideo.style.display = "none";
                        EndLoading();
                    }
                });
            }

            else if (response.res === "Test") {
                LoadingData();
                $("#modal-info").modal("show");
                dPDF.style.display = "none";
                dVideo.style.display = "none";
                dExam.style.display = "block";
                dScoring.style.display = "none";

                iFrameVideo.pause();

                btnExam.style.display = "block"
                btnVideo.style.display = "none";
                btnPDF.style.display = "none";
                getQuestionID();
                EndLoading();
            }

            //For scoring
            else if (response.res === "Scoring") {
                LoadingData();
                iFrameVideo.pause();
                dScoring.style.display = "block";
                dPDF.style.display = "none";
                dVideo.style.display = "none";
                dExam.style.display = "none";

                btnExam.style.display = "none"
                btnVideo.style.display = "none";
                btnPDF.style.display = "none";

                $.ajax({
                    type: "POST",
                    url: "/Exam/getScoreofEmployeeExam",
                    data: { "CourseID": CourseID, "CourseSectionID": CourseSec },
                    success: function (responseScore) {
                        debugger
                        var a = document.getElementById("panelPositive");
                        var b = document.getElementById("panelNegative");

                        $("#labelNoQuestion").text(responseScore._questionCount);
                        $("#labelScore").text(responseScore._score + "%");

                        if (responseScore._score <= 74) {
                            b.style.display = "block";
                        }
                        else if (responseScore._score >= 75) {
                            a.style.display = "block";
                        }
                    }
                });
                EndLoading();
            }
        },
    });

}

var getNextDataToLoad = function () {
    debugger
    var OrderNo = $("#SectionOrderNo").val();
    var data = getAllUrlParams();

    loadData(data); //, OrderNo
}

var getQuestionID = function () {
    var rows = document.getElementsByTagName("tbody")[0].rows; //document.getElementById("tbody").rows.length; //

    for (var i = 0; i < rows.length; i++) {

        var chk = document.getElementsByTagName("input")[i].getAttribute("name");

        //Check if chk is null
        if (chk === null || chk === "" || chk === " ") { }

        else {
            if (chk === "q")
            {   }
            else {
                var value = chk.match(/\d/g);//chk.slice(-1);
                value = value.join("");

                var result = Arrayid.includes(value);
                if (result === false) {
                    Arrayid.push(value);
                }
            }
        }
    }
}

$(".checkBox_").change(function () {
    debugger
    //var id = document.getElementById("chkid");
    var value = this.value;
    if (this.id.checked === true) {
        if (answers.includes(this.value)) {
            answers.splice(answers.findIndex(x => x === value), 1);
        }
        else {
            answers.push(this.value);
        }
    }
});

$(".radio_").change(function () {
    debugger
    // for employe who change their answer
    if (answers.includes(this.value)) {
        // do nothing
    }
    else {
        answers.push(this.value);
    }
});

var confirmation = function () {
    debugger
    var xPDF = document.getElementById("divPDF");
    var xVideo = document.getElementById("divVIDEO");
    var xExam = document.getElementById("divEXAM");
    var data = getAllUrlParams();
    if (xExam.style.display === "block") {
        if (answers === "" || answers === null) {
            alert("Please answer all the questions");
        }
        else { $("#modal-Confirmation").modal('show'); }
    }
}

var SaveAnswers = function () {
    $("#modal-Confirmation").modal('hide')
    debugger
    //for QuestionID
    let unique = [...new Set(Arrayid)];

    $.ajax({
        type: "POST",
        url: "/Exam/SaveEmployeeAnswers",
        data: { "QuestionID": unique, "EmployeeAnswers": answers },
        success: function (response) {
            alert(response.res);
            getNextDataToLoad();
        }
    });
}

var cancelSaving = function () {
    //answers = "";
    $("#modal-Confirmation").modal('hide');
}

var Decision = function () {
    var selected = $("#ddChoices option:selected").val();
    var CourseSec = $("#CourSecID").val();

    var dExam = document.getElementById("divEXAM");
    var dVideo = document.getElementById("divVIDEO");
    var dPDF = document.getElementById("divPDF");
    var dScoring = document.getElementById("divScoring");

    var iFrameVideo = document.getElementById("videoTuitorial");

    var btnExam = document.getElementById("btnNext");
    var btnVideo = document.getElementById("btnProceedVideo");
    var btnPDF = document.getElementById("btnProceedPDF");
    //re-take exam
    if (selected === "1") {
        $.ajax({
            type: "POST",
            url: "/Exam/EmpResetAnswer",
            data: { "CourseSecID": CourseSec },
            success: function (responseReset) {
                LoadingData();
                if (responseReset.res === "true") {
                    window.location.reload(true);
                    //$("#modal-info").modal("show");
                    //dExam.style.display = "block";
                    //dPDF.style.display = "none";
                    //dVideo.style.display = "none";
                    //dScoring.style.display = "none";

                    //iFrameVideo.pause();

                    ////Clearing checkbox and radio button
                    //document.getElementById("chkid").checked = false;
                    //var rad = document.getElementById("radioid");
                    //for (var i = 0; i < rad.length; i++) {
                    //    if (rad[i].checked) rad[i].checked = false;
                    //}

                    //$("#chkid").prop("checked", false);
                    //$("#radioid").prop("checked", false);
                    ////document.getElementById("radioid").checked = false;

                    //btnExam.style.display = "block";
                    //btnVideo.style.display = "none";
                    //btnPDF.style.display = "none";
                    //getQuestionID();
                }
                EndLoading();
            }
        });
    }
    //go home
    else if (selected === "2") {
        window.location.href = "../Home/Index";
    }
}
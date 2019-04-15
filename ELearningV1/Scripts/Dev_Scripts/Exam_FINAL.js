$(document).ready(function () {
    debugger
    var cid = $("#CourseID1").val();
    $("#SectionOrderNo").val(1);
    loadSectionData(cid, 1);
    EraseEmployeeAnswersOnRefreshOrOnLoad(cid);
});

var empAns = new Array();
var questID = new Array();

var input = {
    year: 0,
    month: 0,
    day: 0,
    hours: 0,
    minutes: 0,
    seconds: 0
};

var timestamp = new Date(input.year, input.month, input.day,
    input.hours, input.minutes, input.seconds);

var interval = 1;

setInterval(function () {
    timestamp = new Date(timestamp.getTime() + interval * 1000);
    document.getElementById('countdown2').innerHTML = timestamp.getHours() + 'h:' + timestamp.getMinutes() + 'm:' + timestamp.getSeconds() + 's';
}, Math.abs(interval) * 1000);



var ocProceedNextProcess = function () {
    var cid = $("#CourseID1").val();
    var sec1 = $("#SectionOrderNo").val();
    var secCon = parseInt(sec1) + 1;
    $("#SectionOrderNo").val(secCon);

    loadSectionData(cid, secCon);
}

function loadSectionData(cid, orderN) {
    LoadingData();
    $.ajax({
        type: "POST",
        url: "/Exam/GetSectionDataByCourseIDAndOrderSec",
        data: { "CourseID": cid, "OrderSec": orderN },
        success: function (response) {
            var rType = response.Type;
            var rOrderSec = response.OrderSec;
            var secID = response.ID

            var pdfID = "divPDF" + rOrderSec;
            var testID = "divEXAM" + rOrderSec;
            var vidID = "divVIDEO" + rOrderSec;
            var vidExamID = "divVIDEOExam" + rOrderSec;

            $("#SectionID").val(secID);
            if (rType == "PDF") {
                $("#" + pdfID).removeClass('hidden');
                //Hide
                pdfID = "divPDF" + (orderN - 1);
                testID = "divEXAM" + (orderN - 1);
                vidID = "divVIDEO" + (orderN - 1);
                vidExamID = "divVIDEOExam" + (orderN - 1);

                $("#" + testID).addClass('hidden');
                $("#" + vidID).addClass('hidden');
                $("#" + pdfID).addClass('hidden');
                $("#" + vidExamID).addClass('hidden');
                $("#btnProceedTest").addClass('hidden');
                $("#btnProceed").removeClass('hidden');
                UpdateUserCurrentProgress();
            }
            else if (rType == "Test") {
                $("#" + testID).removeClass('hidden');
                //Hide
                testID = "divEXAM" + (orderN - 1);
                pdfID = "divPDF" + (orderN - 1);
                vidID = "divVIDEO" + (orderN - 1);
                vidExamID = "divVIDEOExam" + (orderN - 1);

                $("#" + pdfID).addClass('hidden');
                $("#" + vidID).addClass('hidden');
                $("#" + testID).addClass('hidden');
                $("#" + vidExamID).addClass('hidden');
                $("#btnProceed").addClass('hidden');
                $("#btnProceedTest").removeClass('hidden');
                UpdateUserCurrentProgress();
            }
            else if (rType == "VideoExam") {
                $("#" + vidExamID).removeClass('hidden');

                testID = "divEXAM" + (orderN - 1);
                pdfID = "divPDF" + (orderN - 1);
                vidID = "divVIDEO" + (orderN - 1);
                vidExamID = "divVIDEOExam" + (orderN - 1);

                $("#" + pdfID).addClass('hidden');
                $("#" + vidID).addClass('hidden');
                $("#" + testID).addClass('hidden');
                $("#" + vidExamID).addClass('hidden');
                $("#btnProceed").addClass('hidden');
                $("#btnProceedTest").removeClass('hidden');
                UpdateUserCurrentProgress();
            }
            else {
                $("#" + vidID).removeClass('hidden');
                //Hide
                vidID = "divVIDEO" + (orderN - 1);
                pdfID = "divPDF" + (orderN - 1);
                testID = "divEXAM" + (orderN - 1);
                vidExamID = "divVIDEOExam" + (orderN - 1);

                $("#" + testID).addClass('hidden');
                $("#" + pdfID).addClass('hidden');
                $("#" + vidID).addClass('hidden');
                $("#" + vidExamID).addClass('hidden');
                $("#btnProceedTest").addClass('hidden');
                $("#btnProceed").removeClass('hidden');
                UpdateUserCurrentProgress();
            }

        },
        error: function (response) {
            var orderN1 = $("#SectionOrderNo").val();
            vidID = "divVIDEO" + (orderN1 - 1);
            pdfID = "divPDF" + (orderN1 - 1);
            testID = "divEXAM" + (orderN1 - 1);
            vidExamID = "divVIDEOExam" + (orderN - 1);

            $("#" + testID).addClass('hidden');
            $("#" + pdfID).addClass('hidden');
            $("#" + vidID).addClass('hidden');
            $("#" + vidExamID).addClass('hidden');
            $("#btnProceed").addClass('hidden');
            $("#btnProceedTest").addClass('hidden');
            $("#divScoring").removeClass('hidden');
            EndLoading();
        }
    });
}

function UpdateUserCurrentProgress() {
    var cid = $("#CourseID1").val();
    var sic = $("#SectionCount").val();
    var sec1 = $("#SectionOrderNo").val();

    $.ajax({
        type: "POST",
        url: "/Exam/UpdateCourseProgess",
        data: { "CourseID": cid, "SectionCount": sic, "CurSection": sec1 },
        success: function (response) {
            UpdateTimeConsumed();
        },
        error: function (response) { }
    });
}

function UpdateTimeConsumed() {
    var cid = $("#CourseID1").val();
    var hours = timestamp.getHours();
    var mins = timestamp.getMinutes();
    var secs = timestamp.getSeconds();
    $.ajax({
        type: "POST",
        url: "/Exam/UpdateUserConsumedTime",
        data: { "CourseID": cid, "StrHours": hours, "StrMinutes": mins, "StrSeconds": secs },
        success: function (response) {
            var result = response.res;
            if (result == true) {
                EndLoading();
            } else {
                alert("ERROR: Saving timestamp");
                EndLoading();
            }

        },
        error: function (response) { }
    });
}

var ocProceedNextTestProcess = function () {
    $("#modal-Confirmation").modal('show');
}

var add_checked = function (checked1) {
    debugger
    var checkBoxName = checked1.name;
    var checkBoxValue = checked1.value;
    if ($('input[name=' + checkBoxName + ']').is(':checked')) {
        empAns.push(checkBoxValue);
    } else {
        empAns.splice($.inArray(checkBoxValue, empAns), 1);
        for (a in empAns) {
            if (empAns[a] == checkBoxValue) {
                empAns.splice(a, 1);
            }
        }
    }
}

var SaveEmployeeAnswers1 = function () {
    var cid = $("#CourseID1").val();
    var sid = $("#SectionID").val();
    $.ajax({
        type: "POST",
        url: "/Exam/GetQuizRadioIDbyCourseID",
        data: { "CourseID": cid, "SectionID": sid },
        success: function (response) {
            for (i in response) {
                debugger
                questID.push(response[i].ID);
                if (response[i].QuestionType == "3") {
                    var radioName = "rd" + response[i].ID;
                    var radioData = $("input[name=" + radioName + "]:checked").val();
                    empAns.push(radioData);
                }
            }
            ExecuteSaveingProcess1();
        },
        error: function (response) { }
    });
}

function ExecuteSaveingProcess1() {
    $.ajax({
        type: "POST",
        url: "/Exam/SaveEmployeeAnswers",
        data: { "QuestionID": questID, "EmployeeAnswers": empAns },
        success: function (response) {
            var result = response.res;
            if (result != "No Data") {
                questID = [];
                empAns = [];
                ExecuteSaveScoreOfUser();
            } else {
                alert("Please atleast answer one question.");
            }
        }
    });
}

function ExecuteSaveScoreOfUser() {
    var cid = $("#CourseID1").val();
    $.ajax({
        type: "POST",
        url: "/Exam/UpdateEmployeeScoreByCourseIDSectionIDAndEmployeeNumber",
        data: { "CourseID": cid },
        success: function (response) {
            var sec1 = $("#SectionOrderNo").val();
            var secCon = parseInt(sec1) + 1;
            $("#SectionOrderNo").val(secCon);
            $("#labelNoQuestion").text(response.mscore);
            $("#labelScore").text(response.tscore);

            $("#modal-Confirmation").modal('hide');
            loadSectionData(cid, secCon);
        }
    });
}

function EraseEmployeeAnswersOnRefreshOrOnLoad(CID) {
    $.ajax({
        type: "POST",
        url: "/Home/EraseEmployeeAswerByCourseID",
        data: { "CourseID": CID },
        success: function (response) {
            ResetEmployeeScore(CID)
        },
        error: function (response) { }
    });
}

function ResetEmployeeScore(CID) {
    $.ajax({
        type: "POST",
        url: "/Home/ResetEmployeeScoreByCourseID",
        data: { "CourseID": CID },
        success: function (response) {

        },
        error: function (response) { }
    });
}

var ocOkFinished = function () {
    window.location.href = '@Url.Action("Index", "Home")';
}
$(document).ready(function () {
    LoadingData();
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            PanelBoxStatus();
            UserActivityChart();
            UserProgressActivity();
            BindDataTable();
        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }

    });
});

var PanelBoxStatus = function () {
    $.ajax({
        type: "POST",
        url: "/Home/LoadPanelBoxData",
        data: {},
        success: function (response) {
            $("#CIP").text(response._cip);
            $("#CC").text(response._cc);
            $("#PA").text(response._pa);
            $("#INC").text(response._inc);
        },
        error: function (response) { }
    });
}

var UserActivityChart = function () {
    $.ajax({
        type: "POST",
        url: "/Home/LoadUserLogInHistoryByID1",
        data: {},
        success: function (response) {
            var WeekList = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

            Highcharts.chart('UserActivity', {

                title: {
                    text: 'LogIn History'
                },

                yAxis: {
                    title: {
                        text: 'IsActive'
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                xAxis: [{
                    categories: WeekList,
                }],
                series: [{
                    name: 'IsActive',
                    data: response._s
                }],

                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                }
            });
        },
        error: function (response) { }
    });
}
//-------------------------DONUT--------------------------------------//
var UserProgressActivity = function () {
    $.ajax({
        type: "POST",
        url: "/Home/LoadUserProgressAndStatus",
        data: {},
        success: function (response) {
            var pieColors = ['#4fff63', '#dbdbdb'];
            var prog = 0;
            if (response._tCurrentPer >= 100) {
                prog = 100;
            } else {
                prog = response._tCurrentPer;
            }
            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'ProGressAct',
                    type: 'pie'
                },
                title: {
                    text: 'Course Progress'
                },
                yAxis: {
                    title: {
                        text: 'Total percent market share'
                    }
                },
                plotOptions: {
                    pie: {
                        colors: pieColors,
                        shadow: false
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.point.name + '</b>: ' + this.y + ' %';
                    }
                },
                series: [{
                    name: 'Browsers',
                    data: [["Progress", prog], ["Remaining", response._tRemnainPer]],
                    size: '60%',
                    innerSize: '50%',
                    showInLegend: true,
                    dataLabels: {
                        enabled: false
                    }
                }]
            });
        },
        error: function (response) { }
    });

}
var otable;
var BindDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#courseProgTable")) {
        //Clear table for redraw
        otable.draw();
    }
    else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        otable = $("#courseProgTable").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Home/LoadUserCurrentCourse",
            "fnServerData": function (sSource, aoData, fnCallback) {
                $.ajax({
                    type: "Get",
                    data: aoData,
                    url: sSource,
                    success: fnCallback
                });
            },
            "aoColumns": [
                {
                    "mData": "Course",
                    "orderable": false,
                    "render": function (Course, type, full, meta) {
                        debugger
                        var comp = moment.tz(full.CompletionDate, 'Asia/Manila').format("MM/DD/YYYY");
                        var now = moment.tz('Asia/Manila').format("MM/DD/YYYY");
                        //var dComp = new Date(comp);
                        // dNow = new Date(now);
                        if (full.Progress < 100 && comp >= now) {
                            return '<a href="#/"  onclick="ocSelectThisCourse(\'' + full.CourseID + '\')" style="font-size:24px;">' + Course + '</a>';
                        }
                        else if (full.Progress <= 100 && comp >= now && full.Score == 0) {
                            return '<a href="#/"  onclick="ocSelectThisCourse(\'' + full.CourseID + '\')" style="font-size:24px;">' + Course + ' <span class="fa fa-film"></span></a>';
                        }
                        else if (full.Progress <= 100 && comp < now) {
                            return '<a href="#/" style="font-size:24px; color:grey;">' + Course + '</a>';
                        }
                        else {
                            return '<a href="#/" style="font-size:24px; color:green;">' + Course + ' <span class="fa fa-check"></span></a>';
                        }

                    }
                },
                {
                    "mData": "Progress",
                    "orderable": false,
                    "className": "dt-center", "targets": "_all",
                    "render": function (Progress, type, full, meta) {
                        if (Progress >= 100) {
                            return '<div class="progress progress-striped"><div class="progress-bar progress-bar-green" style="width: 100%; border-radius:5px; color:black;" align="center">100%</div></div>';
                        }
                        return '<div class="progress progress-striped"><div class="progress-bar progress-bar-green" style="width: ' + Progress + '%; border-radius:5px; color:black;" align="center">' + Progress + '%</div></div>';
                    }
                },
                {
                    "mData": "Score",
                    "orderable": false,
                    "className": "dt-center", "targets": "_all",
                    "render": function (Score, type, full, meta) {
                        if (full.Progress >= 100) {
                            return Score;
                        } else {
                            return 0;
                        }
                    }
                },
                {
                    "mData": "EnrolledDate",
                    "orderable": false,
                    render: function (EnrolledDate, type, row) {
                        if (type === "sort" || type === "type") {
                            return EnrolledDate;
                        }
                        return moment.tz(EnrolledDate, 'Asia/Manila').format("MM/DD/YYYY");
                    }
                },
                {
                    "mData": "CompletionDate",
                    "orderable": false,
                    render: function (CompletionDate, type, row) {
                        if (type === "sort" || type === "type") {
                            return CompletionDate;
                        }
                        return moment.tz(CompletionDate, 'Asia/Manila').format("MM/DD/YYYY");
                    }
                },
                {
                    "mData": "ConsumedTime",
                    "orderable": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "searching": false,
            "bPaginate": false,
            "sScrollY": "300",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
        EndLoading();
    }
}

var ocSelectThisCourse = function (CID) {
    $("#CourseID").val(CID);
    $.ajax({
        type: "POST",
        url: "/Home/CheckCurrentStatus",
        data: { "CourseID": CID },
        success: function (response) {
            debugger
            var result = parseInt(response.res);
            if (result > 0) {
                $("#modalDeleteUserProgress").modal('show');
            } else {
                $("#modalProceedToExam").modal('show');

            }
        },
        error: function (response) { }
    });
}

var ocDeleteUserProgress = function () {
    var CID = $("#CourseID").val();
    if (CID != "") {
        $.ajax({
            type: "POST",
            url: "/Home/ResetUserCourseProgress",
            data: { "CourseID": CID },
            success: function (response) {
                debugger
                var result = response.res;
                if (result == true) {
                    EraseEmployeeAnswers(CID);
                } else {
                    alert("ERROR: The exam result is already set, please contact your administrator if you want to retake the exam.");
                    $("#modalDeleteUserProgress").modal('hide');
                }
            },
            error: function (response) { }
        });
    } else {
        alert("ERROR: Please try again");
    }
}

function EraseEmployeeAnswers(CID) {
    $.ajax({
        type: "POST",
        url: "/Home/EraseEmployeeAswerByCourseID",
        data: { "CourseID": CID },
        success: function (response) {
            debugger
            var result = response.res;
            if (result == true) {
                alert("Data reset successfully");
                $("#modalDeleteUserProgress").modal('hide');
                BindDataTable();
                UserProgressActivity();
            } else {
                alert("ERROR: The exam result is already set, please contact your administrator if you want to retake the exam.");
                $("#modalDeleteUserProgress").modal('hide');
            }
        },
        error: function (response) { }
    });
}

var ocYesTakeExam = function () {
    var CID = $("#CourseID").val();
    $("#modalProceedToExam").modal('hide');
    window.location.href = "/Exam/Exam?CourseID=" + CID + ""
}
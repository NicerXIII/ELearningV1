$(document).ready(function () {
    PanelBoxStatus();
});

var PanelBoxStatus = function () {
    $.ajax({
        type: "POST",
        url: "/Agent/LoadPanelBoxDataOfAllAgents",
        data: {},
        success: function (response) {
            $("#CIP").text(response._cip);
            $("#CC").text(response._cc);
            $("#PA").text(response._pa);
            $("#INC").text(response._inc);
            BindDataTable();
        },
        error: function (response) { }
    });
}

var otable;
var BindDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblEmpListStatus")) {
        //Clear table for redraw
        otable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        otable = $("#tblEmpListStatus").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Agent/LoadEmployeeCourseStatus",
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
                    "mData": "EmployeeNumber",
                    "orderable": false,
                    "width": "150px",
                    "className": "text-center", "targets": "_all",
                    "render": function (EmployeeNumber, type, full, meta) {
                        return '<a href="#/">' + EmployeeNumber + '</a>';
                    }
                },
                {
                    "mData": "EmpName",
                    "orderable": false
                },
                {
                    "mData": "Course",
                    "orderable": false
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
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "Status1",
                    "orderable": false,
                    "className": "dt-center", "targets": "_all"
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
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "bPaginate": false,
            "sScrollY": "300",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}
//Click the columns to view versions. Source: <a href="http://statcounter.com" target="_blank">statcounter.com</a>

var ocCourseReports = function () {
    $.ajax({
        type: "POST",
        url: "/Agent/GetEmployeeStatusForBarChart",
        data: {},
        success: function (response) {
            getDateRange();
            var BarColors = ['#4fff63', '#ff9b9b', '#91f9ff'];
            var now = moment.tz('Asia/Manila').format("YYYY");
            Highcharts.chart('container', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Employee Exam Result of ' + now
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    type: 'category'
                },
                yAxis: {
                    title: {
                        text: 'Total percent market share'
                    }

                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    series: {
                        colors: BarColors,
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.1f}%'
                        }
                    }
                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
                },

                "series": [
                    {
                        "name": "Browsers",
                        "colorByPoint": true,
                        "data": [
                            {
                                "name": "Passed",
                                "y": response.passed
                            },
                            {
                                "name": "Failed",
                                "y": response.failed
                            },
                            {
                                "name": "InProgress",
                                "y": response.inprog
                            }
                        ]
                    }
                ]
            });
        },
        error: function (response) { }
    });
}

function getDateRange() {
    $.ajax({
        type: "POST",
        url: "/Agent/GetDateRange",
        data: {},
        success: function (response) {

            var dateFrom = response._dateFrom;
            var dateTo = response._dateTo;

            var FromDate = new Date(dateFrom);
            var Fday = ("0" + FromDate.getDate()).slice(-2);
            var Fmonth = ("0" + (FromDate.getMonth() + 1)).slice(-2);
            var Ftoday = FromDate.getFullYear() + "-" + (Fmonth) + "-" + (Fday);

            var ToDate = new Date(dateTo);
            var Tday = ("0" + ToDate.getDate()).slice(-2);
            var Tmonth = ("0" + (ToDate.getMonth() + 1)).slice(-2);
            var Ttoday = ToDate.getFullYear() + "-" + (Tmonth) + "-" + (Tday);

            $("#dFrom").val(Ftoday);
            $("#dTo").val(Ttoday);
            EmpStatusDataTable();
        }
    });
}


var stable;
var EmpStatusDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblEmpStatusByDate")) {
        //Clear table for redraw
        stable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        stable = $("#tblEmpStatusByDate").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Agent/LoadEmployeeByStatusAndDateRange",
            "fnServerData": function (sSource, aoData, fnCallback) {

                var empStatus = $("#EmpStatus option:selected").val();
                var dFrom = $("#dFrom").val();
                var dTo = $("#dTo").val();

                aoData.push({ "name": "Status", "value": empStatus });
                aoData.push({ "name": "DFrom", "value": dFrom });
                aoData.push({ "name": "DTo", "value": dTo });

                $.ajax({
                    type: "Get",
                    data: aoData,
                    url: sSource,
                    success: fnCallback
                });
            },
            "aoColumns": [
                {
                    "mData": "EmployeeNumber",
                    "orderable": false,
                    "width": "25px",
                    "className": "text-center", "targets": "_all",
                    "render": function (EmployeeNumber, type, full, meta) {
                        return '<a href="#/">' + EmployeeNumber + '</a>';
                    }
                },
                {
                    "mData": "EmpName",
                    "width": "200px",
                    "orderable": false
                },
                {
                    "mData": "Score",
                    "orderable": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CampiagnName",
                    "orderable": false,
                    "className": "dt-center", "targets": "_all"
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
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "bPaginate": false,
            "sScrollY": "300",
            "scrollX": true,
            "bScrollCollapse": false,
            "searching": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}

$("#EmpStatus").change(function () {
    EmpStatusDataTable();
})

var ocDFrom = function () {
    EmpStatusDataTable();
}

var ocDTo = function () {
    EmpStatusDataTable();
}
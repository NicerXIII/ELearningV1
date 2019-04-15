$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {

        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }

    });
});

var CourseDetailReport = function () { window.location.href = '@Url.Action("CourseDetailReport", "Course")'; }

var ExamList = function () {
    var cid = $("#cID").val();
    window.location.href = '/Exam/ExamList?CourseID=' + cid + '';
}

var ocUpdateCourse = function () {
    debugger
    var isAct = false;
    var cid = $("#cID").val();
    var cName = $("#courseName").val();
    var cDesc = $("#courseDesc").val();

    if ($('input.minimal').is(':checked')) {
        isAct = true;
    }
    if (cid != "" || cid != undefined) {
        $.ajax({
            type: "POST",
            url: "/Course/UpdateCourse",
            data: { "CourseID": cid, "CourseName": cName, "CourseDesc": cDesc, "IsActive": isAct },
            success: function (response) {
                var result = response.res;
                if (result == true) {
                    alert("Course successfully updated");
                } else {
                    alert("UNKNOWN ERROR!!!");
                }
            },
            error: function (response) { }
        });
    } else {
        window.href.location = "/Home/ViewCourse"
    }
}

var ApplyCourse = function () {
    var cid = $("#cID").val();
    if (cid != "" || cid != undefined || cid != null) {
        $.ajax({
            type: "POST",
            url: "/Course/ApplyEmployeebyCourseID",
            data: { "CourseID": cid },
            success: function (response) {
                var result = response.res;
                var status = response.sta;
                if (status != "RE") {
                    if (result == true) {
                        alert("You are successfully applied to the selected course");
                        window.location.href = '/Home/Index';
                    } else {
                        alert("ERROR: Apply course failed");
                    }
                } else {
                    alert("ERROR: You are already applied to the course.");
                }
            },
            error: function (response) { }
        });

    }

}

var ocLoadCourseUsers = function () {
    BindDataTable();
}

var otable;
var BindDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#CourseUserList")) {
        //Clear table for redraw
        otable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        otable = $("#CourseUserList").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/LoadUserEnrolledList",
            "fnServerData": function (sSource, aoData, fnCallback) {

                //For parameter
                var CourseID = $("#cID").val();
                aoData.push({ "name": "CourseID", "value": CourseID });

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
                    "className": "text-center", "targets": "_all",
                    "width": "200px"
                },
                {
                    "mData": "EmpName",
                    "orderable": false
                },
                {
                    "mData": "Department",
                    "orderable": false
                },
                {
                    "mData": "CompletionDate",
                    "orderable": false,
                    "className": "text-center", "targets": "_all",
                    "width": "200px",
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
            "searching": false,
            "bPaginate": false,
            "sScrollY": "300",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}
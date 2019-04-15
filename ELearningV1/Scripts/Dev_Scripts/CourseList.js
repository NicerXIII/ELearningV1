$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            PanelBoxStatus();
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
            CourseDataTable();
        },
        error: function (response) { }
    });
}

var ctable;
var CourseDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblCourse")) {
        //Clear table for redraw
        ctable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        ctable = $("#tblCourse").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/LoadCourseDetails",
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
                    "mData": "ID",
                    "orderable": false,
                    "className": "text-center", "targets": "_all",
                    "render": function (ID, type, full, meta) {
                        return '<a href="#/" onclick="ocSelectThisCourse(\'' + ID + '\')">Select</a> | <a href="#/" onclick="ocDeleteThisCourse(\'' + ID + '\')" style="color:#fd4a81;">Delete</a>';
                    }
                },
                {
                    "mData": "Course",
                    "orderable": false

                },
                {
                    "mData": "Description",
                    "orderable": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "searching": false,
            "bPaginate": false,
            "sScrollY": "500",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}

var etable;
var EmployeeDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblEmployeeEnrolled")) {
        //Clear table for redraw
        etable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        etable = $("#tblEmployeeEnrolled").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/LoadEmployeeByCourseID",
            "fnServerData": function (sSource, aoData, fnCallback) {

                var courseID = $("#cID").val();
                aoData.push({ "name": "CourseID", "value": courseID });

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
                    "render": function (EmployeeNumber, type, full, meta) {
                        return '<a href="#/" onclick="ocKickOutEmployee(\'' + EmployeeNumber + '\')" style="color:#fd4a81;"> <span></span>Remove</a>';
                    }
                },
                {
                    "mData": "EmpName",
                    "orderable": false

                },
                {
                    "mData": "CampiagnName",
                    "orderable": false
                },
                {
                    "mData": "Progress",
                    "orderable": false,
                    "render": function (Progress, type, full, meta) {
                        if (Progress >= 100) {
                            return 100;
                        }
                        return Progress;
                    }
                },
                {
                    "mData": "Score",
                    "orderable": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "searching": false,
            "bPaginate": false,
            "sScrollY": "500",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}


var ocSelectThisCourse = function (cid) {
    $("#cID").val(cid);
    EmployeeDataTable();
}

var ocDeleteThisCourse = function (cid) {
    $("#cID").val(cid);
    $("#lblDelCourse").removeClass('hidden');
    $("#lblDelEmp").addClass('hidden');
    $("#btnDeleteCourse").removeClass('hidden');
    $("#btnDeleteEmployee").addClass('hidden');
    $("#modalDeleteCourseListData").modal('show');
}

var ocDeleteCourse = function () {
    var cid = $("#cID").val();
    if (cid != "") {
        $.ajax({
            type: "POST",
            url: "/Course/DeleteCourseByCourseID",
            data: { "CourseID": cid },
            success: function (response) {
                var r = response.res;
                if (r == true) {
                    alert("Course is successfully deleted");
                    $("#modalDeleteCourseListData").modal('hide');
                    CourseDataTable();
                } else {
                    alert("Error: Please try again");
                }
            },
            error: function (response) { }
        });
    }

}

var ocKickOutEmployee = function (eid) {
    $("#eID").val(eid);
    $("#lblDelCourse").addClass('hidden');
    $("#lblDelEmp").removeClass('hidden');
    $("#btnDeleteCourse").addClass('hidden');
    $("#btnDeleteEmployee").removeClass('hidden');
    $("#modalDeleteCourseListData").modal('show');
}

var ocRemoveEmployee = function () {
    var eid = $("#eID").val();
    var cid = $("#cID").val();
    if (eid != "" && cid != "") {
        $.ajax({
            type: "POST",
            url: "/Course/RemoveEmployeeFromCourseByEMployeeNumber",
            data: { "EmployeeNumber": eid, "CourseID": cid },
            success: function (response) {
                var r = response.res;
                if (r == true) {
                    DeleteAllEmployeeAnswerByCourseID(eid, cid);
                } else {
                    alert("Error: Please try again");
                }
            },
            error: function (response) { }
        });
    }
}

function DeleteAllEmployeeAnswerByCourseID(eid, cid) {
    if (eid != "" && cid != "") {
        $.ajax({
            type: "POST",
            url: "/Course/EraseEmployeeAswerByCourseID",
            data: { "EmployeeNumber": eid, "CourseID": cid },
            success: function (response) {
                var r = response.res;
                if (r == true) {
                    alert("Employe removed successfully");
                    $("#modalDeleteCourseListData").modal('hide');
                    EmployeeDataTable();
                } else {
                    alert("Error: Please try again");
                }
            },
            error: function (response) { }
        });
    }
}
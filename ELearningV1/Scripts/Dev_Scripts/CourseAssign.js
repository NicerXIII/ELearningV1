$(document).ready(function () {
    LoadingData();
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            EmployeeListDataTable();
        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }

    });
});

var etable;
var EmployeeListDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblEmployeeList")) {
        //Clear table for redraw
        etable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        etable = $("#tblEmployeeList").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/GetAllEpmployees",
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
                    "width": "24px",
                    "className": "text-center", "targets": "_all",
                    "render": function (EmployeeNumber, type, full, meta) {
                        return '<a href="#/" onclick="ocSelectEmployee(\'' + EmployeeNumber + '\')">Select</a>';
                    }
                },
                {
                    "mData": "EmpName",
                    "orderable": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "bPaginate": false,
            "sScrollY": "500",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
        EndLoading();
    }
}

var ctable;
var CourseListDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblCourseList")) {
        //Clear table for redraw
        ctable.draw();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        ctable = $("#tblCourseList").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/GetAllCourseAvailable",
            "fnServerData": function (sSource, aoData, fnCallback) {

                var EmployeeNumber = $("#EmpNumber").val();

                aoData.push({ "name": "EmployeeNumber", "value": EmployeeNumber });

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
                    "width": "24px",
                    "className": "text-center", "targets": "_all",
                    "render": function (ID, type, full, meta) {
                        return '<a href="#/" onclick="ocSelectCourseAvailable(\'' + ID + '\')">Select</a>';
                    }
                },
                {
                    "mData": "Course",
                    "orderable": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "bPaginate": false,
            "sScrollY": "500",
            "scrollX": true,
            "bScrollCollapse": false,
            "searching": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}

var actable;
var AppliedCourseDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblAppliedCourse")) {
        //Clear table for redraw
        actable.draw();
        EndLoading();
    } else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");
        actable = $("#tblAppliedCourse").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/GetAllCourseApplied",
            "fnServerData": function (sSource, aoData, fnCallback) {

                var EmployeeNumber = $("#EmpNumber").val();

                aoData.push({ "name": "EmployeeNumber", "value": EmployeeNumber });

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
                    "width": "24px",
                    "className": "text-center", "targets": "_all",
                    "render": function (ID, type, full, meta) {
                        return '<a href="#/" onclick="ocRemoveSelectedCourse(\'' + ID + '\')" style="color:#fd4a81;">Remove</a>';
                    }
                },
                {
                    "mData": "Course",
                    "orderable": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "bPaginate": false,
            "sScrollY": "500",
            "scrollX": true,
            "bScrollCollapse": false,
            "searching": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
        EndLoading();
    }
}

var ocSelectEmployee = function (EmpID) {
    LoadingData();
    $("#EmpNumber").val(EmpID);
    if (EmpID != "") {
        CourseListDataTable();
        AppliedCourseDataTable();
    }
}

var ocSelectCourseAvailable = function (CourseID) {
    LoadingData();
    var empID = $("#EmpNumber").val();
    if (CourseID != "" || CourseID != undefined && EmpID != "") {
        $.ajax({
            type: "POST",
            url: "/Course/AddEmployeeToCourse",
            data: { "EmployeeNumber": empID, "CourseID": CourseID },
            success: function (response) {
                var result = response.res;
                if (result == true) {
                    CourseListDataTable();
                    AppliedCourseDataTable();
                }
            },
            error: function (response) { }
        });
    }
}

var ocRemoveSelectedCourse = function (ID) {
    LoadingData();
    if (ID != "" || ID != undefined) {
        $.ajax({
            type: "POST",
            url: "/Course/RemoveEmployeeCourse",
            data: { "ID": ID },
            success: function (response) {
                var result = response.res;
                if (result == true) {
                    CourseListDataTable();
                    AppliedCourseDataTable();
                }
            },
            error: function (response) { }
        });
    }
}
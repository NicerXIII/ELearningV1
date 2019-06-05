$(document).ready(function () {
    LoadingData();
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            AppliedCourseDataTable();
        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }
    });
});

var actable;
var AppliedCourseDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblAppliedCourse")) {
        //Clear table for redraw
        actable.draw();
        EndLoading();
    }
    else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");

        actable = $("#tblAppliedCourse").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/GetAllCourseApplied",
            "fnServerData": function (sSource, aoData, fnCallback) {

                var EmployeeNumber = $("#EmployeeNumber").val();
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
                    "visible": true,
                    //"className": "text-center", "targets": "_all",
                    "render": function (ID, type, full, meta) {
                        return '<a href="#/" onclick="SelectCourse(\'' + ID + '\')" style="color:#fd4a81;">Select</a>';
                    }
                },
                {
                    "mData": "Course",
                    "orderable": false,
                    //"width": "100px"
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": false,
            "bPaginate": false,
            "sScrollY": "300",
            //"scrollX": false,
            "bScrollCollapse": false,
            "searching": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
        EndLoading();
    }
}

var SelectCourse = function (ID) {
    $("#CourseID").val(ID);
    GetVideoList();
}

var SrcFileArray = [];
var FileTitleArray = [];

var vtable;
var GetVideoList = function () {        //if table exist
    if ($.fn.DataTable.isDataTable("#tblVideoList")) {
        //Clear table for redraw
        vtable.draw();
        EndLoading();
    }
    else {
        moment.tz.add("Asia/Manila|+08 +09|-80 -90|010101010|-1kJI0 AL0 cK10 65X0 mXB0 vX0 VK10 1db0|24e6");

        vtable = $("#tblVideoList").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Course/GetVideoList",
            "fnServerData": function (sSource, aoData, fnCallback) {

                var CourseID = $("#CourseID").val();
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
                    "mData": "CourseID",
                    "orderable": false,
                    "width": "24px",
                    "visible": true,
                    "render": function (SrcFile, type, full, meta) {
                        SrcFileArray.push(full.SrcFile);
                        FileTitleArray.push(full.Title);
                        return '<a href="#/" onclick="ViewVideo(\'' + full.SrcFile + '\',\'' + full.Title + '\')" style="color:#fd4a81;">View</a>';
                    }
                },
                {
                    "mData": "Course",
                    "orderable": false,
                    "width": "100px",
                    "visible": false
                },
                {
                    "mData": "CourseSecID",
                    "orderable": false,
                    //"width": "24px",
                    "visible": false
                },
                {
                    "mData": "Title",
                    "orderable": false,
                    //"width": "100px"
                },
                {
                    "mData": "SrcFile",
                    "orderable": false,
                    //"width": "100px",
                    "visible": false
                }
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": false,
            "bPaginate": false,
            "sScrollY": "300",
            //"scrollX": false,
            "bScrollCollapse": false,
            "searching": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
        EndLoading();
    }
}

var ViewVideo = function (sourceFile, Title) {
    //alert(SrcFileArray.toString());
    $("#labelTitle").text(Title);
    document.getElementById("videoTuitorial").src = sourceFile;
    $("#modalDisplayVideoSelected").modal({
        backdrop: "static",
        keyboard: false
    });
}

var StopPlayingVideo = function () {
    var vid = document.getElementById("videoTuitorial");
    vid.pause();
}

var Previous = function () {
    debugger
    StopPlayingVideo();
    var currentVideo = document.getElementById("videoTuitorial").src;
    var res, res2, Index, newIndex;
    //Localhost
    if (currentVideo.includes("localhost")) {
        res = currentVideo.replace("http://localhost:50968", "");
        res2 = decodeURI(res);
        Index = SrcFileArray.indexOf(res2);

        if (Index >= 0) { newIndex = Index - 1; }
        else { newIndex = 0; }
    }
    //Testing site
    else if (currentVideo.includes("http://warlock:96")) {
        res = currentVideo.replace("http://warlock:96", "");
        res2 = decodeURI(res);
        Index = SrcFileArray.indexOf(res2);

        if (Index >= 0) { newIndex = Index - 1; }
        else { newIndex = 0; }
    }
    //Production
    else {
        res = currentVideo.replace("http://warlock:98", "");
        res2 = decodeURI(res);
        Index = SrcFileArray.indexOf(res2);

        if (Index >= 0) { newIndex = Index - 1; }
        else { newIndex = 0; }
    }

    if (newIndex >= 0) {
        var getPrevVideo = SrcFileArray[newIndex];
        var getPrevTitle = FileTitleArray[newIndex];
        document.getElementById("videoTuitorial").src = getPrevVideo;
        document.getElementById("labelTitle").innerHTML = getPrevTitle;
    }
}

var Next = function () {
    debugger
    StopPlayingVideo();
    var currentVideo = document.getElementById("videoTuitorial").src;
    var res, res2, Index, newIndex;
    if (currentVideo.includes("localhost")) {
        res = currentVideo.replace("http://localhost:50968", "");
        res2 = decodeURI(res);
        Index = SrcFileArray.indexOf(res2);

        if (Index >= 0) { newIndex = Index + 1; }
    }
    //Testing site
    else if (currentVideo.includes("http://warlock:96")) {
        res = currentVideo.replace("http://warlock:96", "");
        res2 = decodeURI(res);
        Index = SrcFileArray.indexOf(res2);

        if (Index >= 0) { newIndex = Index + 1; }
        else { newIndex = 0; }
    }
    else {
        res = currentVideo.replace("http://warlock:98", "");
        res2 = decodeURI(res);
        Index = SrcFileArray.indexOf(res2);

        if (Index >= 0) { newIndex = Index + 1; }
        else { newIndex = 0; }
    }

    if (newIndex >= 0) {
        var getPrevVideo = SrcFileArray[newIndex];
        var getPrevTitle = FileTitleArray[newIndex];
        document.getElementById("videoTuitorial").src = getPrevVideo;
        document.getElementById("labelTitle").innerHTML = getPrevTitle;
    }
}
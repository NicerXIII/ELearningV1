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

var CourseDetailReport = function ()
{ window.location.href = '@Url.Action("CourseDetailReport", "Course")'; }

//var ApplyCourse = function () {
//    var cid = $("#cID").val();
//    if (cid != "" || cid != undefined || cid != null) {
//        $.ajax({
//            type: "POST",
//            url: "/Course/ApplyEmployeebyCourseID",
//            data: { "CourseID": cid},
//            success: function (response) {
//                var result = response.res;
//                if (result == true) {
//                    alert("You are successfully applied to the selected course");
//                    window.location.href = '/Home/Index';
//                } else {
//                    alert("ERROR: Apply course failed")
//                }
//            },
//            error: function (response) { }
//        });

//    }

//}


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
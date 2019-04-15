$(document).ready(function () {
    LoadingData();
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            $("#reloadViewCourse").load("/Home/LoadCourseData #reloadViewCourse");
            PanelBoxStatus();
            EndLoading();
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

var ocViewDetails = function (courseID) {
    //courseID.String.hashCode();
    window.location.href = '/Course/CourseDetail?CourseID=' + courseID + '';
}

String.prototype.hashCode = function () {
    var hash = 0, i, chr;
    if (this.length === 0) return hash;
    for (i = 0; i < this.length; i++) {
        chr = this.charCodeAt(i);
        hash = ((hash << 5) - hash) + chr;
        hash |= 0; // Convert to 32bit integer
    }
    return hash;
};

var ocTakeExam = function (courseID) {
    //alert(courseID);
}

$("#searchCourse").change(function () {
    var search = $("#searchCourse").val();
    $("#reloadViewCourse").load("/Home/LoadCourseDataByCourseName?CName=" + search + " #reloadViewCourse");
})

var ocEmployeeViewDetails = function (courseID) {
    window.location.href = '/Course/EmployeeCourseDetail?CourseID=' + courseID + '';
}
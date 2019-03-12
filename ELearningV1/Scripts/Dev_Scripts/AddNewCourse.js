var ocAddNewCourse = function () {
    var imageName = $("#imageName").val();
    var cname = $("#CName").val();
    var cdesc = $("#CDesc").val();
    var days = $("#MinDay").val();
    if (imageName != "" && cname != "" && cdesc != "") {
        var file = $("#CourseImage").get(0).files;
        var data = new FormData;
        data.append("ImageFile", file[0]);
        $.ajax({
            type: "POST",
            url: "/Course/ImageUpload?CName=" + cname + "&CDesc=" + cdesc + "&Days1=" + days + "",
            data: data,
            contentType: false,
            processData: false,
            success: function (response) {
                var result = response.res;
                if (result == "True") {
                    alert("New course successfully added");
                    $("#modalAddCourse").modal('hide');
                    $("#imageName").val("");
                    $("#CName").val("");
                    $("#CDesc").val("");
                    $("#CourseImage").val("");
                    $("#CoursePic").attr('src', '/UpLoadedImages/imagesaaaaa.jpg');
                    $("#reloadViewCourse").load("/Home/LoadCourseData #reloadViewCourse");
                } else {
                    alert("ERROR: Adding new course failed");
                }
            },
            error: function () { }
        });
    } else {
        alert("Please enter all fields including the image of the course");
    }
}

$("#CourseImage").change(function () {
    debugger
    var File = this.files;
    if (File && File[0]) {
        ReadImage(File[0]);
    }
})

var ReadImage = function (file) {
    debugger
    var ImageName = file.name;
    $("#imageName").val(ImageName);
    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {
        image.src = _file.target.result;
        image.onload = function () {
            $("#CoursePic").attr('src', _file.target.result);
        }
    }
}


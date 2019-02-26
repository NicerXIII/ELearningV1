$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            BindDataTable();
        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }

    });
});

var ocUploadExamVid = function () {
    $("#modalUploadVid").modal("show");
}

    //var ocGenerateLink = function () {
    //    debugger
    //    var linkText = $("#linkGenText").val();
    //    var n = linkText.indexOf("watch?v=");
    //    var link1 = "https://www.youtube.com/embed/" + linkText.substr(n + 8);

    //    if (linkText.indexOf("list") > 0) {
    //        alert("Please enter youtube link that is not in list mode");
    //    } else {
    //        $("#vid123").attr('src', link1);
    //        $("#vid123").removeClass("hidden");
    //    }

    //}

$("#VidImage").change(function () {
    var File = this.files;
    if (File && File[0]) {
        VideoImage(File[0]);
    }
})

$("#PreImage").change(function () {
    var File = this.files;
    if (File && File[0]) {
        VideoImage(File[0]);
    }
})

var VideoImage = function (file) {
    var ImageName = file.name;
    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {
        image.src = _file.target.result;
        image.onload = function () {
        }
    }

}

var ocSaveVideoData = function () {
    debugger
    var title = $("#SourceTitle").val();
    var cid = $("#courseID1").val();

    if (imageName != "" && title != "" && cid != "") {
        var file = $("#VidImage").get(0).files;
        var data = new FormData;
        data.append("ImageFile", file[0]);
        var type1 = file[0].type;
        if (type1.includes("mp4")) {
            $.ajax({
                type: "POST",
                url: "/Exam/SaveVideoData?CID=" + cid + "&Title=" + title + "",
                data: data,
                contentType: false,
                processData: false,
                success: function (response) {
                    var res = response.res;
                    if (res == true) {
                        alert("Video uploaded successfully.");
                        var getVideo = document.getElementById("vidPlayer");
                        $("#vidSource").attr('src', response.vName);
                        getVideo.load()
                        getVideo.play();
                        getVideo.volume = 0.5;

                        $("#SourceTitle").val("");
                        $("#VidImage").val("");
                        BindDataTable();
                    } else {
                        alert("Video upload failed");
                    }
                },
                error: function (response) {
                    alert("ERROR: please select video again.");
                }
            });
        } else {
            alert('ERROR: File format not supported');
        }
    } else {
        alert("Please fill up all fields");
    }
}

var ocCloseUploadVideoModal = function () {
    var getVideo = document.getElementById("vidPlayer");
    getVideo.pause();
    $("#modalUploadVid").modal("hide");
}

var ocUploadExamPresentation = function () {
    $("#modalUploadPresentation").modal('show');
}

var ocSavePresentationData = function () {
    var title = $("#PresTitle").val();
    var cid = $("#courseID1").val();

    if (title != "" && cid != "") {
        var file = $("#PreImage").get(0).files;
        var data = new FormData;
        data.append("ImageFile", file[0]);
        var type1 = file[0].type;


        if (type1.includes("pdf")) {
            $.ajax({
                type: "POST",
                url: "/Exam/SavePPTData?CID=" + cid + "&Title=" + title + "",
                data: data,
                contentType: false,
                processData: false,
                success: function (response) {
                    var res = response.res;
                    if (res == true) {
                        alert("PDF file uploaded successfully.");
                        $("#pdfPres").attr('src', response.pName);
                        $("#pdfPres").removeClass("hidden");
                        $("#PresTitle").val("");
                        $("#PreImage").val("");
                        BindDataTable();
                    } else {
                        alert("File upload failed");
                    }
                },
                error: function (response) {
                    alert("ERROR: please select pdf file again..");
                }
            });
        } else {
            alert("ERROR: Invalid pdf format.");
        }
    } else {
        alert("Please fill up all fields");
    }


}

var ocCloseUploadPresModal = function () {
    $("#PresTitle").val('');
    $("#PreImage").val('');
    $("#modalUploadPresentation").modal('hide');

}

var otable;
var BindDataTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblSection")) {
        //Clear table for redraw
        otable.draw();
    } else {

        otable = $("#tblSection").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Exam/LoadSectionData",
            "fnServerData": function (sSource, aoData, fnCallback) {

                //For Parameter
                var courseID = $("#courseID1").val();

                aoData.push({ "name": "courseID", "value": courseID });

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
                    "width": "10px",
                    "className": "dt-center", "targets": "_all",
                    "orderable": false,
                    "render": function (ID, type, full, meta) {
                        return '<a href="#/" style="font-size:20px; color:red;" onclick="ocDelete(\'' + ID + '\',\'' + full.Title + '\')"><span class="fa fa-trash"></span></a>';
                    }
                },
                {
                    "mData": "Title",
                    "orderable": false,
                    "render": function (Title, type, full, meta) {
                        debugger
                        if (full.Type == "Test") {
                            return '<a href="#/" onclick="ocCreateViewQuiz(\'' + full.ID + '\',\'' + full.CourseID + '\')"> <i class="glyphicon glyphicon-check"></i> ' + " " + Title + '</a>';
                        } else if (full.Type != "Test" && full.Type != "Video" && full.Type == "PDF") {
                            return '<a href="#/"><i class="fa fa-file-pdf-o"></i>' + " " + Title + '</a>';
                        } else {
                            return '<a href="#/"><i class="fa fa-film"></i>' + " " + Title + '</a>';
                        }

                    }
                },
                {
                    "mData": "Type",
                    "visible": false,
                    "orderable": false
                },
                {
                    "mData": "SrcFile",
                    "visible": false,
                    "orderable": false
                },
                {
                    "mData": "CourseID",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "OrderSec",
                    "orderable": false,
                    "width": "25px",
                    "render": function (OrderSec, type, full, meta) {
                        return '<input type="text" style="width:40px; text-align:center;" id="oderBy' + full.ID + '" class="" value="' + OrderSec + '" onchange="ochOrderBy(\'' + full.ID + '\')" />';
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

var ochOrderBy = function (sectionID) {
    debugger
    var secID = sectionID;
    var orderVal = $("#oderBy" + sectionID).val();
    if (secID != "" || secID != undefined) {
        $.ajax({
            type: "POST",
            url: "/Exam/UpdateSectionOrderingByID",
            data: { "SID": secID, "OrderVal": orderVal },
            success: function (response) {
                BindDataTable();
            },
            error: function (response) { }
        });
    }
}

var ocDelete = function (secID, title) {
    $("#sectionID").val(secID);
    $("#sectionName").text(title);
    $("#modalDeleteSection").modal('show');
}

var ocDeleteSelectedSection = function () {
    var secID = $("#sectionID").val();
    if (secID != "") {
        $.ajax({
            type: "POST",
            url: "/Exam/DeleteSectionByID",
            data: { "SID": secID },
            success: function (response) {
                alert("Section deleted successfully.");
                $("#modalDeleteSection").modal('hide');
                BindDataTable();
            },
            error: function (response) { }
        });
    } else {
        alert("Please select again");
        $("#modalDeleteSection").modal('hide');
    }
}
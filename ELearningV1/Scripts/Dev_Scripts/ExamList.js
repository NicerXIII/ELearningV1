$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Home/GetSession",
        data: {},
        success: function (response) {
            BindDataTable();
            //$.ajax({
            //    type: "POST",
            //    url: "/Exam/GetDateEnrolled",
            //    data: {},
            //    success: function (response) {
            //        if (response._res === "Expired")
            //        {
            //            document.getElementById("btnStart").style.display = "none";
            //            document.getElementById("btnStart").disabled = true;
            //            //window.location.href = "../Home/Index";                        
            //        }
            //        else
            //        {   }
            //    },
            //});
        },
        error: function (response) {
            window.location.href = '/LogIn/Index';
        }

    });
});

var ocUploadExamVid = function () {
    $("#vidPlayer").addClass('hidden');
    $("#VideoOnly").removeClass('hidden');
    $("#VideoWithExam").addClass('hidden');
    $("#modalUploadVid").modal("show");
}

var ocUploadExamVidWithExam = function () {
    $("#vidPlayer").addClass('hidden');
    $("#VideoWithExam").removeClass('hidden');
    $("#VideoOnly").addClass('hidden');
    $("#modalUploadVid").modal("show");
}

$("#VidImage").change(function () {
    debugger
    LoadingData();
    var File = this.files;
    if (File && File[0]) {
        VideoImage(File[0]);
    }
})

$("#PreImage").change(function () {
    LoadingData();
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
        EndLoading();
    }

}

var ocSaveVideoData = function () {
    debugger
    LoadingData();
    var title = $("#SourceTitle").val();
    var cid = $("#courseID1").val();
    var type = "Vid";
    if (imageName != "" && title != "" && cid != "") {
        var file = $("#VidImage").get(0).files;
        var data = new FormData;
        data.append("ImageFile", file[0]);
        var type1 = file[0].type;
        if (type1.includes("mp4")) {
            $.ajax({
                type: "POST",
                url: "/Exam/SaveVideoData?CID=" + cid + "&Title=" + title + "&Type1=" + type,
                data: data,
                contentType: false,
                processData: false,
                success: function (response) {
                    var res = response.res;
                    if (res == true) {
                        alert("Video uploaded successfully.");
                        EndLoading();
                        $("#vidPlayer").removeClass('hidden');
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
                        EndLoading();
                    }
                },
                error: function (response) {
                    alert("ERROR: please select video again.");
                    EndLoading();
                }
            });
        } else {
            EndLoading();
            alert('ERROR: File format not supported');
        }
    } else {
        EndLoading();
        alert("Please fill up all fields");
    }
}

var ocSaveVideoWithExamData = function () {
    debugger
    LoadingData();
    var title = $("#SourceTitle").val();
    var cid = $("#courseID1").val();
    var type = "VidExam";
    if (imageName != "" && title != "" && cid != "") {
        var file = $("#VidImage").get(0).files;
        var data = new FormData;
        data.append("ImageFile", file[0]);
        var type1 = file[0].type;
        if (type1.includes("mp4")) {
            $.ajax({
                type: "POST",
                url: "/Exam/SaveVideoData?CID=" + cid + "&Title=" + title + "&Type1=" + type,
                data: data,
                contentType: false,
                processData: false,
                success: function (response) {
                    var res = response.res;
                    if (res == true) {
                        alert("Video uploaded successfully.");
                        EndLoading();
                        $("#vidPlayer").removeClass('hidden');
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
                        EndLoading();
                    }
                },
                error: function (response) {
                    alert("ERROR: please select video again.");
                    EndLoading();
                }
            });
        } else {
            EndLoading();
            alert('ERROR: File format not supported');
        }
    } else {
        EndLoading();
        alert("Please fill up all fields");
    }
}

var ocCloseUploadVideoModal = function () {
    var getVideo = document.getElementById("vidPlayer");
    getVideo.pause();
    $("#modalUploadVid").modal("hide");
}

var ocUploadExamPresentation = function () {
    $("#pdfPres").addClass('hidden');
    $("#modalUploadPresentation").modal('show');
}

var ocSavePresentationData = function () {
    debugger
    LoadingData();
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
                        EndLoading();
                        BindDataTable();
                    } else {
                        alert("File upload failed");
                        EndLoading();
                    }
                },
                error: function (response) {
                    alert("ERROR: please select pdf file again..");
                    EndLoading();
                }
            });
        } else {
            alert("ERROR: Invalid pdf format.");
            EndLoading();
        }
    } else {
        EndLoading();
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
    }
    else {

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
                        if (full.Type == "Test") {
                            // INSIDE viewQuestionList nakalagay
                            //  \'' + full.ID + '\',\'' + full.CourseID + '\'
                            return '<a href="#/" onclick="viewQuestionList(\'' + full.ID + '\',\'' + full.CourseID + '\',\'' + Title + '\')"> <i class="glyphicon glyphicon-check"></i> ' + " " + Title + '</a>';
                        } else if (full.Type != "Test" && full.Type != "Video" && full.Type == "VideoExam")
                        {
                            return '<a href="#/" onclick="viewQuestionList(\'' + full.ID + '\',\'' + full.CourseID + '\',\'' + Title + '\')"> <span class="fa fa-film">&nbsp;</span><i class="glyphicon glyphicon-check"></i> ' + " " + Title + '</a>';
                        }
                        else if (full.Type != "Test" && full.Type != "Video" && full.Type == "PDF") {
                            return '<a href="#/"><i class="fa fa-file-pdf-o"></i>' + " " + Title + '</a>';
                        }
                        else {
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
            "sScrollY": "250",
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

/////////////////////////////////////////////
var totalAns = "";
var totalAns2 = "";

$(function () {
    $('.textarea').wysihtml5();
});

var ocNewTest = function () { $("#modalTestTitle").modal('show'); }

var ocCloseCreateNewTest = function () {
    $("#TestTitle").val("");
    $("#modalTestTitle").modal('hide');
}

$("#ddquestionType").change(function () {
    var selected = $("#ddquestionType option:selected").val();
    var b = document.getElementById("mc");
    var c = document.getElementById("mcoa");

    if (selected === "2") {
        b.style.display = "block";
        //a.style.display = "none";
        c.style.display = "none";
    }
    else if (selected === "3") {
        c.style.display = "block";
        b.style.display = "none";
        //a.style.display = "none";
    }

});

var ocSaveTestTitle = function () {
    var title = $("#TestTitle").val();
    var cid = $("#courseID1").val();
    if (title != "" && cid != "") {
        $.ajax({
            type: "POST",
            url: "/Exam/SaveTestTitle",
            data: { "CID": cid, "Title": title },
            success: function (response) {
                alert(response.res);
                $("#modalTestTitle").modal('hide');
                $("#TestTitle").val("");
                $("#modalTestTitle").modal('hide');
                BindDataTable();
            },
            error: function (response) { }
        });
    }
}

// show modal that show created question list
var viewQuestionList = function (ID, CourseID, Title) {
    $("#sectionID").val(ID);
    $("#secID").val(ID);
    $("#testName").val(Title);
    setTimeout(function () { BindQuestionTable(); }, 1000);
    //BindQuestionTable(ID);
    $("#modalTestQuestion").modal('show');
};

//show another modal for adding new question
var addQuestion = function () {
    debugger
    var getID = $("#secID").val();
    var courID = $("#courseID1").val();

    $("#labelCourSecID").val(getID);
    $("#labelCourID").val(courID);

    $("#addQuestionaire").modal('show');
}

////////////FOR MULTIPLE ANSWER ONLY////////////////
$("#chckMCOne").change(function () {
    var data1 = $("#txtMCOne").val();//this.id;
    if (document.getElementById("chckMCOne").checked) {
        if (totalAns === "" || totalAns === null || totalAns === " ") {
            if (totalAns.includes(data1)) { }
            else { totalAns = data1; }
        }
        else {
            if (totalAns.includes(data1)) { }
            else { totalAns = totalAns + "," + data1; }
        }
    }
});

$("#chckMCTwo").change(function () {
    var data2 = $("#txtMCTwo").val();//this.id;
    if (document.getElementById("chckMCTwo").checked) {
        if (totalAns === "" || totalAns === null || totalAns === " ") {
            if (totalAns.includes(data2)) { }
            else { totalAns = data2; }
        }
        else {
            totalAns = totalAns + "," + data2;
        }
    }
});

$("#chckMCThree").change(function () {
    var data3 = $("#txtMCThree").val();//this.id;
    if (document.getElementById("chckMCThree").checked) {
        if (totalAns === "" || totalAns === null || totalAns === " ") {
            if (totalAns.includes(data3)) { }
            else { totalAns = data3; }
        }
        else {
            if (totalAns.includes(data3)) { }
            else { totalAns = totalAns + "," + data3; }

        }
    }
});

$("#chckMCFour").change(function () {
    var data4 = $("#txtMCFour").val();
    if (document.getElementById("chckMCFour").checked) {
        if (totalAns === "" || totalAns === null || totalAns === " ") {
            if (totalAns.includes(data4)) { }
            else { totalAns = data4; }
        }
        else {
            if (totalAns.includes(data4)) { }
            else { totalAns = totalAns + "," + data4; }
        }
    }
});
////////////FOR MULTIPLE ANSWER ONLY////////////////

////////////FOR ONE ANSWER ONLY////////////////
$("#chckMCOA1").change(function () {
    var data1 = $("#txtMCOA1").val();

    if (totalAns2.includes(data1)) { }
    else { totalAns2 = data1; }
});

$("#chckMCOA2").change(function () {
    var data2 = $("#txtMCOA2").val();

    if (totalAns2.includes(data2)) { }
    else { totalAns2 = data2; }
});

$("#chckMCOA3").change(function () {
    var data3 = $("#txtMCOA3").val();

    if (totalAns2.includes(data3)) { }
    else { totalAns2 = data3; }
});

$("#chckMCOA4").change(function () {
    var data4 = $("#txtMCOA4").val();

    if (totalAns2.includes(data4)) { }
    else { totalAns2 = data4; }
});
////////////FOR ONE ANSWER ONLY////////////////

var saveQuestion = function () {
    debugger

    var ques = $("#textQuestion").val();
    var selected = $("#ddquestionType option:selected").val();
    var getCourSec = $("#secID").val();
    var getCourID = $("#courseID1").val();

    var Ans1 = "";
    var Ans2 = "";
    var Ans3 = "";
    var Ans4 = "";

    // multiple answer || find where are the correct answer
    if (selected === "2") {
        Ans1 = $("#txtMCOne").val();
        Ans2 = $("#txtMCTwo").val();
        Ans3 = $("#txtMCThree").val();
        Ans4 = $("#txtMCFour").val();
    }

    // one answere || to find where is the correct answer
    else if (selected === "3") {
        Ans1 = $("#txtMCOA1").val();
        Ans2 = $("#txtMCOA2").val();
        Ans3 = $("#txtMCOA3").val();
        Ans4 = $("#txtMCOA4").val();
    }

    if (selected === "2") {
        $.ajax({
            type: "POST",
            url: "/Exam/saveQuestion",
            data: { "Question": ques, "QuestionType": selected, "Answer1": Ans1, "Answer2": Ans2, "Answer3": Ans3, "Answer4": Ans4, "CorrectAns": totalAns, "CourseSecID": getCourSec, "CourseID": getCourID },
            success: function (response) {
                debugger
                alert(response.res);
                $("#addQuestionaire").modal('hide');
                BindQuestionTable(getCourSec);
                $("#txtMCOne").val("");
                $("#txtMCTwo").val("");
                $("#txtMCThree").val("");
                $("#txtMCFour").val("");
                $("#textQuestion").val("");
                totalAns = "";
                document.getElementById("chckMCOne").checked = false;
                document.getElementById("chckMCTwo").checked = false;
                document.getElementById("chckMCThree").checked = false;
                document.getElementById("chckMCFour").checked = false;
            },
            error: function (response) { }
        });
    }
    if (selected === "3") {
        $.ajax({
            type: "POST",
            url: "/Exam/saveQuestion",
            data: { "Question": ques, "QuestionType": selected, "Answer1": Ans1, "Answer2": Ans2, "Answer3": Ans3, "Answer4": Ans4, "CorrectAns": totalAns2, "CourseSecID": getCourSec, "CourseID": getCourID },
            success: function (response) {
                debugger
                alert(response.res);
                $("#addQuestionaire").modal('hide');
                BindQuestionTable(getCourSec);
                $("#txtMCOA1").val("");
                $("#txtMCOA2").val("");
                $("#txtMCOA3").val("");
                $("#txtMCOA4").val("");
                $("#textQuestion").val("");
                totalAns2 = "";
                document.getElementById("chckMCOA1").checked = false;
                document.getElementById("chckMCOA2").checked = false;
                document.getElementById("chckMCOA3").checked = false;
                document.getElementById("chckMCOA4").checked = false;
            },
            error: function (response) { }
        });
    }
}

var itable;
var BindQuestionTable = function () {
    //if table exist
    if ($.fn.DataTable.isDataTable("#tblQuestions")) {
        //Clear table for redraw
        itable.draw();
    }
    else {

        itable = $("#tblQuestions").DataTable({
            "bServerSide": true,
            "processing": true,
            "sAjaxSource": "/Exam/getQuestionList",
            "fnServerData": function (sSource, aoData, fnCallback) {
                var a = $("#sectionID").val();

                aoData.push({ "name": "CourSecID", "value": a });

                $.ajax({
                    type: "Get",
                    data: aoData,
                    url: sSource,
                    success: fnCallback
                });
            },
            "aoColumns": [
                {
                    "mData": "OrderNumber",
                    "orderable": false,
                    "width": "25px",
                    "render": function (OrderNumber, type, full, meta) {
                        return '<input type="text" id="txtOrderNo' + full.ID + '" style="width:40px; text-align:center;" value="' + OrderNumber + '" onchange=changeQuestOrderNo("' + full.ID + '") />';
                    }
                },
                {
                    "mData": "Question",
                    "orderable": false,
                    "width": "500px",
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "QuestionType",
                    "orderable": false,
                    "width": "50px",
                    "className": "dt-center", "targets": "_all",
                    "render": function (QuestionType, type, full, meta) {
                        if (QuestionType == "2") {
                            return '<i class="fa fa-check-square" title="Multiple Choice"></i>';
                        }
                        else {
                            return '<i class="fa fa-dot-circle-o" title="Radio Button"></i>';
                        }
                    }
                },
                {
                    "mData": "C1",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "C2",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "C3",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "C4",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CAnswer1",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CAnswer2",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CAnswer3",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CAnswer4",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CourseSectionID",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "CourseID",
                    "orderable": false,
                    "visible": false,
                    "className": "dt-center", "targets": "_all"
                },
                {
                    "mData": "ID",
                    "orderable": false,
                    "width": "100px",
                    "render": function (ID, type, full, meta) {
                        return '<a href="#/" onclick="EditQuestion(\'' + ID + '\',\'' + full.Question + '\',\'' + full.QuestionType + '\',\'' + full.C1 + '\',\'' + full.C2 + '\',\'' + full.C3 + '\',\'' + full.C4 + '\',\'' + full.CAnswer1 + '\',\'' + full.CAnswer2 + '\',\'' + full.CAnswer3 + '\',\'' + full.CAnswer4 + '\',\'' + full.CourseSectionID + '\',\'' + full.CourseID + '\')"> <i class="fa fa-pencil" title="Edit"></i> ' + " " + "Edit" + '</a> | <a href="#/" onclick="DeleteSelectedQuestion(\'' + ID + '\')"> <i class="fa fa-times" title="Delete"></i> ' + " " + "Delete" + '</a>';
                    }
                },
            ],
            responsive: true,
            select: { style: 'single' },
            "bInfo": true,
            "searching": false,
            "bPaginate": false,
            "sScrollY": "200",
            "scrollX": true,
            "bScrollCollapse": false,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]]
        });
    }
}

var EditQuestion = function (ID, Question, Type, C1, C2, C3, C4, CAns, CAns2, CAns3, CAns4, CourSecID, CouID) {
    //alert("Edit: " + " ID: " + ID + ' & ' + "Question: " + Question + ' & ' + "Type: " + Type + ' & ' + "Choice1: " + C1 + ' & ' + "Choice2: " + C2 + ' & ' + "Choice3: " + C3 + ' & ' + "Choice4: " + C4 + ' & ' + "Correct Answer: " + CAns + ' & ' + "CourSecID: " + CourSecID + ' & ' + "CourID: " + CouID);
    debugger
    document.getElementById("chckMCOne").checked = false;
    document.getElementById("chckMCTwo").checked = false;
    document.getElementById("chckMCThree").checked = false;
    document.getElementById("chckMCFour").checked = false;

    document.getElementById("chckMCOA1").checked = false;
    document.getElementById("chckMCOA2").checked = false;
    document.getElementById("chckMCOA3").checked = false;
    document.getElementById("chckMCOA4").checked = false;

    $("#textQuestion").val(Question);
    $("#QuestionID").val(ID);
    document.getElementById("ddquestionType").selectedIndex = Type - 1;

    var b = document.getElementById("mc");
    var c = document.getElementById("mcoa");

    if (Type == "2") {
        b.style.display = "block";
        c.style.display = "none";

        $("#txtMCOne").val(C1);
        $("#txtMCTwo").val(C2);
        $("#txtMCThree").val(C3);
        $("#txtMCFour").val(C4);

        //var temp = new Array();
        //temp = CAns.split(",");

        //for (var data in temp) {
        //alert(temp[data]);
        if (CAns === C1 || CAns2 === C1 || CAns3 === C1 || CAns4 === C1) {
            document.getElementById("chckMCOne").checked = true;
        }
        if (CAns === C2 || CAns2 === C2 || CAns3 === C2 || CAns4 === C2) {
            document.getElementById("chckMCTwo").checked = true;
        }
        if (CAns === C3 || CAns2 === C3 || CAns3 === C3 || CAns4 === C3) {
            document.getElementById("chckMCThree").checked = true;
        }
        if (CAns === C4 || CAns2 === C4 || CAns3 === C4 || CAns4 === C4) {
            document.getElementById("chckMCFour").checked = true;
        }
        //}
    }
    else {
        c.style.display = "block";
        b.style.display = "none";

        $("#txtMCOA1").val(C1);
        $("#txtMCOA2").val(C2);
        $("#txtMCOA3").val(C3);
        $("#txtMCOA4").val(C4);

        if (CAns === C1) {
            document.getElementById("chckMCOA1").checked = true;
        }
        if (CAns2 === C2) {
            document.getElementById("chckMCOA2").checked = true;
        }
        if (CAns3 === C3) {
            document.getElementById("chckMCOA3").checked = true;
        }
        if (CAns4 === C4) {
            document.getElementById("chckMCOA4").checked = true;
        }
    }
}

var DeleteSelectedQuestion = function (ID) {
    //alert("Delete: " + ID);
    var getCourSec = $("#secID").val();
    debugger
    $.ajax({
        type: "POST",
        url: "/Exam/deleteQuestion",
        data: { "ID": ID },
        success: function (response) {
            debugger
            alert(response.res);
            BindQuestionTable(getCourSec);
        },
        error: function (response) { }
    });
}

var UpdateSeletedQuestion = function () {
    debugger
    var radioAnswer1 = "";
    var radioAnswer2 = "";
    var radioAnswer3 = "";
    var radioAnswer4 = "";
    //var totalAnswer2 = "";
    var CorAns1 = "";
    var CorAns2 = "";
    var CorAns3 = "";
    var CorAns4 = "";

    var TestName = $("#testName").val();
    var testID = $("#sectionID").val();

    var getCourSec = $("#secID").val();
    var selectedQType = $("#ddquestionType option:selected").val();
    var question = $("#textQuestion").val();
    var questionID = $("#QuestionID").val();

    if (selectedQType === "2") {
        ans1 = $("#txtMCOne").val();
        ans2 = $("#txtMCTwo").val();
        ans3 = $("#txtMCThree").val();
        ans4 = $("#txtMCFour").val();

        if (document.getElementById("chckMCOne").checked) {
            if (CorAns1 === "" || CorAns1 === null || CorAns1 === " ") {
                CorAns1 = $("#txtMCOne").val(); //totalAnswer1 = $("#txtMCOne").val();
            }
            //else if (CorAns2 === "" || CorAns2 === null || CorAns2 === " ") {
            //    CorAns2 = $("#txtMCOne").val();
            //}
            //else if (CorAns3 === "" || CorAns3 === null || CorAns3 === " ") {
            //    CorAns3 = $("#txtMCOne").val();
            //}
            //else if (CorAns4 === "" || CorAns4 === null || CorAns4 === " ") {
            //    CorAns4 = $("#txtMCOne").val();
            //}
        }
        if (document.getElementById("chckMCTwo").checked) {
            if (CorAns2 === "" || CorAns2 === null || CorAns2 === " ") {
                CorAns2 = $("#txtMCTwo").val(); //totalAnswer1 = $("#txtMCOne").val();
            }
            //else if (CorAns2 === "" || CorAns2 === null || CorAns2 === " ") {
            //    CorAns2 = $("#txtMCTwo").val();
            //}
            //else if (CorAns3 === "" || CorAns3 === null || CorAns3 === " ") {
            //    CorAns3 = $("#txtMCTwo").val();
            //}
            //else if (CorAns4 === "" || CorAns4 === null || CorAns4 === " ") {
            //    CorAns4 = $("#txtMCTwo").val();
            //}
            ////if (totalAnswer1 === "" || totalAnswer1 === null || totalAnswer1 === " ") {
            //    //CorAns2 = $("#txtMCTwo").val();//totalAnswer1 = $("#txtMCTwo").val();
            ////}
            ////else
            ////{   totalAnswer1 = totalAnswer1 + "&" + $("#txtMCTwo").val();   }
        }
        if (document.getElementById("chckMCThree").checked) {
            if (CorAns3 === "" || CorAns3 === null || CorAns3 === " ") {
                CorAns3 = $("#txtMCThree").val(); //totalAnswer1 = $("#txtMCOne").val();
            }
            //else if (CorAns2 === "" || CorAns2 === null || CorAns2 === " ") {
            //    CorAns2 = $("#txtMCThree").val();
            //}
            //else if (CorAns3 === "" || CorAns3 === null || CorAns3 === " ") {
            //    CorAns3 = $("#txtMCThree").val();
            //}
            //else if (CorAns4 === "" || CorAns4 === null || CorAns4 === " ") {
            //    CorAns4 = $("#txtMCThree").val();
            //}
            ////if (totalAnswer1 === "" || totalAnswer1 === null || totalAnswer1 === " ") {
            //    //CorAns3 = $("#txtMCThree").val();//totalAnswer1 = $("#txtMCThree").val();
            ////}
            ////else
            ////{   totalAnswer1 = totalAnswer1 + "&" + $("#txtMCThree").val(); }
        }
        if (document.getElementById("chckMCFour").checked) {
            if (CorAns4 === "" || CorAns4 === null || CorAns4 === " ") {
                CorAns4 = $("#txtMCFour").val(); //totalAnswer1 = $("#txtMCOne").val();
            }
            //else if (CorAns2 === "" || CorAns2 === null || CorAns2 === " ") {
            //    CorAns2 = $("#txtMCFour").val();
            //}
            //else if (CorAns3 === "" || CorAns3 === null || CorAns3 === " ") {
            //    CorAns3 = $("#txtMCFour").val();
            //}
            //else if (CorAns4 === "" || CorAns4 === null || CorAns4 === " ") {
            //    CorAns4 = $("#txtMCFour").val();
            //}
            ////if (totalAnswer1 === "" || totalAnswer1 === null || totalAnswer1 === " ") {
            //    //CorAns4 = $("#txtMCFour").val();//totalAnswer1 = $("#txtMCFour").val();
            ////}
            ////else
            ////{   totalAnswer1 = totalAnswer1 + "&" + $("#txtMCFour").val();  }
        }

        $.ajax({
            type: "POST",
            url: "/Exam/updateQuestion",
            data: { "ID": questionID, "Question": question, "C1": ans1, "C2": ans2, "C3": ans3, "C4": ans4, "CorAns1": CorAns1, "CorAns2": CorAns2, "CorAns3": CorAns3, "CorAns4": CorAns4 },
            success: function (response) {
            },
            error: function (response) { }
        });
    }

    else if (selectedQType == "3") {
        ans1 = $("#txtMCOA1").val();
        ans2 = $("#txtMCOA2").val();
        ans3 = $("#txtMCOA3").val();
        ans4 = $("#txtMCOA4").val();

        if (document.getElementById("chckMCOA1").checked) { radioAnswer1 = $("#txtMCOA1").val(); }
        if (document.getElementById("chckMCOA2").checked) { radioAnswer2 = $("#txtMCOA2").val(); }
        if (document.getElementById("chckMCOA3").checked) { radioAnswer3 = $("#txtMCOA3").val(); }
        if (document.getElementById("chckMCOA4").checked) { radioAnswer4 = $("#txtMCOA4").val(); }

        $.ajax({
            type: "POST",
            url: "/Exam/updateQuestion",
            data: { "ID": questionID, "Question": question, "C1": ans1, "C2": ans2, "C3": ans3, "C4": ans4, "CorAns1": radioAnswer1, "CorAns2": radioAnswer2, "CorAns3": radioAnswer3, "CorAns4": radioAnswer4 },
            success: function (response) {
            },
            error: function (response) { }
        });
    }

    debugger
    $.ajax({
        type: "POST",
        url: "/Exam/updateTestName",
        data: { "ID": testID, "TestName": TestName },
        success: function (response) {
            debugger
            alert(response.res);
            BindQuestionTable(getCourSec);
        },
        error: function (response) { }
    });
}

var AddQuest = function () {
    $("#textQuestion").val("");
    document.getElementById("ddquestionType").selectedIndex = 0;

    $("#txtMCOne").val("");
    $("#txtMCTwo").val("");
    $("#txtMCThree").val("");
    $("#txtMCFour").val("");

    $("#txtMCOA1").val("");
    $("#txtMCOA2").val("");
    $("#txtMCOA3").val("");
    $("#txtMCOA4").val("");

    document.getElementById("chckMCOne").checked = false;
    document.getElementById("chckMCTwo").checked = false;
    document.getElementById("chckMCThree").checked = false;
    document.getElementById("chckMCFour").checked = false;
}

var changeQuestOrderNo = function (ID) {
    //alert(OrderNo);
    var getCourSec = $("#secID").val();
    var ordernoID = $("#txtOrderNo" + ID).val();
    $.ajax({
        type: "POST",
        url: "/Exam/updateQuestionOrderNumber",
        data: { "ID": ID, "OrderNo": ordernoID },
        success: function (response) {
            //alert(response.res);
            BindQuestionTable(getCourSec);
        },
        error: function (response) { }
    });
}

var StartTuitorialExam = function () {
    var getCourseID = $("#courseID1").val();
    var secID = $("#sectionID").val();
    //alert(getCourseID);

    var testURL = "../Exam/Exam?CourseID=" + getCourseID;// "&CourseSectionID=" + secID;
    window.location.href = testURL;//'@Url.Action("Exam", "Exam")';
}
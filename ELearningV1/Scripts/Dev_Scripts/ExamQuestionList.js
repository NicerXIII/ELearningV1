var addQuestion = function () { $("#addQuestionaire").modal(); }

var filltheGap = function () { $("#fillTheGap").modal(); }

var multilpleChoice = function () { $("#multilpleChoicee").modal(); }

$("#ddquestionType").change(function () {
    debugger
    var selected = $("#ddquestionType option:selected").val();
    var a = document.getElementById("fitb");
    var b = document.getElementById("mc");
    var c = document.getElementById("mcoa");

    if (selected === "1") {
        a.style.display = "block";
        b.style.display = "none";
        c.style.display = "none";
    }
    else if (selected === "2") {
        b.style.display = "block";
        a.style.display = "none";
        c.style.display = "none";
    }
    else if (selected === "3") {
        c.style.display = "block";
        b.style.display = "none";
        a.style.display = "none";
    }

});

$(function () {
    // Replace the <textarea id="editor1"> with a CKEditor
    // instance, using default configuration.
    //  CKEDITOR.replace('editor1')
    //bootstrap WYSIHTML5 - text editor
    $('.textarea').wysihtml5()
})

var saveQuestion = function () {
    var ques = $("#textQuestion").text();

    var selected = $("#ddquestionType option:selected").val();

    var Ans1 = "";
    var Ans2 = "";
    var Ans3 = "";
    var Ans4 = "";

    var CorrectAnswer1 = "";//
    var CorrectAnswer2 = "";//
    var CorrectAnswer3 = "";//
    var CorrectAnswer4 = "";//

    if (selected === "1") {
        Ans1 = $("#").val();
        Ans2 = $("#").val();
        Ans3 = $("#").val();
        Ans4 = $("#").val();
    }

    else if (selected === "2") {
        Ans1 = $("#txtMCOne").val();
        Ans2 = $("#txtMCTwo").val();
        Ans3 = $("#txtMCThree").val();
        Ans4 = $("#txtMCFour").val();

        CorrectAnswer1 = document.getElementById("chckMCOne");//
        CorrectAnswer2 = document.getElementById("chckMCTwo");// 
        CorrectAnswer3 = document.getElementById("chckMCThree");//
        CorrectAnswer4 = document.getElementById("chckMCFour");//
    }
    else if (selected === "3") {
        Ans1 = $("#txtMCOA1").val();
        Ans2 = $("#txtMCOA2").val();
        Ans3 = $("#txtMCOA3").val();
        Ans4 = $("#txtMCOA4").val();

        CorrectAnswer1 = document.getElementById("chckMCOA1");//"";
        CorrectAnswer2 = document.getElementById("chckMCOA2");// "";
        CorrectAnswer3 = document.getElementById("chckMCOA3");//"";
        CorrectAnswer4 = document.getElementById("chckMCOA4");//"";
    }

    $.ajax({
        type: "POST",
        url: "Exam/saveQuestion",
        data: { "Question": ques, "QuestionType": selected, "Answer1": Ans1, "Answer2": Ans2, "Answer3": Ans3, "Answer4": Ans4 },
        success: function (response) {
            var result = response.res;
            if (result == true) {
                $("#exampleModal").modal('show');
                $("#txtItemType").val("");
                $("#txtBrand").val("");
                $("#txtModel").val("");
                $("#txtSerialNo").val("");
                $("#ddStatusselect").val("");
                $("#txtQuantity").val("");
                $("#txtLocation").val("");
                $("#txtDescription").val("");
            }
        }
    });
}
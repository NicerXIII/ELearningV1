using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetQuestionsAndEmployeeAnswers
    {
        public string CourseID { get; set; }
        public string CourseSecID { get; set; }
        public string QuestionID { get; set; }
        public string AnswerID { get; set; }
        public string Question { get; set; }
        public string EmployeeAnswer { get; set; }
        public string IsCorrect { get; set; }
        public string OrderNumber { get; set; }
    }

    public class VMGetQuestionsAndEmployeeAnswersList : List<VMGetQuestionsAndEmployeeAnswers> { };
}
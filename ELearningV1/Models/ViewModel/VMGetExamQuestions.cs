using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetExamQuestions
    {
        public string ID { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string CAnswer { get; set; }
        public string CourseSectionID { get; set; }
        public string CourseID { get; set; }
        public int OrderNumber { get; set; }
    }
    public class getExamQuestionList : List<VMGetExamQuestions> { };
}
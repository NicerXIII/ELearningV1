using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMEmpAnswer
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int CourseSecID { get; set; }
        public int QuestionID { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeAnswer { get; set; }
        public string IsCorrect { get; set; }
        public DateTime DateAnswered { get; set; }
    }

    public class VMEmpAnswerList :  List<VMEmpAnswer> { };
}
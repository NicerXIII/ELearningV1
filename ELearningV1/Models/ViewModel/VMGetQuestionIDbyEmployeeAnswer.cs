using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetQuestionIDbyEmployeeAnswer
    {
        public string QuestionID { get; set; }
        public string CAnswer { get; set; }
    }
    public class QuestionIDList: List<VMGetQuestionIDbyEmployeeAnswer> { };
}
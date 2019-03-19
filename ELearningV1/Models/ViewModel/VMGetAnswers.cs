using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetAnswers
    {
        //public string QuestionID { get; set; }
        public string Answers { get; set; }
    }
    public class getAnswersList : List<VMGetAnswers> { };
}
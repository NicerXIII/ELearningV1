using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetEmployeeAnswers
    {
        public string EmployeeAnswer { get; set; }
    }
    public class EmployeeAnswersList : List<VMGetEmployeeAnswers> { };
}
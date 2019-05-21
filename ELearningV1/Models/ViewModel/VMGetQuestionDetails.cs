using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetQuestionDetails
    {
        public string Question { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string CAnswer1 { get; set; }
        public string CAnswer2 { get; set; }
        public string CAnswer3 { get; set; }
        public string CAnswer4 { get; set; }
    }
    public class VMGetQuestionDetailsList : List<VMGetQuestionDetails> { };
}
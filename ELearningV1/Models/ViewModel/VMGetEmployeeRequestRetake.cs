using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetEmployeeRequestRetake
    {
        public string ID { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string EnrolledDate { get; set; }
        public string CompletionDate { get; set; }
    }
    public class VMGetEmployeeRequestRetakeList : List<VMGetEmployeeRequestRetake> { };
}
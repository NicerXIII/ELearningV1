using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetEmployeeEnrollDate
    {
        public DateTime EnrolledDate { get; set; }
        public DateTime CompletionDate { get; set; }
    }
    public class getEmployeeDateEnrolledList : List<VMGetEmployeeEnrollDate> { };
}
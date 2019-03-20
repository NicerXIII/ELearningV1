using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetEmployeeEnrollDate
    {
        public DateTime DateEnrolled { get; set; }
    }
    public class getEmployeeDateEnrolledList : List<VMGetEmployeeEnrollDate> { };
}
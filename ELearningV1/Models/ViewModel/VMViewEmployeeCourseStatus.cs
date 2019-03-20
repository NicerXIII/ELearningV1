using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMViewEmployeeCourseStatus
    {
        public string EmployeeNumber { get; set; }
        public string EmpName { get; set; }
        public string Course { get; set; }
        public int Progress { get; set; }
        public float? Score { get; set; }
        public string Status1 { get; set; }
        public DateTime EnrolledDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

    public class VMViewEmployeeCourseStatusList : List<VMViewEmployeeCourseStatus> { }

}
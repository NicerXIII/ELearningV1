using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMViewCourseUser
    {
        public string EmployeeNumber { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public DateTime CompletionDate { get; set; }
    }
    public class VMViewCourseUserList : List<VMViewCourseUser> { }
}
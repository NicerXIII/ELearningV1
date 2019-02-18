using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMKioskLogInUser
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string ReportTo { get; set; }
    }

    public class VMKioskLogInUserList : List<VMKioskLogInUser> { }

}
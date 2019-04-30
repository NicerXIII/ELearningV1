using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetAppliedEmployees
    {
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
    }
    public class VMGetAppliedEmployeesList : List<VMGetAppliedEmployees> {};
}
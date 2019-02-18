using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMELearnEmpData
    {
        public int fuzeID { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmpName { get; set; }
        public string EmpImage { get; set; }
        public int PEarned { get; set; }
        public int PGranted { get; set; }
        public int PBalance { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class VMELearnEmpDataList : List<VMELearnEmpData> { }
}
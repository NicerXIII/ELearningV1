using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMEmpStatsCount
    {
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int InProgress { get; set; }
    }

    public class VMEmpStatsCountList : List<VMEmpStatsCount> { }
}
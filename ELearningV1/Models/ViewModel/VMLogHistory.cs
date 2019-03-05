using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMLogHistory
    {
        public int Status1 { get; set; }
        public DateTime Date1 { get; set; }

    }

    public class VMLogHistoryList : List<VMLogHistory> { }
}
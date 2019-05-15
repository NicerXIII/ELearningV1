using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMELearningCousesProgress
    {
        public int ID { get; set; }
        public string EmployeeNumber { get; set; }
        public int CourseID { get; set; }
        public string Course { get; set; }
        public double Progress { get; set; }
        public float? Score { get; set; }
        public DateTime EnrolledDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string ConsumedTime { get; set; }
        public string Status { get; set; }
        public string Status2 { get; set; }
    }

    public class VMELearningCousesProgressList : List<VMELearningCousesProgress> { }
}
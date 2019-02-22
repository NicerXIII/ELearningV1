using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMCourseSection
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string SrcFile { get; set; }
        public string CourseID { get; set; }
        public int OrderSec { get; set; }
    }

    public class VMCourseSectionList : List<VMCourseSection> { }
}
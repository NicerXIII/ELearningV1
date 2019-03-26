using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMElearningCourseSection
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string SourceFile { get; set; }
        public string CourseID { get; set; }
        public int OrderSec { get; set; }

        public IEnumerable<VMGetExamQuestion> Quiz { get; set; }
    }
    public class VMElearningCourseSectionList : List<VMElearningCourseSection> { };
}
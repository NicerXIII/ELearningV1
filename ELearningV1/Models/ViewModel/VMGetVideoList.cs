using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetVideoList
    {
        public string CourseID { get; set; }
        public string Course { get; set; }
        public string CourseSecID { get; set; }
        public string Title { get; set; }
        public string SrcFile { get; set; }
    }

    public class VMGetVideoListList: List<VMGetVideoList> { };
}
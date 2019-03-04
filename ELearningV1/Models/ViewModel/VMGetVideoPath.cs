using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetVideoPath
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string VideoPath { get; set; }
        public string CourseID { get; set; }
        public string OrderSec { get; set; }
    }
    public class getVideoPathList : List<VMGetVideoPath> { };
}
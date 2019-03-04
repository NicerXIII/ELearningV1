using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetPDFPath
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string PDFPath { get; set; }
        public string CourseID { get; set; }
        public string OrderSec { get; set; }
    }
    public class getPDFPathList : List<VMGetPDFPath> { };
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class CourseImage
    {
        public string ProductName { get; set; }
        public string Title { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}
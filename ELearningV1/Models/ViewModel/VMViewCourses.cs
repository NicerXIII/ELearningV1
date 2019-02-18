using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMViewCourses
    {
        public int ID { get; set; }
        public string Course { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }

    }

    public class VMViewCoursesList : List<VMViewCourses> { }
}
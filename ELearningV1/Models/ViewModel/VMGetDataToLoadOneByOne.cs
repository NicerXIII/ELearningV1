using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetDataToLoadOneByOne
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string SourceFile { get; set; }
        public string CourSecID { get; set; }
        public string OrderSec { get; set; }
    }
    public class getDataToLoadList : List<VMGetDataToLoadOneByOne> { };
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetPersonalityTestResult
    {
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public int Extroversion { get; set; }
        public int Agreeableness { get; set; }
        public int Conscientiousness { get; set; }
        public int Neuroticism { get; set; }
        public int Openness { get; set; }
    }
    public class VMGetPersonalityTestResultList : List<VMGetPersonalityTestResult> { };
}
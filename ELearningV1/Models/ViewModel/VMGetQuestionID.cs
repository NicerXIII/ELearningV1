﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearningV1.Models.ViewModel
{
    public class VMGetQuestionID
    {
        public string QuestionID { get; set; }
    }
    public class getQuestIDList : List<VMGetQuestionID> { };
}
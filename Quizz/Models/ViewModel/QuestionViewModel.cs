using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class QuestionViewModel
    {
        public int? QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public int? LevelId { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string Answer { get; set; }

    }
}
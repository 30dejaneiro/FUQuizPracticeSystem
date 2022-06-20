using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class ScoreTestViewModel
    {
        public int TestID { get; set; }
        public string TestCode { get; set; }
        public string AccountId { get; set; }
        public DateTime? DateTest { get; set; }
        public int TotalScore { get; set; }
    }
}
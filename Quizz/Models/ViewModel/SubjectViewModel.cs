using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public string AccountId { get; set; }

        public int TotalBank { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
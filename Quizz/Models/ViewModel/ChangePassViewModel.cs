using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class ChangePassViewModel
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string ReNewPass { get; set; }
    }
}
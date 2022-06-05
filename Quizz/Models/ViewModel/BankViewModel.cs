using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class BankViewModel
    {
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public int TotalQues { get; set; }
        public string AccountName { get; set; }
    }
}
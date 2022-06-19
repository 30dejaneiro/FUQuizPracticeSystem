using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class BankViewModel
    {
        public int? BankId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name can be more than 100.")]
        public string BankName { get; set; }
        public int? SubjectId { get; set; }
        public string Subjectname { get; set; }
        public int TotalQues { get; set; }
        public string AccountName { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
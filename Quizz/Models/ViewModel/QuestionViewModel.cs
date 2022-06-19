using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class QuestionViewModel
    {
        public int? QuestionId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Content can be more than 500.")]
        public string QuestionContent { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public int? LevelId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Answer A can be more than 100.")]
        public string A { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Answer B can be more than 100.")]
        public string B { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Answer C can be more than 100.")]
        public string C { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Answer D can be more than 100.")]
        public string D { get; set; }
        public string Answer { get; set; }

    }
}
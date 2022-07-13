using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class ExamQuestionModel
    {
        public int? ExamQuestionId { get; set; }

        public int? ExamId { get; set; }

        public string ExamCode { get; set; }

        [Required(ErrorMessage = "Content can't empty.")]
        [StringLength(500, ErrorMessage = "Content can be more than 500.")]
        public string QuestionContent { get; set; }

        [Required(ErrorMessage = "Answer A can't empty.")]
        [StringLength(100, ErrorMessage = "Answer A can be more than 100.")]
        public string A { get; set; }

        [Required(ErrorMessage = "Answer B can't empty.")]
        [StringLength(100, ErrorMessage = "Answer B can be more than 100.")]
        public string B { get; set; }

        [Required(ErrorMessage = "Answer C can't empty.")]
        [StringLength(100, ErrorMessage = "Answer C can be more than 100.")]
        public string C { get; set; }

        [Required(ErrorMessage = "Answer D can't empty.")]
        [StringLength(100, ErrorMessage = "Answer D can be more than 100.")]
        public string D { get; set; }
        public string Answer { get; set; }
    }
}
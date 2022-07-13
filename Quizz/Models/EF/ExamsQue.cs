namespace Quizz.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExamsQue
    {
        [Key]
        public int exam_ques_id { get; set; }

        public int? exam_id { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        public string A { get; set; }

        [Required]
        public string B { get; set; }

        [Required]
        public string C { get; set; }

        [Required]
        public string D { get; set; }

        [Required]
        [StringLength(5)]
        public string answer { get; set; }

        public virtual Exam Exam { get; set; }
    }
}

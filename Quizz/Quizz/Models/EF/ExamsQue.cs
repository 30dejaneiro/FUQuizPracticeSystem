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

        public int? question_id { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual Question Question { get; set; }
    }
}

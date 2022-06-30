namespace Quizz.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Score
    {
        [Key]
        public int score_id { get; set; }

        [Column("score")]
        public int score1 { get; set; }

        public int? exam_id { get; set; }

        [StringLength(30)]
        public string account_id { get; set; }

        public DateTime? date_test { get; set; }

        public virtual Account Account { get; set; }

        public virtual Exam Exam { get; set; }
    }
}

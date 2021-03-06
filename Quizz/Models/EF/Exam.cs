namespace Quizz.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            ExamsQues = new HashSet<ExamsQue>();
            Scores = new HashSet<Score>();
        }

        [Key]
        public int exam_id { get; set; }

        [Required(ErrorMessage = "Code can't empty")]
        [StringLength(200, ErrorMessage = "Code can't more than 200")]
        public string code { get; set; }

        [Required(ErrorMessage = "Test time can't empty.")]
        [RegularExpression(@"^([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$", ErrorMessage = "Test time not correct format hh:ss:mm")]
        public TimeSpan test_time { get; set; }

        [Required(ErrorMessage = "Number of question can't empty.")]
        public int num_of_ques { get; set; }

        public int total_tested { get; set; }

        public DateTime? date_created { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamsQue> ExamsQues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Score> Scores { get; set; }
    }
}

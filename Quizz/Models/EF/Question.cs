namespace Quizz.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [Key]
        public int question_id { get; set; }

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

        public bool? is_important { get; set; }

        public int? bank_id { get; set; }

        public virtual BankQuestion BankQuestion { get; set; }
    }
}

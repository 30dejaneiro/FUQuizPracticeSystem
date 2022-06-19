namespace Quizz.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Subject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Subject()
        {
            BankQuestions = new HashSet<BankQuestion>();
        }

        [Key]
        public int subject_id { get; set; }

        [Required()]
        [StringLength(50, ErrorMessage = "Subject name can't be more than 50.")]
        public string subject_name { get; set; }

        public DateTime? date_created { get; set; }

        [StringLength(30)]
        public string account_id { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankQuestion> BankQuestions { get; set; }
    }
}

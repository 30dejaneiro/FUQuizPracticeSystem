namespace Quizz.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            BankQuestions = new HashSet<BankQuestion>();
            Scores = new HashSet<Score>();
        }

        [Key]
        [StringLength(30)]
        public string account_id { get; set; }

        [StringLength(50)]
        public string full_name { get; set; }

        public DateTime? dob { get; set; }

        public bool? gender { get; set; }

        [Required]
        [StringLength(20)]
        public string username { get; set; }

        [Required]
        [StringLength(20)]
        public string password { get; set; }

        public bool role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BankQuestion> BankQuestions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Score> Scores { get; set; }
    }
}

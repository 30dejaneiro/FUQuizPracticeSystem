using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class AccountViewModel
    {

        [StringLength(30)]
        public string AccountId { get; set; }

        [Required(ErrorMessage = "Full name can't empty.")]
        [StringLength(50, ErrorMessage = "Full name can be more than 50.")]
        public string FullName { get; set; }

        public DateTime? Dob { get; set; }

        public bool? Gender { get; set; }

        [Required(ErrorMessage = "Username can't empty.")]
        [StringLength(20, ErrorMessage = "Username can be more than 20.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password can't empty.")]
        [StringLength(20, ErrorMessage = "Password can be more than 20.")]
        public string Password { get; set; }

        public bool Role { get; set; }
    }
}
using Quizz.Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class AuthViewModel
    {
        public Account Account { get; set; }

        [DisplayName("Remember")]
        public bool Remember { get; set; }

        [Required(ErrorMessage = "Re-password can't empty.")]
        public string Repassword { get; set; }

        [Required(ErrorMessage = "Old password can't empty.")]
        public string Oldpassword { get; set; }

        [Required(ErrorMessage = "New password can't empty.")]
        public string Newpassword { get; set; }
    }
}
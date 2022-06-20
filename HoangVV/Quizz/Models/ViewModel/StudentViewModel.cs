using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class StudentViewModel
    {
        public string RoleNum { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Roles { get; set; }
        public string Fullname { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public int? Totalscore { get; set; }
    }
}
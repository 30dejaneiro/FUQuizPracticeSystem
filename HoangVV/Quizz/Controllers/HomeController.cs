using Quizz.Controllers;
using Quizz.Models.DAO;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toeic_Quizz.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int? page, string search)
        {
            var iplSubject = new SubjectDAO();
            ViewBag.listSubject = iplSubject.SearchByPage(page, search);
            return View();
        }

        public ActionResult SubjectDetail(int? page, string search, int subject)
        {
            var iplSubject = new SubjectDAO();
            ViewBag.listQues = iplSubject.GetAllBankQuestion(page, search, subject);
            ViewBag.nameSubject = iplSubject.GetSubjectName(subject);
            return View();
        }
        public ActionResult BanksDetail(int bank)
        {
            var iplSubject = new SubjectDAO();
            ViewBag.listQues = iplSubject.GetQuestionsByBank(bank);
            //ViewBag.nameSubject = iplSubject.GetSubjectName(subject);
            return View();
        }

        public ActionResult UserProfile(string id)
        {
            var accId = Session["account"].ToString();
            if (accId != id)
            {
                return RedirectToAction("Index", "NotFound");
            }
            var iplAdmin = new AdminDAO();
            var student = iplAdmin.GetStudentById(id);
            return View(student);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(AuthViewModel av)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var id = Session["account"].ToString() ?? "";
                db.Configuration.ValidateOnSaveEnabled = true;
                var obj = db.Accounts.Where(s => s.account_id.Equals(id) && s.password.Equals(av.Oldpassword)).FirstOrDefault();
                if (obj != null)
                {
                    if (av.Repassword == av.Newpassword)
                    {
                        obj.password = av.Repassword;
                        db.SaveChanges();
                        SetAlert("Change password successfully!", "success");
                    }
                    else if (av.Oldpassword == av.Newpassword)
                    {
                        SetAlert("New password can't equal old password!", "warning");
                    }
                    else
                    {
                        SetAlert("Re-new password is not equal password!", "warning");
                    }
                }
                else
                {
                    SetAlert("Old password is not correct!", "warning");
                }
            }
            return RedirectToAction("ChangePassword", "Home", new { id = Session["account"] });
        }

        [HttpPost]
        public ActionResult ChangeProfile(Account st)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                string id = Session["account"].ToString() ?? "";
                var obj = (from a in db.Accounts where a.account_id.Equals(id) select a).FirstOrDefault();
                if (obj == null)
                {
                    SetAlert("Update Fail!!!", "error");
                }
                else
                {
                    obj.full_name = st.full_name;
                    obj.dob = st.dob;
                    obj.gender = st.gender;
                    db.SaveChanges();
                    Session["fullName"] = st.full_name;
                    SetAlert("Update Successfully!", "success");
                }
            }
            return RedirectToAction("UserProfile", "Home", new { id = Session["account"] });
        }

    }

}
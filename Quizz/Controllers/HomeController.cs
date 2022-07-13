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
            ViewBag.listSubject = iplSubject.PageSubjects(page, search);
            return View();
        }

        public ActionResult SubjectDetail(int? page, string search, int subject)
        {
            var iplBank = new BankDAO();
            var iplSubject = new SubjectDAO();
            ViewBag.listQues = iplBank.GetBanksBySubject(page, search, subject);
            ViewBag.nameSubject = iplSubject.GetSubjectName(subject, 0);
            return View();
        }
        public ActionResult BanksDetail(int bank)
        {
            var iplQuestion = new QuestionDAO();
            var iplSubject = new SubjectDAO();
            var iplBank = new BankDAO();
            ViewBag.nameSubject = iplSubject.GetSubjectName(0, bank);
            ViewBag.nameBank = iplBank.GetBankName(bank);
            ViewBag.listQues = iplQuestion.GetQuestionsByBank(bank);
            return View();
        }

        public ActionResult UserProfile(string id)
        {
            var iplUser = new UserDAO();
            var accId = Session["account"].ToString();
            if (accId != id)
            {
                return RedirectToAction("Index", "NotFound");
            }
            var student = iplUser.GetUserById(id);
            return View(student);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Scores(string id, int? page)
        {
            var iplScore = new ScoreDAO();
            ViewBag.listScore = iplScore.GetScoresByAccount(id, page);
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
        public ActionResult UserProfile(AccountViewModel st)
        {
            var iplUser = new UserDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("account_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool check = iplUser.UserAOU(st, st.AccountId);
                if (check == true)
                {
                    SetAlert("Update success!!!", "success");
                    if (st.AccountId != "1")
                    {
                        Session["fullName"] = st.FullName;
                    }
                }
                else
                {
                    SetAlert("Create fail!!!", "error");
                }
                return RedirectToAction("UserProfile", "Home", new { id = Session["account"] });
            }
            else
            {
                var student = iplUser.GetUserById(st.AccountId);
                return View(student);
            }
        }

    }

}
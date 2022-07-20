using Quizz.Common;
using Quizz.Models.DAO;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizz.Controllers
{
    public class ProfileController : BaseController
    {
        [CheckCredential(Role_ID = "2")]
        public ActionResult UserProfile(string id)
        {
            var iplUser = new UserDAO();
            var student = iplUser.GetUserById(id);
            return View(student);
        }

        [CheckCredential(Role_ID = "2")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [CheckCredential(Role_ID = "2")]
        public ActionResult Scores(string id, int? page)
        {
            var iplScore = new ScoreDAO();
            ViewBag.listScore = iplScore.GetScoresByAccount(id, page);
            return View();
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
        public ActionResult ChangePassword(AuthViewModel av)
        {
            UserLogin userLogin = (UserLogin)Session["account"];
            string id = userLogin.UserID;
            using (QuizzDbContext db = new QuizzDbContext())
            {
                db.Configuration.ValidateOnSaveEnabled = true;
                var obj = db.Accounts.Where(s => s.account_id.Equals(id) && s.password.Equals(av.Oldpassword)).FirstOrDefault();
                if (obj != null)
                {
                    if (av.Repassword == av.Newpassword && av.Oldpassword != av.Newpassword)
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
            return RedirectToAction("ChangePassword", "Profile", new { id });
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
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
                        var UserSession = new UserLogin();
                        UserSession.UserName = st.FullName;
                        Session.Add("account", UserSession);
                    }
                }
                else
                {
                    SetAlert("Create fail!!!", "error");
                }
                return RedirectToAction("UserProfile", "Profile", new { id = st.AccountId });
            }
            else
            {
                var student = iplUser.GetUserById(st.AccountId);
                return View(student);
            }
        }

        [CheckCredential(Role_ID = "2")]
        public ActionResult MyBank(string id, int? page, string search)
        {
            var iplBank = new BankDAO();
            ViewBag.listBank = iplBank.PageBankByAccount(id, page, search);
            ViewBag.accId = id;
            return View();
        }

        [CheckCredential(Role_ID = "2")]
        public ActionResult TestCodes(int? page)
        {
            var iplTest = new TestDAO();
            ViewBag.listTest = iplTest.PageTests(page);
            return View();
        }

        public ActionResult Questions(string id, int? page, string search)
        {
            var iplQuestion = new QuestionDAO();
            ViewBag.listQuestion = iplQuestion.PageQuestionById(id, page, search);
            ViewBag.accId = id;
            return View();
        }

        [HttpGet]
        [CheckCredential(Role_ID = "2")]
        public ActionResult QuestionDetail(int id, string aId)
        {
            var iplBank = new BankDAO();
            var iplQuestion = new QuestionDAO();

            var lecture = iplQuestion.GetQuestionById(id);
            ViewBag.listBank = iplBank.GetBanks();
            ViewBag.accId = aId;
            return View(lecture);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
        public ActionResult QuestionDetail(QuestionViewModel qs)
        {
            var iplBank = new BankDAO();
            var iplQuestion = new QuestionDAO();
            UserLogin userLogin = (UserLogin)Session["account"];
            string session = userLogin.UserID;
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("question_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool check = iplQuestion.QuestionAOU(qs);
                if (check == true)
                {
                    SetAlert(qs.QuestionId == null ? "Create success!!!" : "Update success!!!", "success");
                }
                else
                {
                    SetAlert(qs.QuestionId == null ? "Create fail!!!" : "Update fail!!!", "error");
                }
                if (qs.QuestionId == null)
                {
                    return RedirectToAction("Questions", "Profile", new { id = session });
                }
                else
                {
                    return RedirectToAction("QuestionDetail", "Profile", new { id = qs.QuestionId, aId = session });
                }
            }
            else
            {
                ViewBag.listBank = iplBank.GetBanks();
                ViewBag.accId = session;
                return View();
            }

        }

        [CheckCredential(Role_ID = "2")]
        public ActionResult BankDetail(int? id, string aId)
        {
            var iplBank = new BankDAO();
            var iplSubject = new SubjectDAO();
            var iplUser = new UserDAO();
            var bank = iplBank.GetBankById(id);
            ViewBag.accId = aId;
            ViewBag.listSubject = iplSubject.GetSubjects();
            ViewBag.listAccount = iplUser.GetAllAccount();
            return View(bank);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
        public ActionResult BankDetail(BankViewModel bq)
        {
            var iplBank = new BankDAO();
            var iplSubject = new SubjectDAO();
            UserLogin userLogin = (UserLogin)Session["account"];
            string session = userLogin.UserID;
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("Bank_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                iplBank.BankAOU(bq, bq.BankId, session);
                SetAlert(bq.BankId == null ? "Create success!!!" : "Update success!!!", "success");
                if (bq.BankId == null)
                {
                    return RedirectToAction("MyBank", "Profile", new { id = session });
                }
                else
                {
                    return RedirectToAction("BankDetail", "Profile", new { id = bq.BankId, aId = session });
                }
            }
            else
            {
                ViewBag.listSubject = iplSubject.GetSubjects();
                ViewBag.accId = session;
                return View();
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
        public JsonResult DeleteBank(int id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                try
                {
                    var check = from q in db.BankQuestions where q.bank_id == id select q;
                    db.BankQuestions.Remove(check.FirstOrDefault());
                    db.SaveChanges();
                    SetAlert("Delete successfully!", "success");
                    return Json(new { mess = true });
                }
                catch (Exception)
                {
                    SetAlert("Delete failture! Please delete all question belong this bank", "error");
                    return Json(new { mess = false });
                }
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
        public JsonResult DeleteQuestion(int id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var check = from q in db.Questions where q.question_id == id select q;
                if (check != null)
                {
                    db.Questions.Remove(check.FirstOrDefault());
                    db.SaveChanges();
                    SetAlert("Delete successfully!", "success");
                    return Json(new { mess = true });
                }
                else
                {
                    SetAlert("Delete fail!!!", "error");
                    return Json(new { mess = false });
                }
            }
        }
    }
}
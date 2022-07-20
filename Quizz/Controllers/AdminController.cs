using Quizz.Common;
using Quizz.Models;
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
    public class AdminController : BaseController
    {
        [CheckCredential(Role_ID = "1")]
        public ActionResult Index(int? page, string search)
        {
            var iplUser = new UserDAO();
            ViewBag.listStu = iplUser.PageUsers(page, search);
            return View();
        }

        [HttpGet]
        [CheckCredential(Role_ID = "1")]
        public ActionResult StudentDetail(string id)
        {
            var iplUser = new UserDAO();
            var user = iplUser.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public ActionResult StudentDetail(AccountViewModel st)
        {
            var iplUser = new UserDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("account_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool check = iplUser.UserAOU(st, st.AccountId);
                if (check == true)
                {
                    SetAlert(st.AccountId == null ? "Create success!!!" : "Update success!!!", "success");
                }
                else
                {
                    SetAlert(st.AccountId == null ? "Create fail!!!" : "Update fail!!!", "error");
                }
                return RedirectToAction(st.AccountId == null ? "Index" : "StudentDetail", "Admin", new { id = st.AccountId ?? "" });
            }
            else
            {
                var student = iplUser.GetUserById(st.AccountId);
                return View(student);
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public JsonResult DeleteStudent(string id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var check = from q in db.Accounts where q.account_id == id select q;
                if (check != null)
                {
                    try
                    {
                        db.Accounts.Remove(check.FirstOrDefault());
                        db.SaveChanges();
                        SetAlert("Delete successfully!", "success");
                        return Json(new { mess = true });
                    }
                    catch (Exception)
                    {
                        SetAlert("Delete failt!!!PLease delete all detail of this account", "warning");
                        return Json(new { mess = false });
                    }
                }
                else
                {
                    SetAlert("Delete failt!!!", "error");
                    return Json(new { mess = false });
                }
            }
        }
        [CheckCredential(Role_ID = "1")]
        public ActionResult SubjectList(int? page, string search)
        {
            var iplSubject = new SubjectDAO();
            ViewBag.listSubjects = iplSubject.PageSubjects(page, search);
            return View();
        }
        [CheckCredential(Role_ID = "1")]
        public ActionResult SubjectDetail(int? id)
        {
            var iplSubject = new SubjectDAO();
            var subject = iplSubject.GetSubjectById(id);
            return View(subject);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public ActionResult SubjectDetail(Subject s)
        {
            var iplSubject = new SubjectDAO();
            UserLogin userLogin = (UserLogin)Session["account"];
            string session = userLogin.UserID;

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("subject_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                iplSubject.SubjectAOU(s, s.subject_id, session);
                SetAlert(s.subject_id == 0 ? "Create success!!!" : "Update success!!!", "success");
                return RedirectToAction(s.subject_id == 0 ? "SubjectList" : "SubjectDetail", "Admin", new { id = s.subject_id == 0 ? -1 : s.subject_id });
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public JsonResult DeleteSubject(int id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                try
                {
                    var check = from q in db.Subjects where q.subject_id == id select q;
                    db.Subjects.Remove(check.FirstOrDefault());
                    db.SaveChanges();
                    SetAlert("Delete successfully!", "success");
                    return Json(new { mess = true });
                }
                catch (Exception)
                {
                    SetAlert("Delete failture! Please delete all bank and question belong this subject", "error");
                    return Json(new { mess = false });
                }
            }
        }
        [CheckCredential(Role_ID = "1")]
        public ActionResult BankList(int? page, string search)
        {
            var iplBank = new BankDAO();
            ViewBag.listBank = iplBank.PageBanks(page, search);
            return View();
        }


        [CheckCredential(Role_ID = "1")]
        public ActionResult BankDetail(int? id)
        {
            var iplBank = new BankDAO();
            var iplSubject = new SubjectDAO();
            var bank = iplBank.GetBankById(id);
            ViewBag.listSubject = iplSubject.GetSubjects();
            return View(bank);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
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
                return RedirectToAction(bq.BankId == null ? "BankList" : "BankDetail", "Admin", new { id = bq.BankId == 0 ? -1 : bq.BankId });
            }
            else
            {
                var bank = iplBank.GetBankById(bq.BankId);
                ViewBag.listSubject = iplSubject.GetSubjects();
                return View();
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
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

        [CheckCredential(Role_ID = "1")]
        public ActionResult QuestionList(int? page, string search)
        {
            var iplQuestion = new QuestionDAO();
            ViewBag.listQuestion = iplQuestion.PageQuestion(page, search);
            return View();
        }

        [HttpGet]
        [CheckCredential(Role_ID = "1")]
        public ActionResult QuestionDetail(int id)
        {
            var iplQuestion = new QuestionDAO();
            var iplBank = new BankDAO();
            var lecture = iplQuestion.GetQuestionById(id);
            ViewBag.listBank = iplBank.GetBanks();
            return View(lecture);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public ActionResult QuestionDetail(QuestionViewModel qs)
        {
            var iplQuestion = new QuestionDAO();
            var iplBank = new BankDAO();
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
                return RedirectToAction(qs.QuestionId == null ? "QuestionList" : "QuestionDetail", "Admin", new { id = qs.QuestionId == null ? -1 : qs.QuestionId });
            }
            else
            {
                ViewBag.listBank = iplBank.GetBanks();
                return View();
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
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
        [CheckCredential(Role_ID = "1")]
        public ActionResult TestList(int? page)
        {
            var iplTest = new TestDAO();
            ViewBag.listTest = iplTest.PageTests(page);
            return View();
        }

        [HttpGet]
        [CheckCredential(Role_ID = "1")]
        public ActionResult TestDetail(int id)
        {
            var iplTest = new TestDAO();
            var test = iplTest.GetTestById(id);
            return View(test);
        }


        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult TestDetail(Exam vm)
        {
            var iplTest = new TestDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("exam_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                int status = iplTest.TestAOU(vm, vm.exam_id);
                if (status == 1)
                {
                    SetAlert(vm.exam_id == 0 ? "Create success!!!" : "Update success!!!", "success");
                    return RedirectToAction(vm.exam_id == 0 ? "TestList" : "TestDetail", "Admin", new { id = vm.exam_id == 0 ? -1 : vm.exam_id });
                }
                else
                {
                    SetAlert("Code already exist!", "warning");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public JsonResult DeleteTest(int id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var check = from q in db.Exams where q.exam_id == id select q;
                bool message = false;
                if (check != null)
                {
                    try
                    {
                        db.Exams.Remove(check.FirstOrDefault());
                        db.SaveChanges();
                        SetAlert("Delete successfully!", "success");
                        message = true;
                    }
                    catch (Exception)
                    {
                        SetAlert("Delete fail!", "warning");
                        message = false;
                    }
                }
                return Json(new { mess = message });
            }
        }
        [CheckCredential(Role_ID = "1")]
        public ActionResult ExamQuestions(int? page, string search)
        {
            var iplQuestion = new ExamQuestionDAO();
            ViewBag.listQuestion = iplQuestion.PageQuestion(page, search);
            return View();
        }

        [HttpGet]
        [CheckCredential(Role_ID = "1")]
        public ActionResult ExamQuestionDetail(int id)
        {
            var iplQuestion = new ExamQuestionDAO();
            var iplTest = new TestDAO();
            var ques = iplQuestion.GetQuestionById(id);
            ViewBag.listBank = iplTest.GetTests();
            return View(ques);
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public ActionResult ExamQuestionDetail(ExamQuestionModel qs)
        {
            var iplQuestion = new ExamQuestionDAO();
            var iplTest = new TestDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("exam_ques_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool check = iplQuestion.QuestionAOU(qs);
                if (check == true)
                {
                    SetAlert(qs.ExamQuestionId == null ? "Create success!!!" : "Update success!!!", "success");
                }
                else
                {
                    SetAlert(qs.ExamQuestionId == null ? "Create fail!!!" : "Update fail!!!", "error");
                }
                return RedirectToAction(qs.ExamQuestionId == null ? "ExamQuestions" : "ExamQuestionDetail", "Admin", new { id = qs.ExamQuestionId == null ? -1 : qs.ExamQuestionId });
            }
            else
            {
                ViewBag.listBank = iplTest.GetTests();
                return View();
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public JsonResult DeleteExamQuestion(int id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var check = from q in db.ExamsQues where q.exam_ques_id == id select q;
                if (check != null)
                {
                    db.ExamsQues.Remove(check.FirstOrDefault());
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
        [CheckCredential(Role_ID = "1")]
        public ActionResult ScoreList(int? page)
        {
            var iplScore = new ScoreDAO();
            ViewBag.listScore = iplScore.PageScores(page);
            return View();
        }

        [HttpPost]
        [CheckCredential(Role_ID = "1")]
        public JsonResult ResetPassword(string roleNum)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var check = (from q in db.Accounts where q.account_id.Equals(roleNum) select q).FirstOrDefault();
                if (check != null)
                {
                    check.password = "123";
                    db.SaveChanges();
                    SetAlert("Reset successfully! New password id: 123", "success");
                    return Json(new { mess = true });
                }
                else
                {
                    SetAlert("Reset fail!!!", "error");
                    return Json(new { mess = false });
                }
            }
        }
    }
}
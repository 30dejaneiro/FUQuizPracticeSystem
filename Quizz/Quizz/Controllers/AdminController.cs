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
        // GET: Admin
        public ActionResult Index(int? page, string search)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listStu = iplAdmin.PageStudent(page, search);
            return View();
        }

        public ActionResult SubjectList(int? page, string search)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listSubjects = iplAdmin.PageSubject(page, search);
            return View();
        }

        public ActionResult BankList(int? page, string search)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listBank = iplAdmin.PageBank(page, search);
            return View();
        }

        [HttpGet]
        public ActionResult StudentDetail(string id)
        {
            var iplAdmin = new AdminDAO();
            var student = iplAdmin.GetStudentById(id);
            return View(student);
        }
        public ActionResult SubjectDetail(int? id)
        {
            var iplAdmin = new AdminDAO();
            var subject = iplAdmin.GetSubjectById(id);
            return View(subject);
        }

        public ActionResult BankDetail(int? id)
        {
            var iplAdmin = new AdminDAO();
            var iplSubject = new SubjectDAO();
            var bank = iplAdmin.GetBankById(id);
            ViewBag.listSubject = iplSubject.GetAllSubjects();
            return View(bank);
        }
        [HttpPost]
        public ActionResult BankDetail(BankViewModel bq)
        {
            var iplAdmin = new AdminDAO();
            var session = Session["account"].ToString();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("Bank_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                iplAdmin.BankAOU(bq, bq.BankId, session);
                SetAlert(bq.BankId == null ? "Create success!!!" : "Update success!!!", "success");
                return RedirectToAction(bq.BankId == null ? "BankList" : "BankDetail", "Admin", new { id = bq.BankId == 0 ? -1 : bq.BankId });
            }
            else
            {
                var iplSubject = new SubjectDAO();
                var bank = iplAdmin.GetBankById(bq.BankId);
                ViewBag.listSubject = iplSubject.GetAllSubjects();
                return View();
            }
        }

        [HttpPost]
        public ActionResult SubjectDetail(Subject s)
        {
            var iplAdmin = new AdminDAO();
            var session = Session["account"].ToString();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("subject_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                iplAdmin.SubjectAOU(s, s.subject_id, session);
                SetAlert(s.subject_id == 0 ? "Create success!!!" : "Update success!!!", "success");
                return RedirectToAction(s.subject_id == 0 ? "SubjectList" : "SubjectDetail", "Admin", new { id = s.subject_id == 0 ? -1 : s.subject_id });
            }
            else
            {
                return View();
            }
        }

        public ActionResult QuestionList(int? page, string search)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listQuestion = iplAdmin.PageQuestion(page, search);
            return View();
        }

        [HttpGet]
        public ActionResult QuestionDetail(int id)
        {
            var iplAdmin = new AdminDAO();
            var iplBank = new ProfileDAO();
            var lecture = iplAdmin.GetQuestionById(id);
            ViewBag.listBank = iplBank.GetAllBanks();
            return View(lecture);
        }

        [HttpPost]
        public ActionResult QuestionDetail(QuestionViewModel qs)
        {
            var iplAdmin = new AdminDAO();
            var iplBank = new ProfileDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("question_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool check = iplAdmin.QuestionAOU(qs);
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
                ViewBag.listBank = iplBank.GetAllBanks();
                return View();
            }
        }

        [HttpPost]
        public ActionResult StudentDetail(Account st)
        {
            AdminDAO admin = new AdminDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("account_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool check = admin.SaveOrUpdate(st, st.account_id);
                if (check == true)
                {
                    SetAlert(st.account_id == null ? "Create success!!!" : "Update success!!!", "success");
                    Session["fullName"] = st.full_name;
                }
                else
                {
                    SetAlert(st.account_id == null ? "Create fail!!!" : "Update fail!!!", "error");
                }
                return RedirectToAction(st.account_id == null ? "Index" : "StudentDetail", "Admin", new { id = st.account_id ?? "" });
            }
            else
            {
                var student = admin.GetStudentById(st.account_id);
                return View(student);
            }
        }

        [HttpPost]
        public JsonResult ResetPassword(string roleNum)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                AdminDAO admin = new AdminDAO();
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


        [HttpPost]
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


        [HttpPost]
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

        [HttpPost]
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

        [HttpPost]
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
    }
}
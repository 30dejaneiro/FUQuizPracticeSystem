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
        // GET: Bank
        public ActionResult MyBank(string id, int? page, string search)
        {
            var iplBank = new BankDAO();
            ViewBag.listBank = iplBank.PageBankByAccount(id, page, search);
            ViewBag.accId = id;
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
        public ActionResult QuestionDetail(QuestionViewModel qs)
        {
            var iplBank = new BankDAO();
            var iplQuestion = new QuestionDAO();
            var session = Session["account"].ToString();
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
        public ActionResult BankDetail(BankViewModel bq)
        {
            var iplBank = new BankDAO();
            var iplSubject = new SubjectDAO();

            var session = Session["account"].ToString();
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
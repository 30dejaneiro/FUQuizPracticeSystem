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
            var iplBank = new BankDAO();
            var lecture = iplAdmin.GetQuestionById(id);
            ViewBag.listBank = iplBank.GetAllBanks();
            return View(lecture);
        }

        [HttpPost]
        public ActionResult QuestionAOU(QuestionViewModel qs)
        {
            var iplAdmin = new AdminDAO();
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
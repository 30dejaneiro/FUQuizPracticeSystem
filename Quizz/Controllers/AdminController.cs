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
        public ActionResult SubjectList(int? page, string search)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listSubjects = iplAdmin.PageSubject(page, search);
            return View();
        }

        public ActionResult SubjectDetail(int? id)
        {
            var iplAdmin = new AdminDAO();
            var subject = iplAdmin.GetSubjectById(id);
            return View(subject);
        }

        [HttpPost]
        public ActionResult SubjectDetail(Subject s)
        {
            var iplAdmin = new AdminDAO();
            iplAdmin.SubjectAOU(s, s.subject_id);
            SetAlert(s.subject_id == 0 ? "Create success!!!" : "Update success!!!", "success");
            return RedirectToAction(s.subject_id == 0 ? "SubjectList" : "SubjectDetail", "Admin", new { id = s.subject_id == 0 ? -1 : s.subject_id });
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
    }
}
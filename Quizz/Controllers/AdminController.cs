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

        [HttpGet]
        public ActionResult StudentDetail(string id)
        {
            var iplAdmin = new AdminDAO();
            var student = iplAdmin.GetStudentById(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult StudentSave(Account st)
        {
            AdminDAO admin = new AdminDAO();
            bool check = admin.SaveOrUpdate(st, st.account_id);
            if (check == true)
            {
                SetAlert(st.account_id == null ? "Create success!!!" : "Update success!!!", "success");
            }
            else
            {
                SetAlert(st.account_id == null ? "Create fail!!!" : "Update fail!!!", "error");
            }
            return RedirectToAction(st.account_id == null ? "Index" : "StudentDetail", "Admin", new { id = st.account_id ?? "" });
        }

        [HttpPost]
        public JsonResult DeleteStudent(string id)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var check = from q in db.Accounts where q.account_id == id select q;
                var check1 = from s in db.Scores where s.account_id == id select s;
                if (check != null && check1 != null)
                {
                    db.Accounts.Remove(check.FirstOrDefault());
                    db.Scores.Remove(check1.FirstOrDefault());
                    db.SaveChanges();
                    SetAlert("Delete successfully!", "success");
                    return Json(new { mess = true });
                }
                else
                {
                    SetAlert("Delete failt!!!", "error");
                    return Json(new { mess = false });
                }
            }
        }
    }
}
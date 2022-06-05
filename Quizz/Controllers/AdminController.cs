﻿using Quizz.Models;
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

        public ActionResult ScoreList(int? page)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listScore = iplAdmin.PageScore(page);
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

        [HttpPost]
        public ActionResult SubjectDetail(Subject s)
        {
            var iplAdmin = new AdminDAO();
            iplAdmin.SubjectAOU(s, s.subject_id);
            SetAlert(s.subject_id == 0 ? "Create success!!!" : "Update success!!!", "success");
            return RedirectToAction(s.subject_id == 0 ? "SubjectList" : "SubjectDetail", "Admin", new { id = s.subject_id == 0 ? -1 : s.subject_id });
        }


        public ActionResult TestList(int? page)
        {
            var iplAdmin = new AdminDAO();
            ViewBag.listTest = iplAdmin.PageTest(page);
            return View();
        }

        [HttpGet]
        public ActionResult TestDetail(int id)
        {
            var iplAdmin = new AdminDAO();
            var test = iplAdmin.GetTestById(id);
            return View(test);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestDetail(Exam vm)
        {
            var iplAdmin = new AdminDAO();
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0 && !x.Key.Equals("test_id")).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (errors.Length == 0)
            {
                bool checkCode = iplAdmin.FindByTestCode(vm.code);
                if (checkCode == true)
                {
                    iplAdmin.TestAOU(vm, vm.exam_id);
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
            var iplSubject = new SubjectDAO();
            var lecture = iplAdmin.GetQuestionById(id);
            ViewBag.listSubject = iplSubject.GetAllSubjects();
            return View(lecture);
        }

        [HttpPost]
        public ActionResult QuestionAOU(QuestionSoundViewModel qs)
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

        [HttpPost]
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
                        SetAlert("Delete fail!!!", "error");
                        message = false;
                    }
                }
                return Json(new { mess = message });
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
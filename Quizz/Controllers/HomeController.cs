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
    }
}
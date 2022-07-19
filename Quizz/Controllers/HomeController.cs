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
    public class HomeController : Controller
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


    }

}
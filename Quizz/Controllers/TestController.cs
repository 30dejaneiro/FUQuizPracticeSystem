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
    public class TestController : BaseController
    {
        [CheckCredential(Role_ID = "2")]
        public ActionResult Index()
        {
            return View();
        }

        [CheckCredential(Role_ID = "2")]
        public ActionResult TestDetail(string code)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                TestDAO implPart = new TestDAO();
                DateTime local = DateTime.Now;
                var data = (from t in db.Exams where t.code.Equals(code) select t).FirstOrDefault();
                if (data != null)
                {
                    ViewBag.listQuestion = implPart.GetTestByCode(code);
                    ViewBag.testTime = data.test_time;
                    data.total_tested += 1;
                    db.SaveChanges();
                    return View();
                }
                else
                {
                    SetAlert("Wrong code! Please try again!", "warning");
                    return View("Index");
                }
            }
        }

        [HttpPost]
        [CheckCredential(Role_ID = "2")]
        public JsonResult CheckQuestion(string code, List<QuestionAnswer> data, int totalQuestion)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                UserLogin userLogin = (UserLogin)Session["account"];
                String id = userLogin.UserID;
                var check = db.Accounts.FirstOrDefault(s => s.account_id == id);
                var findCode = db.Exams.FirstOrDefault(e => e.code == code);
                if (check != null)
                {
                    int score = 0;
                    foreach (var item in data)
                    {
                        var ques = (from q in db.Questions where q.question_id == item.QuestionId select q).FirstOrDefault();
                        if (item.UserAnswer == ques.answer)
                        {
                            score += (100 / totalQuestion);
                        }
                    }
                    Score s = new Score()
                    {
                        score1 = score,
                        exam_id = findCode.exam_id,
                        account_id = id,
                        date_test = DateTime.Now
                    };
                    db.Scores.Add(s);
                    db.SaveChanges();
                    return Json(new { mess = true });
                }
                else
                {
                    return Json(new { mess = false });
                }
            }

        }
    }
}
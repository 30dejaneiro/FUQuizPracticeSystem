using PagedList;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.DAO
{
    public class AdminDAO
    {
        private QuizzDbContext db = null;
        public AdminDAO()
        {
            db = new QuizzDbContext();
        }

        public IPagedList<QuestionViewModel> PageQuestion(int? page, string search)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from l in db.Questions
                      join b in db.BankQuestions on l.bank_id equals b.bank_id
                      orderby l.question_id descending
                      select new QuestionViewModel()
                      {
                          QuestionId = l.question_id,
                          QuestionContent = l.content,
                          A = l.A,
                          B = l.B,
                          C = l.C,
                          D = l.D,
                          Answer = l.answer,
                          BankName = b.bank_name
                      }; ;
            if (!String.IsNullOrEmpty(search))
            {
                obj = (obj.Where(p => p.QuestionContent.Contains(search)));
            }
            return obj.ToPagedList(pageIndex, pageSize);
        }


        public QuestionViewModel GetQuestionById(int id)
        {
            var obj = from l in db.Questions
                      where l.question_id == id
                      select new QuestionViewModel()
                      {
                          QuestionId = l.question_id,
                          QuestionContent = l.content,
                          A = l.A,
                          B = l.B,
                          C = l.C,
                          D = l.D,
                          Answer = l.answer,
                          BankId = l.bank_id
                      };
            return obj.FirstOrDefault();
        }

        public bool QuestionAOU(QuestionViewModel lp)
        {
            if (lp.QuestionId == null)
            {
                Question q = new Question()
                {
                    content = lp.QuestionContent,
                    A = lp.A,
                    B = lp.B,
                    C = lp.C,
                    D = lp.D,
                    answer = lp.Answer,
                    bank_id = lp.BankId
                };
                db.Questions.Add(q);
            }
            else
            {
                var obj = (from a in db.Questions where a.question_id == lp.QuestionId select a).FirstOrDefault();
                obj.content = lp.QuestionContent;
                obj.A = lp.A;
                obj.B = lp.B;
                obj.C = lp.C;
                obj.D = lp.D;
                obj.answer = lp.Answer;
                obj.bank_id = lp.BankId;
            }
            db.SaveChanges();
            return true;
        }
    }
}
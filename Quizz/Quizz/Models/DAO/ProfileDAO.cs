using PagedList;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.DAO
{
    public class ProfileDAO
    {
        readonly QuizzDbContext db;
        public ProfileDAO()
        {
            db = new QuizzDbContext();
        }
        public List<BankQuestion> GetAllBanks()
        {
            var obj = from b in db.BankQuestions select b;
            return obj.ToList();
        }

        public IPagedList<BankViewModel> PageBankById(string id, int? page, string search)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from q in db.BankQuestions
                      join s in db.Questions on q.bank_id equals s.bank_id into bq
                      from s in bq.DefaultIfEmpty()
                      join su in db.Subjects on q.subject_id equals su.subject_id
                      where q.account_id == id
                      group new { q, s, su } by new { s.bank_id, q.bank_name, su.subject_name } into g
                      select new BankViewModel()
                      {
                          BankId = g.FirstOrDefault().q.bank_id,
                          BankName = g.Key.bank_name,
                          AccountName = g.FirstOrDefault().q.account_id,
                          Subjectname = g.Key.subject_name,
                          TotalQues = g.Count(m => m.s.bank_id > 0),
                          DateCreated = g.FirstOrDefault().su.date_created
                      };
            if (!String.IsNullOrEmpty(search))
            {
                obj = (obj.Where(p => p.BankName.Contains(search)));
            }
            return obj.OrderBy(m => m.DateCreated).ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<QuestionViewModel> PageQuestionById(string id, int? page, string search)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from l in db.Questions
                      join b in db.BankQuestions on l.bank_id equals b.bank_id
                      where b.account_id == id
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
    }
}
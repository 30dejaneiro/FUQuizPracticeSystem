using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quizz.Models.ViewModel;
using Quizz.Models.EF;

namespace Quizz.Models.DAO
{
    public class SubjectDAO
    {
        private readonly QuizzDbContext db = null;
        public SubjectDAO()
        {
            db = new QuizzDbContext();
        }

        public List<Subject> GetAllSubjects()
        {
            var obj = from s in db.Subjects orderby s.subject_id select s;
            return obj.ToList();
        }
        public List<Account> GetAllAccount()
        {
            var obj = from s in db.Accounts select s;
            return obj.ToList();
        }

        public IPagedList<ScoreTestViewModel> GetScoresByAccount(string id, int? page)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var obj = from sc in db.Scores
                      join t in db.Exams on sc.exam_id equals t.exam_id
                      where sc.account_id == id
                      orderby sc.account_id
                      select new ScoreTestViewModel()
                      {
                          TestCode = t.code,
                          AccountId = sc.account_id,
                          TotalScore = sc.score1,
                          DateTest = sc.date_test
                      };
            return obj.ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<SubjectViewModel> SearchByPage(int? page, string search)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from s in db.Subjects
                      join q in db.BankQuestions on s.subject_id equals q.subject_id into sq
                      from q in sq.DefaultIfEmpty()
                      group new { q, s } by new { s.subject_id, s.subject_name } into g
                      select new SubjectViewModel()
                      {
                          SubjectId = g.FirstOrDefault().s.subject_id,
                          SubjectName = g.FirstOrDefault().s.subject_name,
                          TotalQues = g.Count(m => m.q.subject_id > 0),
                      };
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.SubjectName.Contains(search));
            }
            return obj.OrderBy(m => m.SubjectId).ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<BankViewModel> GetAllBankQuestion(int? page, string search, int subject)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from q in db.BankQuestions
                      join s in db.Questions on q.bank_id equals s.bank_id into bq
                      from s in bq.DefaultIfEmpty()
                      where q.subject_id == subject
                      group new { q, s } by new { s.bank_id, q.bank_name } into g
                      select new BankViewModel()
                      {
                          BankId = g.FirstOrDefault().q.bank_id,
                          BankName = g.Key.bank_name,
                          AccountName = g.FirstOrDefault().q.account_id,
                          TotalQues = g.Count(m => m.s.bank_id > 0),
                      };
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.BankName.Contains(search));
            }
            return obj.OrderBy(m => m.BankId).ToPagedList(pageIndex, pageSize);
        }


        public List<Question> GetQuestionsByBank(int bank)
        {
            var obj = from q in db.Questions where q.bank_id == bank select q;
            return obj.ToList();
        }

        public string GetSubjectName(int subject)
        {
            var obj = from s in db.Subjects where s.subject_id == subject select s;
            return obj.FirstOrDefault().subject_name;
        }

    }
}
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quizz.Models.ViewModel;
using Quizz.Models.EF;

namespace Quizz.Models.DAO
{
    public class BankDAO
    {
        private readonly QuizzDbContext db = null;
        public BankDAO()
        {
            db = new QuizzDbContext();
        }
        public List<BankQuestion> GetBanks()
        {
            var obj = from b in db.BankQuestions select b;
            return obj.ToList();
        }

        public IPagedList<BankViewModel> PageBanks(int? page, string search)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from q in db.BankQuestions
                      join s in db.Questions on q.bank_id equals s.bank_id into bq
                      from s in bq.DefaultIfEmpty()
                      join su in db.Subjects on q.subject_id equals su.subject_id
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

        public IPagedList<BankViewModel> GetBanksBySubject(int? page, string search, int subject)
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

        public string GetBankName(int bank)
        {
            var obj = from b in db.BankQuestions
                      where b.bank_id == bank
                      select b;
            return obj.FirstOrDefault().bank_name;
        }

        public IPagedList<BankViewModel> PageBankByAccount(string id, int? page, string search)
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

        public BankViewModel GetBankById(int? id)
        {
            var obj = from a in db.Accounts
                      join q in db.BankQuestions on a.account_id equals q.account_id
                      join s in db.Subjects on q.subject_id equals s.subject_id
                      where q.bank_id == id
                      select new BankViewModel()
                      {
                          BankId = q.bank_id,
                          BankName = q.bank_name,
                          AccountName = a.account_id,
                          SubjectId = s.subject_id,
                      };
            return obj.FirstOrDefault();
        }
        public void BankAOU(BankViewModel s, int? id, string accId)
        {
            DateTime localDate = DateTime.Now;
            if (id == null)
            {
                BankQuestion bq = new BankQuestion()
                {
                    bank_name = s.BankName,
                    account_id = accId,
                    subject_id = s.SubjectId
                };
                db.BankQuestions.Add(bq);
            }
            else
            {
                var obj = (from a in db.BankQuestions where a.bank_id == id select a).FirstOrDefault();
                obj.bank_name = s.BankName;
                obj.subject_id = s.SubjectId;
            }
            db.SaveChanges();
        }

    }
}
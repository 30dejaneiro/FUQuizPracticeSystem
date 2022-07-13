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

        public List<Subject> GetSubjects()
        {
            var obj = from s in db.Subjects orderby s.subject_id select s;
            return obj.ToList();
        }

        public IPagedList<SubjectViewModel> PageSubjects(int? page, string search)
        {
            int pageSize = 10;
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
                          TotalBank = g.Count(m => m.q.subject_id > 0),
                          AccountId = g.FirstOrDefault().s.account_id,
                          DateCreated = g.FirstOrDefault().s.date_created,
                      };
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.SubjectName.Contains(search));
            }
            return obj.OrderBy(m => m.DateCreated).ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<SubjectViewModel> PageSubjectById(string id, int? page, string search)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from s in db.Subjects
                      join q in db.BankQuestions on s.subject_id equals q.subject_id into sq
                      from q in sq.DefaultIfEmpty()
                      where s.account_id == id
                      group new { q, s } by new { s.subject_id, s.subject_name } into g
                      select new SubjectViewModel()
                      {
                          SubjectId = g.FirstOrDefault().s.subject_id,
                          SubjectName = g.FirstOrDefault().s.subject_name,
                          TotalBank = g.Count(m => m.q.subject_id > 0),
                          AccountId = g.FirstOrDefault().s.account_id,
                          DateCreated = g.FirstOrDefault().s.date_created,
                      };
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.SubjectName.Contains(search));
            }
            return obj.OrderBy(m => m.DateCreated).ToPagedList(pageIndex, pageSize);
        }

        public Subject GetSubjectById(int? id)
        {
            var obj = from s in db.Subjects where s.subject_id == id select s;
            return obj.FirstOrDefault();
        }

        public string GetSubjectName(int subject, int bank)
        {
            IQueryable<Subject> obj;
            if (subject != 0)
            {
                obj = from s in db.Subjects
                      join b in db.BankQuestions on s.subject_id equals b.subject_id
                      where s.subject_id == subject
                      select s;
            }
            else
            {
                obj = from s in db.Subjects
                      join b in db.BankQuestions on s.subject_id equals b.subject_id
                      where b.bank_id == bank
                      select s;
            }
            return obj.FirstOrDefault().subject_name;
        }

        public void SubjectAOU(Subject s, int id, string aId)
        {
            if (id == 0)
            {
                Subject t = new Subject()
                {
                    subject_name = s.subject_name,
                    date_created = DateTime.Now,
                    account_id = aId
                };
                db.Subjects.Add(t);
                db.SaveChanges();
            }
            else
            {
                var obj = (from a in db.Subjects where a.subject_id == id select a).FirstOrDefault();
                obj.subject_name = s.subject_name;
            }
            db.SaveChanges();
        }
    }
}
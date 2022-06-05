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
    }
}
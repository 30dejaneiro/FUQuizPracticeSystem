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

        public IPagedList<Subject> PageSubject(int? page, string search)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from s in db.Subjects select s;
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.subject_name.Contains(search));
            }
            return obj.OrderBy(m => m.subject_id).ToPagedList(pageIndex, pageSize);
        }

        public Subject GetSubjectById(int? id)
        {
            var obj = from s in db.Subjects where s.subject_id == id select s;
            return obj.FirstOrDefault();
        }

        public void SubjectAOU(Subject s, int id)
        {
            DateTime localDate = DateTime.Now;
            if (id == 0)
            {
                Subject t = new Subject()
                {
                    subject_name = s.subject_name
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
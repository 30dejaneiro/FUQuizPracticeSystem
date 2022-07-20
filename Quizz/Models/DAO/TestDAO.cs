using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quizz.Models.ViewModel;
using Quizz.Models.EF;

namespace Quizz.Models.DAO
{
    public class TestDAO
    {
        private readonly QuizzDbContext db = null;

        public TestDAO()
        {
            db = new QuizzDbContext();
        }

        public IEnumerable<QuestionViewModel> GetTestByCode(string code)
        {
            var obj = (from q in db.ExamsQues
                       join e in db.Exams on q.exam_id equals e.exam_id
                       where e.code == code
                       select new QuestionViewModel()
                       {
                           QuestionId = q.exam_ques_id,
                           QuestionContent = q.content,
                           A = q.A,
                           B = q.B,
                           C = q.C,
                           D = q.D,
                           Answer = q.answer,
                       });
            return obj.ToList();
        }

        public IPagedList<Exam> PageTests(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from v in db.Exams
                      orderby v.exam_id
                      select v;
            return obj.ToPagedList(pageIndex, pageSize);
        }

        public List<Exam> GetTests()
        {
            var obj = from v in db.Exams select v;
            return obj.ToList();
        }

        public Exam GetTestById(int id)
        {

            var obj = from a in db.Exams where a.exam_id == id select a;
            return obj.FirstOrDefault();
        }

        public bool FindByTestCode(string code)
        {
            var obj = (from t in db.Exams where t.code == code select t).FirstOrDefault();
            return obj == null;
        }

        public int GetTotalQuesById(int id)
        {
            var obj = (from t in db.ExamsQues where t.exam_id == id select t).Count();
            return obj;
        }
        

        public int TestAOU(Exam vm, int id)
        {
            int status = 0;
            if (id == 0)
            {
                bool checkCode = FindByTestCode(vm.code);
                if (checkCode == true)
                {
                    Exam t = new Exam()
                    {
                        code = vm.code,
                        test_time = vm.test_time,
                        date_created = DateTime.Now,
                        num_of_ques = vm.num_of_ques,
                        total_tested = 0,
                    };
                    db.Exams.Add(t);
                    status = 1;
                }
            }
            else
            {
                var obj = (from a in db.Exams where a.exam_id == id select a).FirstOrDefault();
                obj.code = vm.code;
                obj.test_time = vm.test_time;
                obj.num_of_ques = vm.num_of_ques;
                status = 1;
            }
            db.SaveChanges();
            return status;
        }
    }
}
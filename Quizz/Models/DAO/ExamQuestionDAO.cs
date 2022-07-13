using PagedList;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.DAO
{
    public class ExamQuestionDAO
    {
        private readonly QuizzDbContext db = null;
        public ExamQuestionDAO()
        {
            db = new QuizzDbContext();
        }
        public IPagedList<ExamQuestionModel> PageQuestion(int? page, string search)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from l in db.ExamsQues
                      join q in db.Exams on l.exam_id equals q.exam_id
                      orderby l.exam_id descending
                      select new ExamQuestionModel()
                      {
                          ExamQuestionId = l.exam_ques_id,
                          QuestionContent = l.content,
                          A = l.A,
                          B = l.B,
                          C = l.C,
                          D = l.D,
                          Answer = l.answer,
                          ExamCode = q.code
                      }; ;
            if (!String.IsNullOrEmpty(search))
            {
                obj = (obj.Where(p => p.QuestionContent.Contains(search)));
            }
            return obj.ToPagedList(pageIndex, pageSize);
        }

        //public IPagedList<ExamQuestionModel> PageQuestionById(string id, int? page, string search)
        //{
        //    int pageSize = 10;
        //    int pageIndex = 1;
        //    pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    var obj = from l in db.ExamsQues
        //              join q in db.Exams on l.exam_id equals q.exam_id
        //              where b.account_id == id
        //              orderby l.question_id descending
        //              select new ExamQuestionModel()
        //              {
        //                  ExamQuestionId = l.exam_ques_id,
        //                  QuestionContent = l.content,
        //                  A = l.A,
        //                  B = l.B,
        //                  C = l.C,
        //                  D = l.D,
        //                  Answer = l.answer,
        //              }; ;
        //    if (!String.IsNullOrEmpty(search))
        //    {
        //        obj = (obj.Where(p => p.QuestionContent.Contains(search)));
        //    }
        //    return obj.ToPagedList(pageIndex, pageSize);
        //}

        public ExamQuestionModel GetQuestionById(int id)
        {
            var obj = from l in db.ExamsQues
                      join q in db.Exams on l.exam_id equals q.exam_id
                      where l.exam_ques_id == id
                      select new ExamQuestionModel()
                      {
                          ExamQuestionId = l.exam_ques_id,
                          QuestionContent = l.content,
                          A = l.A,
                          B = l.B,
                          C = l.C,
                          D = l.D,
                          Answer = l.answer,
                      };
            return obj.FirstOrDefault();
        }
        public bool QuestionAOU(ExamQuestionModel lp)
        {
            if (lp.ExamQuestionId == null)
            {
                ExamsQue q = new ExamsQue()
                {
                    content = lp.QuestionContent,
                    A = lp.A,
                    B = lp.B,
                    C = lp.C,
                    D = lp.D,
                    answer = lp.Answer,
                    exam_id = lp.ExamId,
            };
                db.ExamsQues.Add(q);
            }
            else
            {
                var obj = (from a in db.ExamsQues where a.exam_ques_id == lp.ExamQuestionId select a).FirstOrDefault();
                obj.content = lp.QuestionContent;
                obj.A = lp.A;
                obj.B = lp.B;
                obj.C = lp.C;
                obj.D = lp.D;
                obj.answer = lp.Answer;
                obj.exam_id = lp.ExamId;
            }
            db.SaveChanges();
            return true;
        }
    }
}
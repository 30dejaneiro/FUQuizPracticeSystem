using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var obj = (from q in db.Questions join qe in db.ExamsQues on q.question_id equals qe.question_id
                           join e in db.Exams on qe.exam_id equals e.exam_id
                           where e.code == code
                           select new QuestionViewModel()
                           {
                               QuestionId = q.question_id,
                               QuestionContent = q.content,
                               A = q.A,
                               B = q.B,
                               C = q.C,
                               D = q.D,
                               Answer = q.answer,
                           });
                return obj.ToList();
        }
    }
}
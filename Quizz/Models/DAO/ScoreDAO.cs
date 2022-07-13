using PagedList;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.DAO
{
    public class ScoreDAO
    {

        private readonly QuizzDbContext db = null;
        public ScoreDAO()
        {
            db = new QuizzDbContext();
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
                          DateTest = sc.date_test,
                          TotalQues = t.num_of_ques
                      };
            return obj.ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<ScoreTestViewModel> PageScores(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var obj = from sc in db.Scores
                      join t in db.Exams on sc.exam_id equals t.exam_id
                      orderby sc.account_id
                      select new ScoreTestViewModel()
                      {
                          TestCode = t.code,
                          AccountId = sc.account_id,
                          DateTest = sc.date_test,
                          TotalScore = sc.score1,
                          TotalQues = t.num_of_ques
                      };
            return obj.ToPagedList(pageIndex, pageSize);
        }
    }
}
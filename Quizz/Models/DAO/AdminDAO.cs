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

        public IPagedList<Account> PageStudent(int? page, string search)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from a in db.Accounts select a;
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.full_name.Contains(search));
            }
            return obj.OrderBy(m => m.account_id).ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<SubjectViewModel> PageSubject(int? page, string search)
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

        public IPagedList<Exam> PageTest(int? page)
        {

            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from v in db.Exams
                      orderby v.exam_id
                      select v;
            return obj.ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<ScoreTestViewModel> PageScore(int? page)
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
                          TotalScore = sc.score1
                      };
            return obj.ToPagedList(pageIndex, pageSize);
        }

        public IPagedList<BankViewModel> PageBank(int? page, string search)
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

        public IPagedList<QuestionViewModel> PageQuestion(int? page, string search)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from l in db.Questions
                      join b in db.BankQuestions on l.bank_id equals b.bank_id
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

        public Account GetStudentById(string roleNum)
        {
            var obj = from a in db.Accounts where a.account_id == roleNum select a;
            return obj.FirstOrDefault();
        }

        public Subject GetSubjectById(int? id)
        {
            var obj = from s in db.Subjects where s.subject_id == id select s;
            return obj.FirstOrDefault();
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

        public Exam GetTestById(int id)
        {

            var obj = from a in db.Exams where a.exam_id == id select a;
            return obj.FirstOrDefault();
        }

        public QuestionViewModel GetQuestionById(int id)
        {
            var obj = from l in db.Questions
                      join b in db.BankQuestions on l.bank_id equals b.bank_id
                      where l.question_id == id
                      select new QuestionViewModel()
                      {
                          QuestionId = l.question_id,
                          QuestionContent = l.content,
                          A = l.A,
                          B = l.B,
                          C = l.C,
                          D = l.D,
                          Answer = l.answer,
                          BankId = b.bank_id
                      };
            return obj.FirstOrDefault();
        }

        public bool FindByTestCode(string code)
        {
            var obj = (from t in db.Exams where t.code == code select t).FirstOrDefault();
            if (obj != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int TestAOU(Exam vm, int id)
        {
            int status = 0;
            if (id == 0)
            {
                bool checkCode = FindByTestCode(vm.code);
                if (checkCode == false)
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

        public void SubjectAOU(Subject s, int id,string aId)
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

        public void BankAOU(BankViewModel s, int? id,string accId)
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
                db.SaveChanges();
            }
            else
            {
                var obj = (from a in db.BankQuestions where a.bank_id == id select a).FirstOrDefault();
                obj.bank_name = s.BankName;
                obj.subject_id = s.SubjectId;
            }
            db.SaveChanges();
        }

        public bool QuestionAOU(QuestionViewModel lp)
        {
            if (lp.QuestionId == null)
            {
                Question q = new Question()
                {
                    content = lp.QuestionContent,
                    A = lp.A,
                    B = lp.B,
                    C = lp.C,
                    D = lp.D,
                    answer = lp.Answer,
                    bank_id = lp.BankId
                    
                };
                db.Questions.Add(q);
            }
            else
            {
                var obj = (from a in db.Questions where a.question_id == lp.QuestionId select a).FirstOrDefault();
                obj.content = lp.QuestionContent;
                obj.A = lp.A;
                obj.B = lp.B;
                obj.C = lp.C;
                obj.D = lp.D;
                obj.answer = lp.Answer;
                obj.bank_id = lp.BankId;
            }
            db.SaveChanges();
            return true;
        }
        public bool SaveOrUpdate(Account st, string id)
        {
            var students = (from row in db.Accounts select row).OrderByDescending(m => m.account_id).Take(1);
            int number = Convert.ToInt32(students.FirstOrDefault().account_id.Last().ToString());

            if (id == null)
            {
                Account a = new Account
                {
                    account_id = "Ms00" + (number + 1),
                    role = st.role,
                    gender = st.gender,
                    dob = st.dob,
                    username = st.username,
                    password = st.password,
                    full_name = st.full_name
                };
                db.Accounts.Add(a);
            }
            else
            {
                var obj = (from a in db.Accounts where a.account_id.Equals(id) select a).FirstOrDefault();
                obj.full_name = st.full_name;
                obj.dob = st.dob;
                obj.gender = st.gender;
                obj.role = st.role;
            }
            db.SaveChanges();
            return true;
        }
    }
}
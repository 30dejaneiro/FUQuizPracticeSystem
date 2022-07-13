using PagedList;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.DAO
{
    public class UserDAO
    {
        private readonly QuizzDbContext db = null;
        public UserDAO()
        {
            db = new QuizzDbContext();
        }
        public List<AccountViewModel> GetAllAccount()
        {
            var obj = from s in db.Accounts
                      select new AccountViewModel()
                      {
                          AccountId = s.account_id,
                          FullName = s.full_name,
                          Dob = s.dob,
                          Gender = s.gender,
                          Username = s.username,
                          Password = s.password,
                          Role = s.role
                      };
            return obj.ToList();
        }

        public IPagedList<AccountViewModel> PageUsers(int? page, string search)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from s in db.Accounts
                      select new AccountViewModel()
                      {
                          AccountId = s.account_id,
                          FullName = s.full_name,
                          Dob = s.dob,
                          Gender = s.gender,
                          Username = s.username,
                          Password = s.password,
                          Role = s.role
                      };
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.FullName.Contains(search));
            }
            return obj.OrderBy(m => m.AccountId).ToPagedList(pageIndex, pageSize);
        }

        public AccountViewModel GetUserById(string roleNum)
        {
            var obj = from s in db.Accounts where s.account_id == roleNum
                      select new AccountViewModel()
                      {
                          AccountId = s.account_id,
                          FullName = s.full_name,
                          Dob = s.dob,
                          Gender = s.gender,
                          Username = s.username,
                          Password = s.password,
                          Role = s.role
                      };
            return obj.FirstOrDefault();
        }

        public bool UserAOU(AccountViewModel st, string id)
        {
            var students = (from row in db.Accounts select row).OrderByDescending(m => m.account_id).Take(1);
            int number = Convert.ToInt32(students.FirstOrDefault().account_id.Last().ToString());

            if (id == null)
            {
                Account a = new Account
                {
                    account_id = "Ms00" + (number + 1),
                    role = st.Role,
                    gender = st.Gender,
                    dob = st.Dob,
                    username = st.Username,
                    password = st.Password,
                    full_name = st.FullName
                };
                db.Accounts.Add(a);
            }
            else
            {
                var obj = (from a in db.Accounts where a.account_id.Equals(id) select a).FirstOrDefault();
                obj.full_name = st.FullName;
                obj.dob = st.Dob;
                obj.gender = st.Gender;
                obj.role = st.Role;
            }
            db.SaveChanges();
            return true;
        }
    }
}
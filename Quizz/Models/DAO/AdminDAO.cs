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
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var obj = from a in db.Accounts select a;
            if (!String.IsNullOrEmpty(search))
            {
                obj = obj.Where(p => p.full_name.Contains(search));
            }
            return obj.OrderBy(m => m.account_id).ToPagedList(pageIndex, pageSize);
        }

        public Account GetStudentById(string roleNum)
        {
            var obj = from a in db.Accounts where a.account_id == roleNum select a;
            return obj.FirstOrDefault();
        }

        public bool SaveOrUpdate(Account st, string id)
        {
            var totalStudent = (from row in db.Accounts select row).Count();
            if (id == null)
            {
                Account a = new Account
                {
                    account_id = "Ms00" + (totalStudent + 1),
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
using Quizz.Models;
using Quizz.Models.EF;
using Quizz.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toeic_Quizz.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthViewModel m)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                var data = db.Accounts.Where(s => s.username.Equals(m.Account.username) && s.password.Equals(m.Account.password)).FirstOrDefault();
                if (data != null)
                {
                    Session["account"] = data.account_id;
                    Session["fullName"] = data.full_name;
                    Session["role"] = data.role == true ? "Admin" : "Student";
                    Session.Timeout = 60;
                    if (data.role)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    return View("Login", data);
                }
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(AuthViewModel m)
        {
            using (QuizzDbContext db = new QuizzDbContext())
            {
                if (m.Account.password == m.Repassword)
                {
                    var check = db.Accounts.FirstOrDefault(s => s.username == m.Account.username);
                    if (check == null)
                    {
                        db.Configuration.ValidateOnSaveEnabled = true;
                        var students = (from row in db.Accounts orderby row.account_id descending select row).Take(1);
                        int number = Convert.ToInt32(students.FirstOrDefault().account_id.Last().ToString());
                        Account a = new Account
                        {
                            account_id = "Ms00" + (number + 1),
                            username = m.Account.username,
                            password = m.Account.password,
                            full_name = "Default name",
                            role = false,
                        };
                        Score s = new Score
                        {
                            account_id = "Ms00" + (number + 1),
                            score1 = 0,
                        };
                        db.Accounts.Add(a);
                        db.Scores.Add(s);
                        db.SaveChanges();
                        ViewBag.Message = "Register Successfully!";
                        return View("Login");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Username is exists!";
                        return View("SignUp");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Re-password is not correct!";
                    return View("SignUp");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
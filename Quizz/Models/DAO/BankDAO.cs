using Quizz.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.DAO
{
    public class BankDAO
    {
        QuizzDbContext db;
        public BankDAO()
        {
            db = new QuizzDbContext();
        }
        public List<BankQuestion> GetAllBanks()
        {
            var obj = from b in db.BankQuestions select b;
            return obj.ToList();
        }
    }
}
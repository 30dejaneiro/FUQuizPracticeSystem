using Quizz.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toeic_Quizz.Controllers
{
    public class NotFoundController : BaseController
    {
        // GET: NotFound
        public ActionResult Index()
        {
            return View();
        }
    }
}
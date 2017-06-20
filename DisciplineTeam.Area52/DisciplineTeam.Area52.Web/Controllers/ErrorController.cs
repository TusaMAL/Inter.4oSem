using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Erro404()
        {
            return View();
        }
    }
}
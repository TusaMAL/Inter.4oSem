using DisciplineTeam.Area52.Web.Models;
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
            try
            {
            if (((Admin)Session["usuario"]).Status == 2)
            {
                ViewBag.StatusAdmin = ((Admin)Session["usuario"]).Status;
            }
            return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
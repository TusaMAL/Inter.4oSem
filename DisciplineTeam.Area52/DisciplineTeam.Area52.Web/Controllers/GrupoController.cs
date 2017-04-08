using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class GrupoController : Controller
    {
        // GET: Grupos
        public ActionResult Index()
        {
            return View();
        }
        //GET: Search
        public ActionResult Groups()
        {
            return View();
        }
        //GET: Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: Usuario/Friends
        public ActionResult Members()
        {
            return View();
        }
    }
}
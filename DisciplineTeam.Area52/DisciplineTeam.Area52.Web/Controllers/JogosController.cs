using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class JogosController : Controller
    {
        public static List<Jogo> lista = new List<Jogo>();

        // GET: Jogos
        public ActionResult Index()
        {
            return View(lista);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Jogo j)
        {
            if (ModelState.IsValid)
            {
                lista.Add(j);

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensagem = "Falha no cadastro!";
                return View(j);
            }
        }
    }
}
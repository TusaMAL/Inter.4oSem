using DisciplineTeam.Area52.Web.Filtro;
using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class JogoController : Controller
    {
        //public static List<Jogo> lista = new List<Jogo>();
        [UsuarioFiltro]
        // GET: Jogos
        public ActionResult Index()
        {
            using (JogoModel model = new JogoModel())
            {
                Admin user = (Admin)Session["usuario"];
                ViewBag.StatusAdmin = user.Status;
                return View(model.ReadJogos());

            } // model.Dispose();
        }
        [UsuarioFiltro]
        public ActionResult Create()
        {
            Admin user = (Admin)Session["usuario"];
            ViewBag.StatusAdmin = user.Status;
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Create(Jogo e)
        {
            Admin user = (Admin)Session["usuario"];
            ViewBag.StatusAdmin = user.Status;
            if (ModelState.IsValid)
            {
                using (JogoModel model = new JogoModel())
                {
                    int id = ((Admin)Session["usuario"]).IdPessoa;
                    model.Create(e, id);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensagem = "Falha no cadastro!";
                return View(e);
            }
        }
    }
}
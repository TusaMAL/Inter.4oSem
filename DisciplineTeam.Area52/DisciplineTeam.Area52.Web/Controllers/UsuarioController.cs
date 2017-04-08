using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }
        //GET: Person
        public ActionResult Person()
        {
            return View();
        }
        // Get: Usuario/User
        public ActionResult User()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult ForgotP()
        {
            return View();
        }
        // GET: Usuario
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario e)
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                Usuario user = model.Read(e.Email, e.Senha);

                if (user == null)
                {
                    ViewBag.Erro = "Informações inválidas";
                }
                else
                {
                    Session["usuario"] = user;
                    return RedirectToAction("Index", "Produto");
                }
            }
            return View();
        }
        // GET: Usuario
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Usuario e)
        {
            if (ModelState.IsValid)
            {
                using (UsuarioModel model = new UsuarioModel())
                {
                    model.Create(e);
                    return RedirectToAction("Index");
                }
            }
            return View(e);
        }
    }
}
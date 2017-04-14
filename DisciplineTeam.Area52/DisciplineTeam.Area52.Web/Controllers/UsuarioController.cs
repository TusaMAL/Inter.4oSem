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
        /* Faz o login e chama o metodo .Read do UsuarioModel para ler os dados do banco*/
        [HttpPost]
        public ActionResult Login(Usuario e)
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                Usuario user = model.Read(e.Email, e.Senha);
                /*Retorna mensagem de erro caso as informações estejam diferentes no banco pois vai retornar um objeto vazio*/
                if (user == null)
                {
                    ViewBag.Erro = "Wrong login data!";
                }
                else
                {
                    /*Cria a sessão do usuario e redireciona para a pagina do profile*/
                    Session["usuario"] = user;
                    return RedirectToAction("Index", "Usuario");
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
        public ActionResult Create(Usuario e, string Sucesso)
        {
            if (ModelState.IsValid)
            {
                /* faz com que o conteudo não seja nulo para que seja exibido mensagem de confirmação na pagina login*/
                TempData["Sucesso"] = "true";
                using (UsuarioModel model = new UsuarioModel())
                {
                    if (model.Check(e))
                    {
                        model.Create(e);
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Erro = "Usuário já cadastrado";
                    }
                }
            }
            return View(e);
        }
    }
}
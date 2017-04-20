using DisciplineTeam.Area52.Web.Models;
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
            //Faz com que apareça os jogos ja cadastrados no combobox igual o henrique fez pra mostrar o negocio dos Cards lá
            using (GrupoModel model = new GrupoModel())
            {
                return View(model.Read());
            } // model.Dispose();
        }
        [HttpPost]
        public ActionResult Create(Grupo e)
        {
            if (ModelState.IsValid)
            {
                using (GrupoModel model = new GrupoModel())
                {
                    int id = ((Usuario)Session["usuario"]).IdPessoa;
                    //Passando o id do criador do grupo como parametro
                    model.Create(e, id);
                    return RedirectToAction("Index");
                }
            }
            return View(e);
        }
        // GET: Usuario/Friends
        public ActionResult Members()
        {
            return View();
        }
    }
}
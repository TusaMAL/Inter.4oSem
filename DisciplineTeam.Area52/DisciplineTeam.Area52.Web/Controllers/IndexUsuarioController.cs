using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class IndexUsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            List<Grupo> listgrupo = new List<Grupo>();
            using (UsuarioModel model = new UsuarioModel())
            {
                Usuario user = (Usuario)Session["usuario"];
                int id = user.IdPessoa;
                user = model.ReadU(id);
                ViewBag.UserLog = user;
            }
            using (GrupoModel model = new GrupoModel())
            {
                Usuario user = (Usuario)Session["usuario"];
                int id = user.IdPessoa;
                List<Grupo> grupo = new List<Grupo>();
                listgrupo = model.ReadGrupo(id);
            }
            return View(listgrupo);
        }
    }
}
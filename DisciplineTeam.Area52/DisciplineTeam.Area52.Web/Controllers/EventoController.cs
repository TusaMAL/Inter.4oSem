using DisciplineTeam.Area52.Web.Filtro;
using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class EventoController : Controller
    {
        // GET: Eventos
        [UsuarioFiltro]
        public ActionResult Index()
        {
            return View();
        }
        [UsuarioFiltro]
        public ActionResult Create()
        {
            using (GrupoModel model = new GrupoModel())
            {
                int idgrupo = int.Parse(Request.QueryString["GrupoId"]);            //Converte o Id da URL para poder ser usado
                Usuario user = (Usuario)Session["usuario"];
                int idusuario = user.IdPessoa;         
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                        
            }
            return View();
        }
    }
}
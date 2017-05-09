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
            int idgrupo = int.Parse(Request.QueryString[0]);                        //Converte o primeiro parametro que vem da URL
            int idevento = int.Parse(Request.QueryString[1]);                       //Converte o segundo parametro que vem da URL
            using (EventoModel model = new EventoModel())
            {
                ViewBag.ReadEvento = model.ReadEvento(idevento, idgrupo);           //Pega as informações do evento       
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar 
            }
            return View();
        }
        [UsuarioFiltro]
        public ActionResult Create()
        {
            int idgrupo = int.Parse(Request.QueryString[0]);                    //Converte o primeiro parametro da URL para poder ser usado        
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                   //Pega as informações do grupo pra mostrar       
            }

            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Create(Evento e)
        {
            int idgrupo = int.Parse(Request.QueryString[0]);                    //Converte o primeiro parametro da URL para poder ser usado        
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                   //Pega as informações do grupo pra mostrar       
            }
            if (ModelState.IsValid)
            {
                using (EventoModel model = new EventoModel())
                {
                    model.Create(e, idgrupo);                                   //Cria o evento
                }
            }
            return RedirectToAction("Index", "Grupo", new { GrupoId = idgrupo });
        }
    }
}
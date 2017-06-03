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
            int iduser = ((Usuario)Session["usuario"]).IdPessoa;
            int idgrupo = int.Parse(Request.QueryString[0]);                        //Converte o primeiro parametro que vem da URL
            int idevento = int.Parse(Request.QueryString[1]);                       //Converte o segundo parametro que vem da URL

            Evento e = new Evento();
            ViewBag.IdUsuario = iduser;
            using (EventoModel model = new EventoModel())
            {
                e = model.ReadEvento(idevento, idgrupo);                                        //Pega as informações do evento 
                ViewBag.ReadEvento = e;
                DateTime date = Convert.ToDateTime(e.Data);
                ViewBag.DataFormatada = date.ToString(@"dd-MM-yyyy");                           //Converte a data pro formato de dia/mes/ano
                ViewBag.ViewConfUserEvento = model.ViewConfUserEvento(idgrupo, idevento);       //Mostra os usuarios com presença confirmada
                ViewBag.QuantUserPartEvento = model.QuantUserPartEvento(idgrupo, idevento);     //Retorna o count de usuarios que vão ao evento
                ViewBag.UserStatusEvento = model.UserStatusEvento(idgrupo, iduser, idevento);   //Pega o status do usuario no evento para mostrar na view
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar 
                ViewBag.StatusUserGrupo = model.StatusUserGrupo(iduser, idgrupo);
            }
            return View(e);
        }
        [UsuarioFiltro]
        //GET
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
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnPartEvento()
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            int idevento = int.Parse(Request.QueryString[1]);
            int iduser = int.Parse(Request.QueryString[2]);

            using (EventoModel model = new EventoModel())
            {
                model.PartEvento(idgrupo, iduser, idevento);
            }
            return RedirectToAction("Index", "Evento", new { GrupoId = idgrupo, EventoId = idevento });
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnSairEvento()
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            int idevento = int.Parse(Request.QueryString[1]);
            int iduser = int.Parse(Request.QueryString[2]);

            using (EventoModel model = new EventoModel())
            {
                model.SairEvento(idgrupo, iduser, idevento);
            }
            return RedirectToAction("Index", "Evento", new { GrupoId = idgrupo, EventoId = idevento });
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnPartEventoUpdate()
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            int idevento = int.Parse(Request.QueryString[1]);
            int iduser = int.Parse(Request.QueryString[2]);

            using (EventoModel model = new EventoModel())
            {
                model.PartEventoUpdate(idgrupo, iduser, idevento);
            }
            return RedirectToAction("Index", "Evento", new { GrupoId = idgrupo, EventoId = idevento });
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnEditEvento(Evento e)
        {
            e.IdGrupo = int.Parse(Request.QueryString[0]);
            e.IdEvento = int.Parse(Request.QueryString[1]);
            using (EventoModel model = new EventoModel())
            {
                model.EditInfoEvento(e);
            }
            return RedirectToAction("Index", "Evento", new { GrupoId = e.IdGrupo, EventoId = e.IdEvento });
        }
        [UsuarioFiltro]
        //GET
        public ActionResult ViewEventos()
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            using (EventoModel model = new EventoModel())
            {
                ViewBag.ViewEventos = model.ViewEventos(idgrupo);
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);
            }
                return View();
        }
    }
}
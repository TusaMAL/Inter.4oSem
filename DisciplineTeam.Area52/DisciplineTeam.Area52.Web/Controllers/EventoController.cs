using DisciplineTeam.Area52.Web.Filtro;
using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            Evento e = new Evento();
            ViewBag.Endereco = e;

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
                    DateTime date = DateTime.Now;
                    DateTime dataevento = Convert.ToDateTime(e.Data);
                    if (dataevento < date)
                    {
                        TempData["DataInvalida"] = "Your event date is older than the current date, for creating an event please use a newer date.";
                        return View(e);
                    }
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
            return RedirectToAction("Index", "Evento", new { GrupoId = e.IdGrupo, EventoId = e.IdEvento, e });
        }
        [UsuarioFiltro]
        public ActionResult EditEvento()
        {
            Evento e = new Evento();
            e.IdGrupo = int.Parse(Request.QueryString[0]);
            e.IdEvento = int.Parse(Request.QueryString[1]);
            using (EventoModel model = new EventoModel())
            {
                e = model.ReadEvento(e.IdEvento, e.IdGrupo);
                ViewBag.EventoId = e.IdEvento;
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(e.IdGrupo);
            }
            return View(e);
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
        public ActionResult Members()
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            int idevento = int.Parse(Request.QueryString[1]);

            Evento e = new Evento();
            using (EventoModel model = new EventoModel())
            {
                e = model.ReadEvento(idevento, idgrupo);                                            //Pega as informações do evento 
                ViewBag.ReadEvento = e;
                DateTime date = Convert.ToDateTime(e.Data);
                ViewBag.DataFormatada = date.ToString(@"dd-MM-yyyy");                               //Converte a data pro formato de dia/mes/ano
                ViewBag.ViewConfUserEventoAll = model.ViewConfUserEventoAll(idgrupo, idevento);     //Mostra os usuarios com presença confirmada
                ViewBag.QuantUserPartEvento = model.QuantUserPartEvento(idgrupo, idevento);         //Retorna o count de usuarios que vão ao evento
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar 
            }
            return View(e);
        }
        [HttpPost]
        public async Task<ActionResult> Endereco(FormCollection endereco)
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            string CEP = endereco["Cep"];
            string url = "http://viacep.com.br/ws/" + CEP + "/json/";

            if (Validacoes.VerificarValidadeDoCep(CEP) == false)
            {
                TempData["CepInvalido"] = "Invalid Zip-Code!";
                return RedirectToAction("Create", "Evento", new { GrupoId = idgrupo });
            }
                Evento e = new Evento();
            //using System.Net.Http;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            e = await response.Content.ReadAsAsync<Evento>();
            TempData["Cep"] = e.Cep;
            TempData["Localidade"] = e.Localidade + " - " + e.Uf;
            TempData["Bairro"] = e.Bairro;
            TempData["Logradouro"] = e.Logradouro;


            return RedirectToAction("Create", "Evento", new { GrupoId = idgrupo });
        }
        [HttpPost]
        public async Task<ActionResult> EnderecoEdit(FormCollection endereco)
        {
            int idgrupo = int.Parse(Request.QueryString[0]);
            int idevento = int.Parse(Request.QueryString[1]);
            string CEP = endereco["Cep"];
            string url = "http://viacep.com.br/ws/" + CEP + "/json/";
            Evento e = new Evento();
            if (Validacoes.VerificarValidadeDoCep(CEP) == false)
            {
                TempData["CepInvalido"] = "Invalid Zip-Code!";
                return RedirectToAction("EditEvento", "Evento", new { GrupoId = idgrupo, EventoId = idevento });
            }
            
            //using System.Net.Http;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            e = await response.Content.ReadAsAsync<Evento>();
            TempData["Cep"] = e.Cep;
            TempData["Localidade"] = e.Localidade + " - " + e.Uf;
            TempData["Bairro"] = e.Bairro;
            TempData["Logradouro"] = e.Logradouro;


            return RedirectToAction("EditEvento", "Evento", new { GrupoId = idgrupo, EventoId = idevento });
        }
        //Get Usuario dos eventos
        


    }
}
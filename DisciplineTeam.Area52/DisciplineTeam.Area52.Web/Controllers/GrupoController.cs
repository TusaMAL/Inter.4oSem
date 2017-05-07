using DisciplineTeam.Area52.Web.Filtro;
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
        [UsuarioFiltro]
        // GET: Grupos
        public ActionResult Index()
        {
            int idgrupo = int.Parse(Request.QueryString["GrupoId"]);            //Converte o Id da URL para poder ser usado
            Usuario user = (Usuario)Session["usuario"];
            int idusuario = user.IdPessoa;
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.ReadPartGrupo = model.ReadPartGrupo(idusuario);             //Seleciona 6 primeiros usuarios e mostra na lista do grupo
                ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                ViewBag.QuantUserGrupos = model.QuantUserGrupos(idgrupo);           //Mostra o count de usuarios na div de grupos
            }
            using (MensagemModel model = new MensagemModel())
            {
                ViewBag.ReadMensagem = model.ReadMensagem(idgrupo, idusuario);      //Ler as mensagens já postadas no grupo
            }
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Index(Mensagem e)
        {
            if (ModelState.IsValid)
            {
                int idgrupo = int.Parse(Request.QueryString["GrupoId"]);            //Converte o Id da URL para poder ser usado
                Usuario user = (Usuario)Session["usuario"];
                int idusuario = user.IdPessoa;
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.ReadPartGrupo = model.ReadPartGrupo(idgrupo);               //Seleciona 6 primeiros usuarios e mostra na lista do grupo
                    ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                    ViewBag.QuantUserGrupos = model.QuantUserGrupos(idgrupo);           //Mostra o count de usuarios na div de grupos
                }
                using (MensagemModel model = new MensagemModel())
                {
                    model.PostMensagem(e, idusuario, idgrupo);                          //Model pra fazer post da mensagem
                    ViewBag.ReadMensagem = model.ReadMensagem(idgrupo, idusuario);      //Ler as mensagens já postadas no grupo
                }
            }
            return View();
        }
        [UsuarioFiltro]
        //GET: Search
        public ActionResult Groups()
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                Usuario user = (Usuario)Session["usuario"];
                int id = user.IdPessoa;
                user = model.ReadU(id);                                             //Lê informações do usuario para jogar na ViewBag
                ViewBag.UserLog = user;
            }
            using (GrupoModel model = new GrupoModel())
            {
                Usuario user = (Usuario)Session["usuario"];
                int id = user.IdPessoa;
                List<ViewAll> listgrupo = new List<ViewAll>();                      //Cria lista de Grupo na Classe ViewAll que tem todos os atributos das classes
                listgrupo = model.ReadGrupoTotal(id);                               //Lê os grupos que o usuario participa e coloca na lista
                ViewBag.Grupos = listgrupo;                                         //Coloca a lista na viewBag pra mostrar na view
                ViewBag.Quantgrupopart = model.QuantGruposParticipa(id);            //Retorna o count de grupos que o usuario está
            }
            return View();
        }
        [UsuarioFiltro]
        //GET: Create
        public ActionResult Create()
        {
            using (JogoModel model = new JogoModel())
            {
                List<Jogo> lista = new List<Jogo>();
                lista = model.ReadJogos();                                          //Lê os jogos que estão cadastrados e joga na lista
                ViewBag.ListaJogos = lista;                                         //Manda a lista de jogos para a SelectList da View para aparecer o dropdownbox com os jogos
            }
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Create(Grupo e ,int IdJogo)
        {
            if (ModelState.IsValid)
            {
                using (GrupoModel model = new GrupoModel())
                {
                    int iduser = ((Usuario)Session["usuario"]).IdPessoa;
                    //Passando o id do criador do grupo como parametro
                    model.Create(e, iduser, IdJogo);                                
                    return RedirectToAction("Groups", "Grupo");
                }
            }
            return View();
        }
        [UsuarioFiltro]
        // GET: Usuario/Friends
        public ActionResult Members()
        {
            using (GrupoModel model = new GrupoModel())
            {
                Usuario user = (Usuario)Session["usuario"];
                int idusuario = user.IdPessoa;                                      
                ViewBag.ReadMembrosGrupoTotal = model.ReadMembrosGrupoTotal(idusuario); //Retorna a quantidade de membros participantes do grupo
                if (Request.QueryString["GrupoID"] != null)
                {
                    int idgrupo = int.Parse(Request.QueryString["GrupoID"]);            //Converte o ID da URL para int
                    ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                    ViewBag.QuantUserGrupos = model.QuantUserGrupos(idgrupo);           //Retorna o count de usuarios do grupo
                }
                else
                {
                    using (UsuarioModel modeluser = new UsuarioModel())
                    {
                        ViewBag.UserLog = modeluser.ReadU(int.Parse(Request.QueryString["UserID"]));    //Pega o Id do usuario pela URL com a QueryString e Joga as informações lidas na ViewBag para mostrar na view
                    }
                }
               
            }
            return View();
        }
        //Get Search
        public ActionResult Search()
        {
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Search(string busca)
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                ViewBag.UserLog = model.ReadU(((Usuario)Session["usuario"]).IdPessoa);  //Recebe Id do usuario pela session, pega os dados do mesmo e coloca na ViewBag para mostrar na View   
            }
            using (GrupoModel model = new GrupoModel())
            {         
                ViewBag.BuscaGrupo = model.BuscarGrupo(busca);                           //Busca e retorna informações de busca do jogo
            }
            return View();
        }
        //GET
        [UsuarioFiltro]
        public ActionResult Person()
        {
            int iduser = int.Parse(Request.QueryString["UserID"]);
            using (UsuarioModel model = new UsuarioModel())
            {
                ViewBag.UserLog = model.ReadU(iduser);                                  //Recebe Id do usuario pela session, pega os dados do mesmo e coloca na ViewBag para mostrar na View
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.Grupos = model.ReadGrupo(iduser);                               //Lê os grupos em que o usuario escolhido participa para mostrar na view
                ViewBag.Quantgrupopart = model.QuantGruposParticipa(iduser);            //Retorna o count dos grupos em que o usuario escolhido participa pra mostrar
            }
            return View();
        }
        
    }
}
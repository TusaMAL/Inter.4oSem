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
            try
            {
                int quant;
                if (Request.QueryString.Keys.Count > 1)
                {
                    quant = int.Parse(Request.QueryString[1]);
                    if (quant != 10 && quant != 25 && quant != 50 && quant != 999)
                    {
                        ViewBag.ErroQuant = true;
                        quant = 10;
                    }
                }
                else
                {
                    quant = 10;
                }
                int iduser = ((Usuario)Session["usuario"]).IdPessoa;
                int idgrupo = int.Parse(Request.QueryString[0]);            //Converte o Id da URL para poder ser usado
                ViewBag.IdUsuario = iduser;
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.ReadPartGrupo = model.ReadPartGrupo(idgrupo);               //Seleciona 6 primeiros usuarios e mostra na lista do grupo
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.QuantUserGrupos = model.QuantUserGrupos(idgrupo);           //Mostra o count de usuarios na div de grupos
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.StatusUserGrupo = model.StatusUserGrupo(iduser, idgrupo);   //Pega o status do usuario para mostrar os botões para interagir no site
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.QuantModGrupo = model.QuantModGrupo(idgrupo);               //Retora o count de moderadores do grupo
                }
                using (MensagemModel model = new MensagemModel())
                {
                    ViewBag.ReadMensagem = model.ReadMensagem(idgrupo, quant);      //Ler as mensagens já postadas no grupo
                }
                using (MensagemModel model = new MensagemModel())
                {
                    ViewBag.QuantMsgGrupo = model.QuantMsgGrupo(idgrupo);
                }
                using (EventoModel model = new EventoModel())
                {
                    ViewBag.ViewEventosIndex = model.ViewEventosIndex(idgrupo);               //Mostra os eventos cadastrados no grupo
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Index(Mensagem e)
        {
            try
            {
                int iduser = ((Usuario)Session["usuario"]).IdPessoa;
                ViewBag.IdUsuario = iduser;
                if (ModelState.IsValid)
                {
                    int idgrupo = int.Parse(Request.QueryString["GrupoId"]);                        //Converte o Id da URL para poder ser usado
                    using (GrupoModel model = new GrupoModel())
                    {
                        ViewBag.ReadPartGrupo = model.ReadPartGrupo(idgrupo);               //Seleciona 6 primeiros usuarios e mostra na lista do grupo
                    }
                    using (GrupoModel model = new GrupoModel())
                    {
                        ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                    }
                    using (GrupoModel model = new GrupoModel())
                    {
                        ViewBag.QuantUserGrupos = model.QuantUserGrupos(idgrupo);           //Mostra o count de usuarios na div de grupos
                    }
                    using (GrupoModel model = new GrupoModel())
                    {
                        ViewBag.StatusUserGrupo = model.StatusUserGrupo(iduser, idgrupo);   //Retorna o status pra mostra o botão pro usuario
                    }
                    using (MensagemModel model = new MensagemModel())
                    {
                        model.PostMensagem(e, iduser, idgrupo);                          //Model pra fazer post da mensagem
                    }
                    using (MensagemModel model = new MensagemModel())
                    {
                        ViewBag.ReadMensagem = model.ReadMensagem(idgrupo, 10);                 //Ler as mensagens já postadas no grupo
                    }
                    using (MensagemModel model = new MensagemModel())
                    {
                        ViewBag.QuantMsgGrupo = model.QuantMsgGrupo(idgrupo);
                    }
                    using (EventoModel model = new EventoModel())
                    {
                        ViewBag.ViewEventosIndex = model.ViewEventosIndex(idgrupo);                   //Mostra os eventos cadastrados no grupo
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        //GET: Search
        public ActionResult Groups()
        {
            try
            {
                int iduser = 0;

                if (Request.QueryString.Keys.Count > 0)
                {
                    iduser = int.Parse(Request.QueryString["UserID"]);
                }
                else
                {
                    iduser = ((Usuario)Session["usuario"]).IdPessoa;
                }
                using (UsuarioModel model = new UsuarioModel())
                {
                    ViewBag.ReadU = model.ReadU(iduser);                                  //Recebe Id do usuario pela session, pega os dados do mesmo e coloca na ViewBag para mostrar na View
                }
                using (UsuarioModel model = new UsuarioModel())
                {
                    ViewBag.GetAgeUser = model.GetAgeUser(iduser);                      //Pegar idade
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.Grupos = model.ReadGrupoTotal(iduser);   //Coloca a lista na viewBag pra mostrar na view
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.Quantgrupopart = model.QuantGruposParticipa(iduser);                        //Retorna o count de grupos que o usuario está
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        //GET: Create
        public ActionResult Create()
        {
            try
            {
                using (JogoModel model = new JogoModel())
                {
                    ViewBag.ReadJogos = model.ReadJogos();     //Manda a lista de jogos para a SelectList da View para aparecer o dropdownbox com os jogos
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Create(Grupo e, int IdJogo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string img;
                    using (JogoModel model = new JogoModel())
                    {
                        img = model.ReadJogoImg(IdJogo);
                    }
                    using (GrupoModel model = new GrupoModel())
                    {
                        int iduser = ((Usuario)Session["usuario"]).IdPessoa;
                        model.Create(e, iduser, IdJogo, img);                    //Passando o id do criador do grupo como parametro
                        return RedirectToAction("Groups", "Grupo");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        // GET: Usuario/Friends
        public ActionResult Members()
        {
            try
            {
                int iduser = ((Usuario)Session["usuario"]).IdPessoa;
                int idgrupo = int.Parse(Request.QueryString[0]);
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.ReadMembrosGrupoTotal = model.ReadMembrosGrupoTotal(idgrupo); //Retorna a quantidade de membros participantes do grupo
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.InfoGrupo = model.InfoGrupo(idgrupo);                       //Pega as informações do grupo pra mostrar
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.QuantUserGrupos = model.QuantUserGrupos(idgrupo);           //Retorna o count de usuarios do grupo
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.StatusUserGrupo = model.StatusUserGrupo(iduser, idgrupo);   //Retorna o status pra mostra o botão pro usuario
                    ViewBag.IdUsuario = iduser;
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        //Get Search
        [UsuarioFiltro]
        public ActionResult Search()
        {
            try
            {
                int iduser = ((Usuario)Session["usuario"]).IdPessoa;
                if (Request.QueryString.Keys.Count > 0)
                {
                    string busca = (Request.QueryString[0]);                                    //Recebe o primeiro parametro da URL
                    using (UsuarioModel model = new UsuarioModel())
                    {
                        ViewBag.ReadU = model.ReadU(((Usuario)Session["usuario"]).IdPessoa);  //Recebe Id do usuario pela session, pega os dados do mesmo e coloca na ViewBag para mostrar na View   
                    }
                    using (UsuarioModel model = new UsuarioModel())
                    {
                        ViewBag.GetAgeUser = model.GetAgeUser(iduser);
                    }
                    using (GrupoModel model = new GrupoModel())
                    {
                        ViewBag.BuscaGrupo = model.BuscarGrupo(busca);                           //Busca e retorna informações de busca do jogo
                    }
                    return View();
                }
                else
                {
                    using (UsuarioModel model = new UsuarioModel())
                    {
                        ViewBag.ReadU = model.ReadU(iduser);  //Recebe Id do usuario pela session, pega os dados do mesmo e coloca na ViewBag para mostrar na View   
                    }
                    using (UsuarioModel model = new UsuarioModel())
                    {
                        ViewBag.GetAgeUser = model.GetAgeUser(iduser);
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        //GET
        [UsuarioFiltro]
        public ActionResult Person()
        {
            try
            {
                int iduser = int.Parse(Request.QueryString["UserID"]);
                using (UsuarioModel model = new UsuarioModel())
                {
                    ViewBag.ReadU = model.ReadU(iduser);                                  //Recebe Id do usuario pela session, pega os dados do mesmo e coloca na ViewBag para mostrar na View
                }
                using (UsuarioModel model = new UsuarioModel())
                {
                    ViewBag.GetAgeUser = model.GetAgeUser(iduser);
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.ReadGrupo = model.ReadGrupo(iduser);                               //Lê os grupos em que o usuario escolhido participa para mostrar na view
                }
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.QuantGruposParticipa = model.QuantGruposParticipa(iduser);            //Retorna o count dos grupos em que o usuario escolhido participa pra mostrar
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnPartGrupo()
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                using (GrupoModel model = new GrupoModel())
                {
                    model.PartGrupo(iduser, idgrupo);
                }
                return RedirectToAction("Index", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnSairGrupo()
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                using (GrupoModel model = new GrupoModel())
                {
                    model.SairGrupo(iduser, idgrupo);
                }
                return RedirectToAction("Index", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnVoltarGrupo()
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                using (GrupoModel model = new GrupoModel())
                {
                    model.VoltarGrupo(iduser, idgrupo);
                }
                return RedirectToAction("Index", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnDeleteMsgUser()
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                int idmsg = int.Parse(Request.QueryString[2]);
                using (MensagemModel model = new MensagemModel())
                {
                    model.DeleteMsgUser(iduser, idgrupo, idmsg);
                }
                return RedirectToAction("Index", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnAddMod()
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                using (GrupoModel model = new GrupoModel())
                {
                    model.AddMod(idgrupo, iduser);
                }
                return RedirectToAction("Members", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnBanUser()
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                using (GrupoModel model = new GrupoModel())
                {
                    model.BanUser(idgrupo, iduser);
                }
                return RedirectToAction("Members", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult BtnEditAboutGrupo(FormCollection form)
        {
            try
            {
                int idgrupo = int.Parse(Request.QueryString[0]);
                int iduser = int.Parse(Request.QueryString[1]);
                string desc = form["descricao"];
                using (GrupoModel model = new GrupoModel())
                {
                    model.EditarGrupo(idgrupo, iduser, desc);
                    TempData["SucessoAbout"] = "Group about updated successfully!";
                }
                return RedirectToAction("Index", "Grupo", new { GrupoID = idgrupo });
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught", ex);
                return RedirectToAction("Erro404", "Error");
            }
        }
    }
}
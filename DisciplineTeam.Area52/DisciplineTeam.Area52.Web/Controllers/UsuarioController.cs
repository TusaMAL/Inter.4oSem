using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisciplineTeam.Area52.Web.Filtro;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class UsuarioController : Controller
    {
        [UsuarioFiltro]
        // GET: Usuario
        public ActionResult Index()//Testando as informações do usuario pegadas no BD
        {
            int idusuario = ((Usuario)Session["usuario"]).IdPessoa;
            using (UsuarioModel model = new UsuarioModel())
            {
                ViewBag.ReadU = model.ReadU(idusuario);                           //Pega informações do usuario que logou e manda paraa view
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.ReadGrupo = model.ReadGrupo(idusuario);                        //Retorna os grupos em que o usuario está participando
                ViewBag.QuantGruposParticipa = model.QuantGruposParticipa(idusuario);     //Retorna o count de grupos em que o usuario participa
            }
            using (MensagemModel model = new MensagemModel())
            {
                ViewBag.ReadMensagemIndex = model.ReadMensagemIndex(idusuario);     //Exibe no feed as mensagens dos grupos em que o usuario participa TODO: ainda nao sei se mostra de todos que estão no grupo
            }
            return View();
        }
        [UsuarioFiltro]
        //GET: Person
        public ActionResult Person()
        {
            return View();
        }
        //GET: Edit
        [UsuarioFiltro]
        public ActionResult Edit()
        {
            Usuario e = new Usuario();
            using (UsuarioModel model = new UsuarioModel())
            {
                e = model.ReadEditUsuario(((Usuario)Session["usuario"]).IdPessoa);      //Lê os dados do usuario no BD e mostra no Formulário para poder ser editado
            }
            return View(e);
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Edit(Usuario e)
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                model.EditUsuario(e, ((Usuario)Session["usuario"]).IdPessoa);       //Recebe como parametro os dados editados do form e pega o id do usuario da sessão para rodar o update no banco
                ViewBag.SucessoEdit = true;                                         //Usado para exibir mensagem de confirmação na view
            }
            return View(e);
        }
        //GET: Edit
        [UsuarioFiltro]
        public ActionResult EditSecurity()
        {
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult EditSecurity(string Senha, string NewPwd)               /* Recebe como parametros a senha atual e nova OBS: Tive que criar atributos na classe pessoa pq o VS dava pau 
                                                                                    creio que por causa do form no estilo abas mas pra nao mudar coloquei os atributos*/
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                Usuario user = (Usuario)Session["usuario"];
                user.Senha = model.GetSenha(user.IdPessoa);                         //Consulta o banco para pegar senha do usuário
                if (Senha != user.Senha)
                {
                    ViewBag.ChangePwdFail = "This is not your current password";
                }
                else
                {
                    if (NewPwd != Senha)
                    {
                        model.ChangePwd(user.IdPessoa, NewPwd);                     //Se o teste chegar aqui a senha do usuario será trocada pela nova
                        ViewBag.ChangePwdSucess = "Password changed successfull.";
                    }
                    else
                    {
                        ViewBag.ChangePwdFail = "This is your current password, for changing please enter a different one.";
                    }
                }
            }
            return View();
        }
        [UsuarioFiltro]
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
                Pessoa user = model.Read(e.Email, e.Senha);
                /*Retorna mensagem de erro caso as informações estejam diferentes no banco pois vai retornar um objeto vazio*/
                if (user == null)
                {
                    ViewBag.Erro = "Wrong login data!";
                }
                else
                {
                    if (user is Usuario)
                    {
                        /*Cria a sessão do usuario e redireciona para a pagina do profile*/
                        ViewBag.User = user;
                        Session["usuario"] = user;
                        return RedirectToAction("Index", "Usuario");
                    }
                    else if (user is Admin)
                    {
                        /* Cria a sessão do admin e redireciona para a pagina de criação de jogos*/
                        Session["usuario"] = user;
                        return RedirectToAction("Index", "Jogo");
                    }
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
                    if (model.Check(e))                             //Checa se o email não está em uso
                    {
                        TempData["Sucesso"] = "true";               /* faz com que o conteudo não seja nulo para que seja exibido mensagem de confirmação na pagina login*/
                        model.Create(e);                            //Cria a conta do usuario
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Erro = "Email already in use";
                    }
                }
            }
            return View(e);
        }
        [UsuarioFiltro]
        public ActionResult Logout()                                //Action para fazer logout
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
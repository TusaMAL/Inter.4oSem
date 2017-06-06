using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisciplineTeam.Area52.Web.Filtro;
using System.Web.Hosting;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class UsuarioController : Controller
    {
        [UsuarioFiltro]
        // GET: Usuario
        public ActionResult Index() //Testando as informações do usuario pegadas no BD
        {
            int iduser = ((Usuario)Session["usuario"]).IdPessoa;
            int quant;
            if (Request.QueryString.Keys.Count > 0)
            {
                quant = int.Parse(Request.QueryString[0]);
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
            using (UsuarioModel model = new UsuarioModel())
            {
                ViewBag.ReadU = model.ReadU(iduser);                                 //Pega informações do usuario que logou e manda paraa view
                ViewBag.GetAgeUser = model.GetAgeUser(iduser);
            }
            using (GrupoModel model = new GrupoModel())
            {
                ViewBag.ReadGrupo = model.ReadGrupo(iduser);                         //Retorna os grupos em que o usuario está participando
                ViewBag.QuantGruposParticipa = model.QuantGruposParticipa(iduser);   //Retorna o count de grupos em que o usuario participa
            }
            using (MensagemModel model = new MensagemModel())
            {
                ViewBag.ReadMensagemIndex = model.ReadMensagemIndex(iduser, quant);         //Exibe no feed as mensagens dos grupos em que o usuario participa TODO: ainda nao sei se mostra de todos que estão no grupo
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
            e.IdPessoa = ((Usuario)Session["usuario"]).IdPessoa;
            using (UsuarioModel model = new UsuarioModel())
            {
                model.EditUsuario(e);       //Recebe como parametro os dados editados do form e pega o id do usuario da sessão para rodar o update no banco
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
        public ActionResult EditPicture()
        {
            using (UsuarioModel model = new UsuarioModel())
            {
                ViewBag.ReadU = model.ReadU(((Usuario)Session["usuario"]).IdPessoa);                                 //Pega informações do usuario que logou e manda paraa view
            }
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult EditPicture(Usuario e)
        {
            
            int iduser = ((Usuario)Session["usuario"]).IdPessoa;
            HttpPostedFileBase arquivo = Request.Files[0];                                          //Recebe o primeiro parametro de arquivo

            using (System.Drawing.Image pic = System.Drawing.Image.FromStream(arquivo.InputStream)) //Converte a arquivo para imagem para poder comparar a as dimensões
            {
                if (pic.Height != 256 && pic.Width != 256)
                {
                    TempData["ErroDimensao"] = "Please use a picture with 256x256 pixels.";         //Semelhante a viewbag porem ela "vive" fora da pagina que foi criada
                    return RedirectToAction("EditPicture");
                }
                else if (arquivo.ContentType != "image/png" && arquivo.ContentType != "image/jpeg" && arquivo.ContentType != "image/jpg")           //Verifica o formato do arquivo
                {
                    TempData["ErroFormato"] = "Application only supports PNG or JPG image types.";
                    return RedirectToAction("EditPicture");
                }
                else if (arquivo.ContentLength > 2097152)                                              //Verifica se o arquivo não é > que 2 MiB
                {
                    TempData["ErroTamanho"] = "Please upload picture with less than 2MiB.";
                    return RedirectToAction("EditPicture");
                }
            }
            DateTime today = DateTime.Now;                                                          //cria uma variavel da hora atual

            string nome = today.ToString("yyyyMMddhhmmss");                                         //converte a hora e data atual para ser usado como nome da imagem do perfil
            if (Request.Files.Count > 0)                                                            // Verifica se recebe algum arquivo
            {
                if (arquivo.ContentLength > 0)                                                      //verifica se ele possui algo
                {
                    //arquivo.FileName pegar nome arquivo
                    //string caminho = "C:/Users/Felipe/Pictures/testebd/" + arquivo.FileName;    //Uso apenas de protótipo
                    //String que vai para o banco, se tiver o caminho todo da imagem o site não mostra
                    string img = "/img/userpics/" + iduser.ToString() + System.IO.Path.GetExtension(arquivo.FileName);
                    string path = HostingEnvironment.ApplicationPhysicalPath;                        //Pega o diretório em que o projeto está
                    //Onde vai ser armazenado
                    string caminho = path + "\\img\\userpics\\" + iduser.ToString() + System.IO.Path.GetExtension(arquivo.FileName);
                    arquivo.SaveAs(caminho);
                    e.Imagem = img;
                    TempData["SucessoImg"] = "Profile picture updated successfully.";
                }
            }
            using (UsuarioModel model = new UsuarioModel())
            {
                model.ChangePicture(e, iduser);
                ViewBag.ReadU = model.ReadU(iduser);                                 //Pega informações do usuario que logou e manda paraa view
            }
            return View();
        }
        [UsuarioFiltro]
        public ActionResult EditDob()
        {
            Usuario e = new Usuario();
            using (UsuarioModel model = new UsuarioModel())
            {
                e = model.ReadEditUsuario(((Usuario)Session["usuario"]).IdPessoa);      //Lê os dados do usuario no BD e mostra no Formulário para poder ser editado
            }
            return View(e);
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
using DisciplineTeam.Area52.Web.Filtro;
using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class JogoController : Controller
    {
        //public static List<Jogo> lista = new List<Jogo>();
        [UsuarioFiltro]
        // GET: Jogos
        public ActionResult Index()
        {
            using (JogoModel model = new JogoModel())
            {
                Admin user = (Admin)Session["usuario"];
                ViewBag.StatusAdmin = user.Status;
                return View(model.ReadJogos());

            } // model.Dispose();
        }
        [UsuarioFiltro]
        public ActionResult Create()
        {
            Admin user = (Admin)Session["usuario"];
            ViewBag.StatusAdmin = user.Status;
            return View();
        }
        [UsuarioFiltro]
        [HttpPost]
        public ActionResult Create(Jogo e)
        {
            Admin user = (Admin)Session["usuario"];
            ViewBag.StatusAdmin = user.Status;
            if (ModelState.IsValid)
            {
                HttpPostedFileBase arquivo = Request.Files[0];

                DateTime today = DateTime.Now;

                string nome = today.ToString("yyyyMMddhhmmss");
                if (Request.Files.Count > 0)    // Verifica se recebe algum arquivo
                {

                    if (arquivo.ContentLength > 0) //verifica se ele possui algo
                    {
                        if (arquivo.ContentType == "application/png" && arquivo.ContentType == "application/jpg")
                        {
                            return RedirectToAction("Index");
                        }
                        //arquivo.FileName pegar nome arquivo
                        //string caminho = "C:/Users/Felipe/Pictures/testebd/" + arquivo.FileName;    //Uso apenas de protótipo
                        string img = "/img/imgjogo/" + nome + System.IO.Path.GetExtension(arquivo.FileName);
                        string caminho = "C:\\Users\\Felipe\\Documents\\GitHub\\Inter.4oSem\\DisciplineTeam.Area52\\DisciplineTeam.Area52.Web\\img\\imgjogo\\" + nome + System.IO.Path.GetExtension(arquivo.FileName);
                        arquivo.SaveAs(caminho);

                        e.Imagem = img;
                    }
                }
                
                using (JogoModel model = new JogoModel())
                {
                    int id = ((Admin)Session["usuario"]).IdPessoa;
                    model.Create(e, id);

                    
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensagem = "Falha no cadastro!";
                return View(e);
            }
        }
    }
}
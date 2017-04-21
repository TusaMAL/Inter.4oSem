﻿using DisciplineTeam.Area52.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisciplineTeam.Area52.Web.Controllers
{
    public class GrupoController : Controller
    {
        // GET: Grupos
        public ActionResult Index()
        {
            return View();
        }
        //GET: Search
        public ActionResult Groups()
        {
            return View();
        }
        //GET: Create
        public ActionResult Create()
        {
            ViewBag.JogoId = new SelectList
                (
                    new JogoModel().Read(),
                    "IdJogo",
                    "Nome",
                    "Imagem"
                );

            return View();
        }
        [HttpPost]
        public ActionResult Create(Grupo e, int jogoid)
        {
            if (ModelState.IsValid)
            {
                using (GrupoModel model = new GrupoModel())
                {
                    ViewBag.JogoId = new SelectList(
                        new JogoModel().Read(),
                        "IdJogo",
                        "Nome",
                        "Imagem",
                        jogoid
                        );
                    int id = ((Usuario)Session["usuario"]).IdPessoa;
                    //Passando o id do criador do grupo como parametro
                    model.Create(e, id, jogoid);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        // GET: Usuario/Friends
        public ActionResult Members()
        {
            return View();
        }
    }
}
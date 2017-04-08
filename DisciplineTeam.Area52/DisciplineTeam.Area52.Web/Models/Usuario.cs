using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Usuario : Pessoa
    {
        [Required(ErrorMessage = "Nick is required")]
        public string Nick { get; set; }

        public string Datanasc { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string Cep { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Usuario : Pessoa
    {
        [Required]
        public string Datanasc { get; set; }

        public string Descricao { get; set; }

        public string Imagem { get; set; }
    }
}
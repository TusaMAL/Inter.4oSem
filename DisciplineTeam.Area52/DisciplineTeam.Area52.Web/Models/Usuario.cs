using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Usuario
    {
        [Required]
        public string datanasc { get; set; }

        public string descricao { get; set; }

        public string imagem { get; set; }
    }
}
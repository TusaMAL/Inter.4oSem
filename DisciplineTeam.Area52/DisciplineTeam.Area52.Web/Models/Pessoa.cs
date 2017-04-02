using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public abstract class Pessoa
    {
        public int IdPessoa { get; set; }

        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public int Status { get; set; }
    }
}
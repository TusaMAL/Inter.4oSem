using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        public string nick { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string senha { get; set; }

        public int status { get; set; }
    }
}
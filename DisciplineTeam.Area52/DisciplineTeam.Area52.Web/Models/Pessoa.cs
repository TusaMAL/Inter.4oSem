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

        [Required(ErrorMessage = "O campo nome é obrigatorio")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo email é obrigatorio")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatorio")]
        [MinLength(6, ErrorMessage = "A senha deve conter no minimo 6 caracteres")]
        public string Senha { get; set; }

        public int Status { get; set; }
    }
}
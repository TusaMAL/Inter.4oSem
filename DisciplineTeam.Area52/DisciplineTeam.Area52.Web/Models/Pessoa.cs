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

        [Required(ErrorMessage = "Name is required")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "The password must have 6 characters")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPwd { get; set; }
        [Required(ErrorMessage = "New Password confirmation is required")]
        public string NewPwdConfirm { get; set; }

        public int Status { get; set; }
    }
}
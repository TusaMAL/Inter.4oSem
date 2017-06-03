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
        public string Sexo { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string Datanasc { get; set; }
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Profile picture is required")]
        public string Imagem { get; set; }
    }
}
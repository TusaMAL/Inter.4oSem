using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public int IdGrupo { get; set; }
        [Required(ErrorMessage = "Event name is required")]
        public string Nome { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string Data { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:MM}")]
        [DataType(DataType.Time)]
        public string Hora { get; set; }
        public int Tipo { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Descricao { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }

    }
}
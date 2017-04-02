using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public int IdGrupo { get; set; }
        public string Nome { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public int Tipo { get; set; }
        public string Endereco { get; set; }
        public string Descricao { get; set; }

    }
}
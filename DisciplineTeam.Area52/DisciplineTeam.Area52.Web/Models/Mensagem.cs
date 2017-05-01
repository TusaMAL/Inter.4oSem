using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Mensagem
    {
        public int IdMensagem { get; set; }
        public int IdUsuario { get; set; }
        public int IdGrupo { get; set; }
        public DateTime Datahora { get; set; }
        public string Texto { get; set; }
        public string NickUsuario { get; set; }
        public string ImagemUsuario { get; set; }
        public string NomeGrupo { get; set; }

    }
}
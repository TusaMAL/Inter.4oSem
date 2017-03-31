using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Participante
    {
        public int IdUsuario { get; set; }
        public int IdGrupo { get; set; }
        public bool Moderador { get; set; }

    }
}
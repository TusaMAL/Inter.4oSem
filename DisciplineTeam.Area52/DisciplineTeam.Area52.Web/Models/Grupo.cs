using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class Grupo
    {
        public int IdGrupo { get; set; }
        public int IdJogo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        
    }
}
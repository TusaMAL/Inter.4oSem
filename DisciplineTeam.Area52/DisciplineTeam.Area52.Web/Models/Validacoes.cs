using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public static class Validacoes
    {
        public static bool VerificarValidadeDoCep(string CEP)
        {
            if (CEP.Trim().Length == 9)
            {
                return System.Text.RegularExpressions.Regex.IsMatch(CEP, ("[0-9]{5}-[0-9]{3}"));
            }
            else if (CEP.Trim().Length == 8)
            {
                CEP = Formatacao.FormatarCEP(CEP);
                return System.Text.RegularExpressions.Regex.IsMatch(CEP, ("[0-9]{5}-[0-9]{3}"));
            }
            else
            {
                return false;
            }
        }
        private static bool verificarProblemasNaRequisicao(string strJSON)
        {
            if (strJSON.Contains("\"erro\": true"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

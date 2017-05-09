using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class MensagemModel : ModelBase
    {
        //Post da mensagem
        public void PostMensagem(Mensagem e, int iduser, int idgrupo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadMsg @texto, @usuario_id, @grupo_id";

            cmd.Parameters.AddWithValue("@texto", e.Texto);
            cmd.Parameters.AddWithValue("@usuario_id", iduser);
            cmd.Parameters.AddWithValue("@grupo_id", idgrupo); 

            cmd.ExecuteNonQuery();
        }
        //Leitura das Mensagens dentro do grupo
        public List<ViewAll> ReadMensagem(int idgrupo)
        {
            List<ViewAll> lista = new List<ViewAll>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT TOP 10 * FROM v_Grupo_Msg WHERE @idgrupo = grupo_id ORDER BY Datahora DESC";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                DateTime data = (DateTime)reader["Datahora"];
                p.MsgDatahora = data.ToString("dd/MM/yyyy, HH:mm");
                p.MsgTexto = (string)(reader["Texto"]);
                p.PIdPessoa = (int)reader["IdUsuario"];
                p.UNick = (string)reader["Nickusuario"];
                //p.ImagemUsuario = (string)reader["Imagemusuario"];
                p.GIdGrupo = (int)reader["Idgrupo"];
                p.GNome = (string)reader["Nomegrupo"];
                lista.Add(p);
            }
            return lista;
        }
        //Retorna as ultimas 10 mensagens postadas ordenadas por data
        public List<ViewAll> ReadMensagemIndex(int iduser)
        {
            List<ViewAll> lista = new List<ViewAll>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT TOP 10 * FROM v_Grupo_Msg_Part WHERE PartIdUser = @iduser AND PartIdGrupo = Idgrupo ORDER BY Datahora DESC";

            cmd.Parameters.AddWithValue("@iduser", iduser);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                DateTime data = (DateTime)reader["Datahora"];
                p.MsgDatahora = data.ToString("dd/MM/yyyy, HH:mm");
                p.MsgTexto = (string)(reader["Texto"]);
                p.PIdPessoa = (int)reader["IdUsuario"];
                p.UNick = (string)reader["Nickusuario"];
                //p.ImagemUsuario = (string)reader["Imagemusuario"];
                p.GIdGrupo = (int)reader["Idgrupo"];
                p.GNome = (string)reader["Nomegrupo"];
                lista.Add(p);
            }
            return lista;
        }
    }
}
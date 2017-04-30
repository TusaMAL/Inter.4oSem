using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class GrupoModel : ModelBase
    {
        public void Create(Grupo e, int id, int idjogo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadGrupo @nome, @descricao, @imagem, @idjogo, @id";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@descricao", e.Descricao);
            cmd.Parameters.AddWithValue("@Imagem", ((object)e.Imagem ?? DBNull.Value)); //Não Vai ficar aqui imagino eu
            cmd.Parameters.AddWithValue("@idjogo", idjogo);
            cmd.Parameters.AddWithValue("@id", id);


            cmd.ExecuteNonQuery();
        }
        public List<Grupo> ReadGrupo(int id)
        {
            List<Grupo> lista = new List<Grupo>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT TOP 6 * FROM v_Grupo_Part WHERE @id = id";
            
            cmd.Parameters.AddWithValue("@id", id);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Grupo p = new Grupo();
                p.IdGrupo = (int)reader["IdGrupo"];
                p.Nome = (string)reader["Nome"];
                p.Imagem = (string)(reader["Imagem"] != DBNull.Value ? reader["Imagem"] : null);
                lista.Add(p);
            }
            return lista;
        }
        //Retorna o Count dos grupos
        public int QuantGruposParticipa(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT COUNT(usuario_id) AS GruposUser FROM Participantes WHERE usuario_id = @id";
            
            cmd.Parameters.AddWithValue("@id", id);

            int quant = (int)cmd.ExecuteScalar();
            return quant;
        }
        public void PostMensagem(Mensagem e, int iduser)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadMsg '2017-04-29 20:19:00', @texto, @usuario_id, @grupo_id";

            //cmd.Parameters.AddWithValue("@datahora", 2017-04-29 20:19:00);
            cmd.Parameters.AddWithValue("@texto", e.Texto);
            cmd.Parameters.AddWithValue("@usuario_id", iduser);
            cmd.Parameters.AddWithValue("@grupo_id", 1); // tem q pegar id do grupo

            cmd.ExecuteNonQuery();
        }
    }
}
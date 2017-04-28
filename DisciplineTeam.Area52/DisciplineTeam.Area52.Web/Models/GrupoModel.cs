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
                p.Nome = (string)reader["Nome"];
                p.Imagem = (string)(reader["Imagem"] != DBNull.Value ? reader["Imagem"] : null);
                lista.Add(p);
            }
            return lista;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class GrupoModel : ModelBase
    {
        public void Create(Grupo e, int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadGrupo @nome, @descricao, @imagem, 2, @id"; //Passando o 2 temporariamente até conseguirmos pegar o id do combobox

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@descricao", e.Descricao);
            cmd.Parameters.AddWithValue("@Imagem", ((object)e.Imagem ?? DBNull.Value)); //Não Vai ficar aqui imagino eu
            cmd.Parameters.AddWithValue("@id", id);


            cmd.ExecuteNonQuery();
        }
        //Ctrl+C + Ctrl+V no metodo do JogoModel, não sei se da pra usar um metodo de outro model
        //Usado pra fazer com que apareça os jogos na View de Grupo/Create dentro do combobox
        public List<Jogo> Read()
        {
            List<Jogo> lista = new List<Jogo>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM v_Jogos ORDER BY nome";
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Jogo p = new Jogo();
                p.Nome = (string)reader["Nome"];
                p.Descricao = (string)reader["Descricao"];
                p.Imagem = (string)(reader["Imagem"] != DBNull.Value ? reader["Imagem"] : null);
                lista.Add(p);
            }

            return lista;
        }
    }
}
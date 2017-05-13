using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class JogoModel : ModelBase
    {
        public void Create(Jogo e, int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"Exec cadJogo @Nome, @Descricao, @Imagem, @id";

            cmd.Parameters.AddWithValue("@Nome", e.Nome);
            cmd.Parameters.AddWithValue("@Descricao", e.Descricao);
            cmd.Parameters.AddWithValue("@Imagem", e.Imagem);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public List<Jogo> ReadJogos()
        {
            List<Jogo> lista = new List<Jogo>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM v_Jogos ORDER BY JogoNome";
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Jogo p = new Jogo();
                    p.IdJogo = (int)reader["JogoId"];
                    p.Nome = (string)reader["JogoNome"];
                    p.Descricao = (string)reader["JogoDesc"];
                    p.Imagem = (string)reader["JogoImg"];
                    lista.Add(p);
                }

            return lista;
        }
        public string ReadJogoImg(int jogoid)
        {
            string img;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM v_Jogos WHERE JogoId = @jogoid";
            //cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@jogoid", jogoid);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                img = (string)reader["JogoImg"];
                return img;
            }
            else
            {
                img = "Erro imagem não encontrada";
                return img;
            }
        }
    }
}
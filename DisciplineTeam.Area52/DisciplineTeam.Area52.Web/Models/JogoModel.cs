using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class JogoModel : ModelBase
    {
        public void Create(Jogo e, int idadm)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"Exec cadJogo @Nome, @Descricao, @Imagem, @id";

            cmd.Parameters.AddWithValue("@Nome", e.Nome);
            cmd.Parameters.AddWithValue("@Descricao", e.Descricao);
            cmd.Parameters.AddWithValue("@Imagem", e.Imagem);
            cmd.Parameters.AddWithValue("@id", idadm);

            cmd.ExecuteNonQuery();
        }
        public Jogo ReadJogoEdit(int idjogo)
        {
            Jogo p = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM v_Jogos WHERE @idjogo = JogoID";
            //cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@IdJogo", idjogo);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                p = new Jogo();
                p.IdJogo = (int)reader["JogoId"];
                p.Nome = (string)reader["JogoNome"];
                p.Descricao = (string)reader["JogoDesc"];
                p.Imagem = (string)reader["JogoImg"];
            }

            return p;
        }
        //Método para editar as informações do jogo
        public void EditJogo(Jogo e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"Exec editJogo @IdJogo, @Nome, @Descricao";

            cmd.Parameters.AddWithValue("@IdJogo", e.IdJogo);
            cmd.Parameters.AddWithValue("@Nome", e.Nome);
            cmd.Parameters.AddWithValue("@Descricao", e.Descricao);

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
        public string ReadJogoImg(int idjogo)
        {
            string img;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM v_Jogos WHERE JogoId = @jogoid";
            //cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@jogoid", idjogo);

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
        //Muda a foto do jogo
        public void ChangeImgJogo(Jogo e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE jogos SET imagem = @newimg WHERE @idjogo = id";

            cmd.Parameters.AddWithValue("@idjogo", e.IdJogo);
            cmd.Parameters.AddWithValue("@newimg", e.Imagem);

            cmd.ExecuteNonQuery();
        }
    }
}
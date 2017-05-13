using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class GrupoModel : ModelBase
    {
        public void Create(Grupo e, int iduser, int idjogo, string img)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadGrupo @nome, @descricao, @imagem, @idjogo, @id";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@descricao", e.Descricao);
            cmd.Parameters.AddWithValue("@Imagem", img);
            cmd.Parameters.AddWithValue("@idjogo", idjogo);
            cmd.Parameters.AddWithValue("@id", iduser);


            cmd.ExecuteNonQuery();
        }
        //Seleciona 6 grupos para exibir no perfil
        public List<Grupo> ReadGrupo(int iduser)
        {
            List<Grupo> lista = new List<Grupo>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TOP 6 * FROM v_Grupo_Part WHERE @iduser = id AND (PartStatus = 1 OR PartStatus = 2)";

            cmd.Parameters.AddWithValue("@iduser", iduser);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Grupo p = new Grupo();
                p.IdGrupo = (int)reader["IdGrupo"];
                p.Nome = (string)reader["Nome"];
                p.Imagem = (string)reader["Imagem"];
                lista.Add(p);
            }
            return lista;
        }
        public List<ViewAll> BuscarGrupo(string busca)
        {
            List<ViewAll> lista = new List<ViewAll>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_Info_Grupo WHERE NomeGrupo LIKE  @busca";
            cmd.Parameters.AddWithValue("@busca", "%" + busca + "%");

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows == false)
            {
                lista = null;
                return lista;
            }
            else
            {
                while (reader.Read())
                {
                    ViewAll p = new ViewAll();
                    p.GIdGrupo = (int)reader["IdGrupo"];
                    p.GNome = (string)reader["NomeGrupo"];
                    p.GImagem = (string)reader["ImagemGrupo"];
                    p.JNome = (string)reader["NomeJogo"];
                    lista.Add(p);
                }
                return lista;
            }

        }
        //Seleciona os grupos do usuario
        public List<ViewAll> ReadGrupoTotal(int id)
        {
            List<ViewAll> lista = new List<ViewAll>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_Grupo_Part WHERE @id = id AND (PartStatus = 1 OR PartStatus = 2)";

            cmd.Parameters.AddWithValue("@id", id);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                p.GIdGrupo = (int)reader["IdGrupo"];
                p.GNome = (string)reader["Nome"];
                p.GImagem = (string)reader["Imagem"];
                p.JNome = (string)reader["NomeJogo"];
                lista.Add(p);

            }
            return lista;
        }
        //Retorna o Count dos grupos
        public int QuantGruposParticipa(int iduser)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT COUNT(usuario_id) AS GruposUser FROM Participantes WHERE usuario_id = @iduser AND (status = 1 OR status = 2)";

            cmd.Parameters.AddWithValue("@iduser", iduser);

            int quant = (int)cmd.ExecuteScalar();
            return quant;
        }
        //Retorna o Count dos usuarios dentro dos grupos
        public int QuantUserGrupos(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT COUNT(usuario_id) AS GruposUser FROM Participantes WHERE grupo_id = @id AND (status = 1 OR status = 2)";

            cmd.Parameters.AddWithValue("@id", id);

            int quant = (int)cmd.ExecuteScalar();
            return quant;
        }
        //Seleciona os 6 participantes do grupo TODO: Precisamos criar uma regra pra selecionar 6 pessoas
        public List<ViewAll> ReadPartGrupo(int idgrupo)
        {
            List<ViewAll> lista = new List<ViewAll>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TOP 6 * FROM v_User_Grupo_Part WHERE PartIdGrupo = @idgrupo AND (PartStatus = 1 OR PartStatus = 2)";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                p.PartIdUsuario = (int)reader["PartIdUser"];
                p.PartIdGrupo = (int)reader["PartIdGrupo"];
                p.UNick = (string)reader["Nick"];
                p.UImagem = (string)reader["Imagem"];
                lista.Add(p);
            }
            return lista;
        }
        //Retorna a quantidade de membros participantes do grupo
        public List<ViewAll> ReadMembrosGrupoTotal(int idgrupo)
        {
            List<ViewAll> lista = new List<ViewAll>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_User_Grupo_Part WHERE PartIdGrupo = @idgrupo AND PartStatus = 1 OR PartStatus = 2";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                p.PartIdUsuario = (int)reader["PartIdUser"];
                p.UNick = (string)reader["Nick"];
                p.UImagem = (string)reader["Imagem"];
                p.PNome = (string)reader["Nome"];
                lista.Add(p);
            }
            return lista;
        }
        //Retorna as Informações do Grupo
        public ViewAll InfoGrupo(int idgrupo)
        {
            ViewAll e = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_Info_Grupo WHERE IdGrupo = @idgrupo";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                e = new ViewAll();
                e.GIdGrupo = (int)reader["IdGrupo"];
                e.GIdJogo = (int)reader["IdJogo"];
                e.GNome = (string)reader["NomeGrupo"];
                e.GImagem = (string)reader["ImagemGrupo"];
                e.GDescricao = (string)reader["Descricao"];
                e.JNome = (string)reader["NomeJogo"];
            }
            return e;
        }
        //Recebe os parametros e insere os dados do usuario
        public void PartGrupo (int iduser, int idgrupo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC partGrupo @idgrupo, @iduser";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@iduser", iduser);
            

            cmd.ExecuteNonQuery();
        }
        //Altera o status do membro para null
        public void SairGrupo(int iduser, int idgrupo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC sairGrupo @idgrupo, @iduser";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@iduser", iduser);


            cmd.ExecuteNonQuery();
        }
        //Altera o status do membro para 1
        public void VoltarGrupo(int iduser, int idgrupo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC voltarGrupo @idgrupo, @iduser";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@iduser", iduser);


            cmd.ExecuteNonQuery();
        }
    }
}
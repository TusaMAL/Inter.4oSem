using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class UsuarioModel : ModelBase
    {
        public void Create(Usuario e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadUser @nome, @email, @senha, @nick";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@email", e.Email);
            cmd.Parameters.AddWithValue("@senha", e.Senha);
            cmd.Parameters.AddWithValue("@nick", e.Nick);
            

            cmd.ExecuteNonQuery();
        }
        public Boolean Check(Usuario e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM Pessoas WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", e.Email);
            SqlDataReader reader = cmd.ExecuteReader();
            /*HasRows verifica se a consulta da tabela retorna linhas, se sim retorna true*/
            if (reader.HasRows == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /* Método para fazer o login no site */
        public Pessoa Read(string email, string senha)
        {
            Pessoa e = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM Pessoas WHERE email = @email AND senha = @senha";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);

            SqlDataReader reader = cmd.ExecuteReader();
            /*Faz a validação dos dados no banco e se coincidirem autentica o usuario*/
            if (reader.Read())
            {
                int status = (int)reader["Status"];
                if (status == 1)
                {
                    /*Insancia objeto usuario*/
                    e = new Usuario();
                    e.IdPessoa = (int)reader["Id"];
                    e.Nome = (string)reader["Nome"];
                    e.Email = (string)reader["Email"];
                    e.Status = (int)reader["Status"];
                   
                }
                if (status == 2)
                {
                    /*Instancia objeto Admin*/
                    e = new Admin();
                    e.IdPessoa = (int)reader["Id"];
                    e.Nome = (string)reader["Nome"];
                    e.Email = (string)reader["Email"];
                    e.Status = (int)reader["Status"];
                }
            }

            return e;
        }

        //Tentando fazer a leitura dos dados do usuario para lançar na pagina do mesmo _\|/_
        public Usuario ReadU(int id)
        {
            Usuario e = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_UserTest WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                e = new Usuario();
                e.IdPessoa = (int)reader["Id"];
                e.Nome = (string)reader["Nome"];
                e.Nick = (string)reader["Nick"];
                //e.Datanasc = (DateTime)(reader["Datanasc"] != DBNull.Value ? reader["Datanasc"] : Convert.ToDateTime((DateTime?)null));
                e.Sexo = (string)(reader["Sexo"]!= DBNull.Value ? reader["Sexo"] : null);
                e.Cidade = (string)(reader["Cidade"] != DBNull.Value ? reader["Cidade"] : null);
                e.Estado = (string)(reader["Estado"] != DBNull.Value ? reader["Estado"] : null);
            }
            return e;
        }
        public Usuario ReadEditUsuario(int idusuario)
        {
            Usuario e = new Usuario();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"select * from v_User_Info WHERE @idusuario = PessoaId";
            cmd.Parameters.AddWithValue("@idusuario", idusuario);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                e.Nome = (string)reader["PessoaNome"];
                e.Nick = (string)reader["Nick"];
                //DateTime data = (DateTime)reader["Datanasc"];
                //e.Datanasc = data.ToString("dd/MM/yyyy");
                e.Sexo = (string)(reader["Sexo"] != DBNull.Value ? reader["Sexo"] : null);
                e.Datanasc = (DateTime)(reader["Datanasc"] != DBNull.Value ? reader["Datanasc"] : Convert.ToDateTime((DateTime?)null));
                //e.Datanasc = data.ToString("dd/MM/yyyy");
                e.Cidade = (string)(reader["Cidade"] != DBNull.Value ? reader["Cidade"] : null);
                e.Estado = (string)(reader["Estado"] != DBNull.Value ? reader["Estado"] : null);
                e.Cep = (string)(reader["Cep"] != DBNull.Value ? reader["Cep"] : null);
                e.Imagem = (string)(reader["Imagem"] != DBNull.Value ? reader["Imagem"] : null);
                e.Descricao = (string)(reader["Descricao"] != DBNull.Value ? reader["Descricao"] : null);
            }
            return e;
        }
        public void EditUsuario(Usuario e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadUser @nome, @email, @senha, @nick";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@email", e.Email);
            cmd.Parameters.AddWithValue("@senha", e.Senha);
            cmd.Parameters.AddWithValue("@nick", e.Nick);


            cmd.ExecuteNonQuery();
        }
    }
}
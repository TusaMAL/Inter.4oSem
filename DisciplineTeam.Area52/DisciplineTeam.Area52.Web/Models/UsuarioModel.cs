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
        public Usuario Read(string email, string senha)
        {
            Usuario e = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM Pessoas WHERE email = @email AND senha = @senha";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);

            SqlDataReader reader = cmd.ExecuteReader();
            /*Faz a validação dos dados no banco e se coincidirem autentica o usuario*/
            if (reader.Read())
            {
                e = new Usuario();
                e.IdPessoa = (int)reader["Id"];
                e.Nome = (string)reader["Nome"];
                e.Email = (string)reader["Email"];
                e.Status = (int)reader["Status"];
            }

            return e;
        }
    }
}
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
        public Usuario Read(string email, string senha)
        {
            Usuario e = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM Usuarios WHERE Email = @email AND Senha = @senha";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@senha", senha);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                e = new Usuario();
                e.IdPessoa = (int)reader["IdPessoa"];
                e.Nome = (string)reader["Nome"];
                e.Email = (string)reader["Email"];
            }

            return e;
        }
    }
}
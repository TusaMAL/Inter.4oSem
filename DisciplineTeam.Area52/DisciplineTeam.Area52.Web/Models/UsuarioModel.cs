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
            cmd.CommandText = @"EXEC cadUser @nome, @email, @senha, @nick, @datanasc";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@email", e.Email);
            cmd.Parameters.AddWithValue("@senha", e.Senha);
            cmd.Parameters.AddWithValue("@nick", e.Nick);
            DateTime data = Convert.ToDateTime(e.Datanasc);
            cmd.Parameters.AddWithValue("@datanasc", data);



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
        //Tentando fazer a leitura dos dados do usuario para lançar na pagina do mesmo
        public Usuario ReadU(int id)
        {
            Usuario e = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_User_Info WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                e = new Usuario();
                e.IdPessoa = (int)reader["PessoaId"];
                e.Nome = (string)reader["Nome"];
                e.Nick = (string)reader["Nick"];
                DateTime data = (DateTime)reader["Datanasc"];
                e.Datanasc = data.ToString("dd/MM/yyyy");
                e.Sexo = (string)(reader["Sexo"]!= DBNull.Value ? reader["Sexo"] : null);
                e.Descricao = (string)(reader["Descricao"] != DBNull.Value ? reader["Descricao"] : null);
                e.Imagem = (string)reader["Imagem"];
            }
            return e;
        }
        //Faz a leitura dos dados do usuario para exibir na tela Settings
        public Usuario ReadEditUsuario(int idusuario)
        {
            Usuario e = new Usuario();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"select * from v_User_Info_Edit WHERE @idusuario = PessoaId";
            cmd.Parameters.AddWithValue("@idusuario", idusuario);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                e.Nome = (string)reader["PessoaNome"];
                e.Nick = (string)reader["Nick"];
                DateTime data = (DateTime)reader["Datanasc"];
                e.Datanasc = data.ToString("dd/MM/yyyy");
                e.Sexo = (string)(reader["Sexo"] != DBNull.Value ? reader["Sexo"] : null);
                //e.Datanasc = (DateTime)(reader["Datanasc"] != DBNull.Value ? reader["Datanasc"] : Convert.ToDateTime((DateTime?)null));
                //e.Datanasc = data.ToString("dd/MM/yyyy");
                e.Imagem = (string)reader["Imagem"];
                e.Descricao = (string)(reader["Descricao"] != DBNull.Value ? reader["Descricao"] : null);
            }
            return e;
        }
        //Editar as informações ja existentes do usuário
        public void EditUsuario(Usuario e, int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC editInfo_User @IdUsuario, @nome, @nick, @sexo, @datanasc, @descricao";
           
            cmd.Parameters.AddWithValue("@IdUsuario", id);
            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@nick", e.Nick);
            e.Sexo = (e.Sexo != null ? e.Sexo : "");
            cmd.Parameters.AddWithValue("@sexo", e.Sexo);
            DateTime date = Convert.ToDateTime(e.Datanasc);
            cmd.Parameters.AddWithValue("@datanasc", date);
            e.Descricao = (e.Descricao != null ? e.Descricao : "");
            cmd.Parameters.AddWithValue("@descricao", e.Descricao);

            cmd.ExecuteNonQuery();
        }
        //Mudar senha usuario
        public void ChangePwd(int iduser, string newpwd)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC editPwd @IdUsuario, @newpwd";

            cmd.Parameters.AddWithValue("@IdUsuario", iduser);
            cmd.Parameters.AddWithValue("@newpwd", newpwd);
            
            cmd.ExecuteNonQuery();
        }
        //Método para pegar senha do usuario do banco pra poder comparar na hora de muda-la
        public string GetSenha(int iduser)
        {
            Usuario e = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM pessoas WHERE @iduser = pessoas.id";

            cmd.Parameters.AddWithValue("@iduser", iduser);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                e = new Usuario();
                e.Senha = (string)reader["senha"];
            }
            return e.Senha;
        }
        public void ChangePicture(Usuario e, int iduser)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE usuarios SET imagem = @img WHERE @iduser = pessoa_id";

            cmd.Parameters.AddWithValue("@iduser", iduser);
            cmd.Parameters.AddWithValue("@img", e.Imagem);

            cmd.ExecuteNonQuery();
        }
        public int GetAgeUser(int iduser)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT DATEDIFF(month, datanasc, GETDATE()) / 12 AnoNasc FROM usuarios WHERE pessoa_id = @iduser";

            cmd.Parameters.AddWithValue("@iduser", iduser);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                iduser = (int)reader["AnoNasc"]; 
            }
            else
            {
                iduser = 200;
            }
            return iduser;

        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public class EventoModel : ModelBase
    {
        //Cria o evento
        public void Create(Evento e, int idgrupo)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC cadEvento @nome, @data, @horario, @tipo, @cep, @numero, @logradouro, @bairro, @cidade, @uf, @descricao, @grupo_id";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@data", e.Data);
            cmd.Parameters.AddWithValue("@horario", e.Hora);
            cmd.Parameters.AddWithValue("@tipo", e.Tipo);
            e.Cep = (e.Cep != null ? e.Cep : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@cep", e.Cep);
            e.Numero = (e.Numero != null ? e.Numero : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@numero", e.Numero);
            e.Logradouro = (e.Logradouro != null ? e.Logradouro : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@logradouro", e.Logradouro);
            e.Bairro = (e.Bairro != null ? e.Bairro : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@bairro", e.Bairro);
            e.Localidade = (e.Localidade != null ? e.Localidade : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@cidade", e.Localidade);
            e.Uf = (e.Uf != null ? e.Uf : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@uf", e.Uf);
            cmd.Parameters.AddWithValue("@descricao", e.Descricao);
            cmd.Parameters.AddWithValue("@grupo_id", idgrupo);

            cmd.ExecuteNonQuery();
        }
        //Lê os dados do evento e mostra na index do mesmo
        public Evento ReadEvento(int idevento, int idgrupo)
        {
            Evento p = null;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_Event_Index WHERE IdGrupo = EventoIdGrupo AND EventoIdGrupo = @idgrupo AND IdEvento = @idevento"; //TODO: Achar uma regra pra selecionar os eventos que estão mais proximos

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@idevento", idevento);
            //cmd.CommandType = System.Data.CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                p = new Evento();
                p.IdGrupo = (int)reader["IdGrupo"];
                p.IdEvento = (int)reader["IdEvento"];
                p.Nome = (string)reader["NomeEvento"];
                DateTime data = (DateTime)reader["DataEvento"];
                p.Data = data.ToString(@"yyyy-MM-dd");
                TimeSpan hora = (TimeSpan)reader["HoraEvento"];
                p.Hora = hora.ToString(@"hh\:mm");
                p.Tipo = (int)reader["TipoEvento"];
                p.Descricao = (string)reader["DescricaoEvento"];
                p.Cep = (string)(reader["CepEvento"] != DBNull.Value ? reader["CepEvento"] : null);
                p.Numero = (string)(reader["NrEvento"] != DBNull.Value ? reader["NrEvento"] : null);
                p.Logradouro = (string)(reader["LogEvento"] != DBNull.Value ? reader["LogEvento"] : null);
                p.Bairro = (string)(reader["BairroEvento"] != DBNull.Value ? reader["BairroEvento"] : null);
                p.Localidade = (string)(reader["CidadeEvento"] != DBNull.Value ? reader["CidadeEvento"] : null);
                p.Uf = (string)(reader["UfEvento"] != DBNull.Value ? reader["UfEvento"] : null);

            }    
                return p;
        }
        //Retorna lista de eventos do grupo
        public List<Evento> ViewEventos(int idgrupo)
        {
            List<Evento> lista = new List<Evento>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT DATEDIFF(n, GETDATE(), DataEvento), * FROM v_Event_Grupo WHERE 0 <= DATEDIFF(n, GETDATE(), DataEvento) AND IdGrupo = EventoIdGrupo AND EventoIdGrupo = @idgrupo ORDER BY DataEvento ASC";
            
            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            //cmd.CommandType = System.Data.CommandType.Text;

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
                    Evento p = new Evento();
                    p.IdGrupo = (int)reader["IdGrupo"];
                    p.IdEvento = (int)reader["IdEvento"];
                    p.Nome = (string)reader["NomeEvento"];
                    DateTime data = (DateTime)reader["DataEvento"];
                    p.Data = data.ToString(@"dd-MM-yyyy");
                    TimeSpan hora = (TimeSpan)reader["HoraEvento"];
                    p.Hora = hora.ToString(@"hh\:mm");
                    p.Tipo = (int)reader["TipoEvento"];
                    lista.Add(p);
                }

                return lista;
            }
        }
        //Retorna lista de eventos do grupo
        public List<Evento> ViewEventosIndex(int idgrupo)
        {
            List<Evento> lista = new List<Evento>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TOP 6 DATEDIFF(n, GETDATE(), DataEvento), * FROM v_Event_Grupo WHERE DATEDIFF(n, GETDATE(), DataEvento) <= 86400 AND DATEDIFF(n, GETDATE(), DataEvento) > 0 AND IdGrupo = EventoIdGrupo AND EventoIdGrupo = @idgrupo ORDER BY DataEvento ASC";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            //cmd.CommandType = System.Data.CommandType.Text;

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
                    Evento p = new Evento();
                    p.IdGrupo = (int)reader["IdGrupo"];
                    p.IdEvento = (int)reader["IdEvento"];
                    p.Nome = (string)reader["NomeEvento"];
                    DateTime data = (DateTime)reader["DataEvento"];
                    p.Data = data.ToString(@"dd-MM-yyyy");
                    TimeSpan hora = (TimeSpan)reader["HoraEvento"];
                    p.Hora = hora.ToString(@"hh\:mm");
                    p.Tipo = (int)reader["TipoEvento"];
                    lista.Add(p);
                }

                return lista;
            }
        }
        //Faz o insert dos dados do usuário no banco
        public void PartEvento(int idgrupo, int iduser, int idevento)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC partEvento @grupo_id, @usuario_id, @evento_id";

            cmd.Parameters.AddWithValue("@grupo_id", idgrupo);
            cmd.Parameters.AddWithValue("@usuario_id", iduser);
            cmd.Parameters.AddWithValue("@evento_id", idevento);

            cmd.ExecuteNonQuery();
        }
        //Faz o update do status do usuario no evento para 0
        public void SairEvento(int idgrupo, int iduser, int idevento)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC sairEvento @grupo_id, @usuario_id, @evento_id";

            cmd.Parameters.AddWithValue("@grupo_id", idgrupo);
            cmd.Parameters.AddWithValue("@usuario_id", iduser);
            cmd.Parameters.AddWithValue("@evento_id", idevento);

            cmd.ExecuteNonQuery();
        }
        //Faz o update do status do usuario no evento para 1
        public void PartEventoUpdate(int idgrupo, int iduser, int idevento)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC partEventoUpdate @grupo_id, @usuario_id, @evento_id";

            cmd.Parameters.AddWithValue("@grupo_id", idgrupo);
            cmd.Parameters.AddWithValue("@usuario_id", iduser);
            cmd.Parameters.AddWithValue("@evento_id", idevento);

            cmd.ExecuteNonQuery();
        }
        //Mostra os usuarios que tem presença confirmada no evento
        public List<ViewAll> ViewConfUserEvento(int idgrupo, int idevento)
        {
            List<ViewAll> lista = new List<ViewAll>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TOP 6 * FROM v_Conf_Evento_Grupo WHERE ConfGrupoId = @idgrupo AND ConfEventoId = @idevento AND ConfStatus = 1";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@idevento", idevento);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                p.CIdGrupo = (int)reader["ConfGrupoId"];
                p.CIdUsuario = (int)reader["ConfUserId"];
                p.CIdEvento = (int)reader["ConfEventoId"];
                p.UNick = (string)reader["UserNick"];
                p.UImagem = (string)(reader["UserImg"] != DBNull.Value ? reader["UserImg"] : null);
                lista.Add(p);
            }
            return lista;
        }
        //Mostra os usuarios que tem presença confirmada no evento
        public List<ViewAll> ViewConfUserEventoAll(int idgrupo, int idevento)
        {
            List<ViewAll> lista = new List<ViewAll>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_Conf_Evento_Grupo WHERE ConfGrupoId = @idgrupo AND ConfEventoId = @idevento AND ConfStatus = 1";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@idevento", idevento);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewAll p = new ViewAll();
                p.CIdGrupo = (int)reader["ConfGrupoId"];
                p.CIdUsuario = (int)reader["ConfUserId"];
                p.CIdEvento = (int)reader["ConfEventoId"];
                p.UNick = (string)reader["UserNick"];
                p.UImagem = (string)(reader["UserImg"] != DBNull.Value ? reader["UserImg"] : null);
                lista.Add(p);
            }
            return lista;
        }
        //Retorna o Count dos usuarios que vão para o evento
        public int QuantUserPartEvento(int idgrupo, int idevento)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT COUNT(usuario_id) AS ParticipantesEvento FROM confirmados c WHERE c.grupo_id = @idgrupo AND c.evento_id = @idevento AND c.status = 1";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@idevento", idevento);

            int quant = (int)cmd.ExecuteScalar();
            return quant;
        }
        //Retorna o status do usuário para mostrar os botões na view do evento
        public int UserStatusEvento(int idgrupo, int iduser, int idevento)
        {
            int status = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM v_Conf_Evento_Grupo WHERE ConfGrupoId = @idgrupo AND ConfUserId = @iduser AND ConfEventoId = @idevento";

            cmd.Parameters.AddWithValue("@idgrupo", idgrupo);
            cmd.Parameters.AddWithValue("@iduser", iduser);
            cmd.Parameters.AddWithValue("@idevento", idevento);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows == false)
            {
                status = 3;
                return status;
            }
            else if (reader.Read())
            {
                status = (int)reader["ConfStatus"];
            }
            return status;
        }
        //Edita as informações do evento
        public void EditInfoEvento(Evento e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC editarEvento @grupo_id, @id , @nome, @data, @horario, @tipo, @cep, @numero, @logradouro, @bairro, @cidade,@uf, @descricao";


            cmd.Parameters.AddWithValue("@grupo_id", e.IdGrupo);
            cmd.Parameters.AddWithValue("@id", e.IdEvento);
            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@data", e.Data);
            cmd.Parameters.AddWithValue("@horario", e.Hora);
            cmd.Parameters.AddWithValue("@tipo", e.Tipo);
            e.Cep = (e.Cep != null ? e.Cep : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@cep", e.Cep);
            e.Numero = (e.Numero != null ? e.Numero : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@numero", e.Numero);
            e.Logradouro = (e.Logradouro != null ? e.Logradouro : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@logradouro", e.Logradouro);
            e.Bairro = (e.Bairro != null ? e.Bairro : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@bairro", e.Bairro);
            e.Localidade = (e.Localidade != null ? e.Localidade : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@cidade", e.Localidade);
            e.Uf = (e.Uf != null ? e.Uf : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@uf", e.Uf);
            cmd.Parameters.AddWithValue("@descricao", e.Descricao);

            cmd.ExecuteNonQuery();
        }
    }
}
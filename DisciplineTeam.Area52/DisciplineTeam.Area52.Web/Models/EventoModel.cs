using System;
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
            cmd.CommandText = @"EXEC cadEvento @nome, @data, @horario, @tipo, @endereco, @descricao, @grupo_id";

            cmd.Parameters.AddWithValue("@nome", e.Nome);
            cmd.Parameters.AddWithValue("@data", e.Data);
            cmd.Parameters.AddWithValue("@horario", e.Hora);
            cmd.Parameters.AddWithValue("@tipo", e.Tipo);
            e.Endereco = (e.Endereco != null ? e.Endereco : ""); //Se receber valor nulo, insere no banco valor nulo
            cmd.Parameters.AddWithValue("@endereco", e.Endereco);
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
                p.Data = data.ToString("dd/MM/yyyy");
                TimeSpan hora = (TimeSpan)reader["HoraEvento"];
                p.Hora = hora.ToString(@"hh\:mm");
                p.Tipo = (int)reader["TipoEvento"];
                p.Descricao = (string)reader["DescricaoEvento"];
                p.Endereco = (string)(reader["EndEvento"] != DBNull.Value ? reader["EndEvento"] : null);
            }    
                return p;
        }
        //Retorna lista de eventos do grupo
        public List<Evento> ViewEventos(int idgrupo)
        {
            List<Evento> lista = new List<Evento>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TOP 6 * FROM v_Event_Grupo WHERE IdGrupo = EventoIdGrupo AND EventoIdGrupo = @idgrupo ORDER BY DataEvento DESC"; //TODO: Achar uma regra pra selecionar os eventos que estão mais proximos

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
                    p.Data = data.ToString("dd/MM/yyyy");
                    TimeSpan hora = (TimeSpan)reader["HoraEvento"];
                    p.Hora = hora.ToString(@"hh\:mm");
                    p.Tipo = (int)reader["TipoEvento"];
                    lista.Add(p);
                }

                return lista;
            }
        }
        //Faz o insert dos dados do usuário no banco
        public void PartEvento(int grupo_id, int usuario_id, int evento_id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC partEvento @grupo_id, @usuario_id, @evento_id";

            cmd.Parameters.AddWithValue("@grupo_id", grupo_id);
            cmd.Parameters.AddWithValue("@usuario_id", usuario_id);
            cmd.Parameters.AddWithValue("@evento_id", evento_id);

            cmd.ExecuteNonQuery();
        }
        //Faz o update do status do usuario no evento para 2
        public void SairEvento(int grupo_id, int usuario_id, int evento_id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC sairEvento @grupo_id, @usuario_id, @evento_id";

            cmd.Parameters.AddWithValue("@grupo_id", grupo_id);
            cmd.Parameters.AddWithValue("@usuario_id", usuario_id);
            cmd.Parameters.AddWithValue("@evento_id", evento_id);

            cmd.ExecuteNonQuery();
        }
        //Faz o update do status do usuario no evento para 1
        public void PartEventoUpdate(int grupo_id, int usuario_id, int evento_id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"EXEC partEventoUpdate @grupo_id, @usuario_id, @evento_id";

            cmd.Parameters.AddWithValue("@grupo_id", grupo_id);
            cmd.Parameters.AddWithValue("@usuario_id", usuario_id);
            cmd.Parameters.AddWithValue("@evento_id", evento_id);

            cmd.ExecuteNonQuery();
        }
        //Mostra os usuarios que tem presença confirmada no evento
        public List<ViewAll> ViewConfUserEvento(int idgrupo, int idevento)
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
    }
}
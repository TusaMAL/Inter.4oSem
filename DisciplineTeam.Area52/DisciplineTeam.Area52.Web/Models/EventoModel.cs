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
            cmd.CommandText = @"SELECT * FROM v_Event_Index WHERE EventoIdGrupo = @idgrupo AND IdEvento = @idevento"; //TODO: Achar uma regra pra selecionar os eventos que estão mais proximos

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
            cmd.CommandText = @"SELECT TOP 6 * FROM v_Event_Grupo WHERE EventoIdGrupo = @idgrupo ORDER BY DataEvento DESC"; //TODO: Achar uma regra pra selecionar os eventos que estão mais proximos

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
    }
}
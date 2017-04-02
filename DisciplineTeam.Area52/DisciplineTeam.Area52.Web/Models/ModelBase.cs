using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DisciplineTeam.Area52.Web.Models
{
    public abstract class ModelBase : IDisposable
    {
        protected SqlConnection connection;
        
        public ModelBase()
        {
            string strConn = "Data Source = localhost; " +
                " Initial Catalog = BDarea52;" +
                " Integrated Security = true";
            //"User Id = sa; Password = dba";

            connection = new SqlConnection(strConn);

            connection.Open();
        }
        public void Dispose()
        {
            connection.Close();
        }
    }
}
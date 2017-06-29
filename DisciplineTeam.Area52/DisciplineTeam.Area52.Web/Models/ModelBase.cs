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
            //string strConn = "Data Source = localhost;" +
            //    " Initial Catalog = BDarea52;" +
            //    " Integrated Security = true;" +
            //      "User Id = sa; Password = dba";

            string strConn = "Server=tcp:area52.database.windows.net,1433;Initial Catalog=BDarea52;Persist Security Info=False;User ID=felipe;Password=m9p13ku2jM;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            connection = new SqlConnection(strConn);

            connection.Open();
        }
        public void Dispose()
        {
            connection.Close();
        }
    }
}
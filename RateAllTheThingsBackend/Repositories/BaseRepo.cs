using System.Configuration;
using System.Data.SqlClient;

namespace RateAllTheThingsBackend.Repositories
{
    public abstract class BaseRepo
    {
        protected SqlConnection Connection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]);
            }
        }
    }
}
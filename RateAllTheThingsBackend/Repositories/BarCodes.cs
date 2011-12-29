using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class BarCodes : IBarCodes
    {
        public IEnumerable<BarCode> All()
        {
            IEnumerable<BarCode> barCodes = Enumerable.Empty<BarCode>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                connection.Open();
                barCodes = connection.Query<BarCode>("SELECT * FROM BarCodes");   
            }

            return barCodes;
        }

        public BarCode Get(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]))
            {
                connection.Open();
                var barcode = connection.Query<BarCode>("SELECT TOP 1 * FROM BarCodes WHERE ID = @ID", new {ID = id}).Single();
                return barcode;
            }
        }
    }
}
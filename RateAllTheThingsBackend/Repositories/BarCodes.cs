using System;
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
            using (SqlConnection connection = Connection())
            {
                connection.Open();
                barCodes = connection.Query<BarCode>("SELECT * FROM BarCodes");   
            }

            return barCodes;
        }

        private static SqlConnection Connection()
        {
            return new SqlConnection(ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"]);
        }

        public BarCode Get(long id)
        {
            using (SqlConnection connection = Connection())
            {
                connection.Open();
                var barcode = connection.Query<BarCode>("SELECT TOP 1 * FROM BarCodes WHERE ID = @ID", new {ID = id}).Single();
                return barcode;
            }
        }

        public BarCode Get(string format, string code)
        {
            using (SqlConnection connection = Connection())
            {
                connection.Open();
                var barcodes = connection.Query<BarCode>("SELECT TOP 1 * FROM BarCodes WHERE Format = @FORMAT AND Code = @CODE", new { FORMAT = format, CODE = code }).ToArray();
                if (barcodes.Any())
                    return barcodes.First();
                return null;
            }
        }

        public BarCode Create(string format, string code)
        {
            using (SqlConnection connection = Connection())
            {
                connection.Open();

                var id = connection.Query<decimal>("INSERT INTO BarCodes(Format, Code) values(@FORMAT, @CODE); SELECT SCOPE_IDENTITY();", new { FORMAT = format, CODE = code }).Single();
                return this.Get((Int64)id);
            }
        }

        public BarCode Update(BarCode barCode)
        {
            using (SqlConnection connection = Connection())
            {
                connection.Open();

                connection.Execute("UPDATE BarCodes set Name = @NAME where Id = @ID", new {ID =barCode.Id, NAME = barCode.Name} );
                return this.Get(barCode.Id);
            }
        }
    }
}
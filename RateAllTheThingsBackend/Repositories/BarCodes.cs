using System.Collections.Generic;
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
            using (SqlConnection connection = new SqlConnection())
            {
                barCodes = connection.Query<BarCode>("SELECT * FROM BarCode");   
            }

            return barCodes;
        }

        public BarCode Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
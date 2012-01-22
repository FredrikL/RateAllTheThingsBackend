using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class BarCodes : BaseRepo, IBarCodes
    {
        public IEnumerable<BarCode> All()
        {
            IEnumerable<BarCode> barCodes = Enumerable.Empty<BarCode>();
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                barCodes = connection.Query<BarCode>("SELECT * FROM BarCodes");   
            }

            return barCodes;
        }

        public BarCode Get(long id)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                var barcode = connection.Query<BarCode>("SELECT TOP 1 * FROM BarCodes WHERE ID = @ID", new {ID = id}).Single();
                return barcode;
            }
        }

        public BarCode Get(string format, string code)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                var barcodes = connection.Query<BarCode>("SELECT TOP 1 * FROM BarCodes WHERE Format = @FORMAT AND Code = @CODE", new { FORMAT = format, CODE = code }).ToArray();
                if (barcodes.Any())
                    return barcodes.First();
                return null;
            }
        }

        public BarCode Create(string format, string code, long createdBy)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                var id = connection.Query<decimal>("INSERT INTO BarCodes(Format, Code) values(@FORMAT, @CODE); SELECT SCOPE_IDENTITY();", new { FORMAT = format, CODE = code }).Single();
                return this.Get((Int64)id);
            }
        }

        public BarCode Update(BarCode barCode, long updatedBy)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();

                connection.Execute("UPDATE BarCodes set Name = @NAME where Id = @ID", new {ID =barCode.Id, NAME = barCode.Name} );
                return this.Get(barCode.Id);
            }
        }

        public bool Exists(long barcodeId)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                return connection.Query<BarCode>("SELECT TOP 1 * FROM BarCodes WHERE ID = @ID", new { ID = barcodeId }).Any();
            }
        }

        public void Rate(long barCodeId, byte rating, long userid)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                connection.Execute("INSERT INTO Rating(BarCodeID, Author, Rating) VALUES(@BARCODEID, @AUTHOR, @RATING)",
                                   new
                                       {
                                           BARCODEID = barCodeId,
                                           AUTHOR = userid,
                                           RATING = rating
                                       });
            }            
        }

        public bool HasRated(long userid, long barCodeId)
        {
            using (SqlConnection connection = Connection)
            {
                connection.Open();
                return connection.Query<int>("SELECT Count(Rating) FROM Rating WHERE BarCodeId = @BARCODEID AND Author = @AUTHOR",
                                                 new
                                                     {
                                                         BARCODEID = barCodeId,
                                                         AUTHOR = userid
                                                     }).Any();
            }
        }
    }
}
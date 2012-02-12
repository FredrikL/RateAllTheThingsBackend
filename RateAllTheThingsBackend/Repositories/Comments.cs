using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class Comments : BaseRepo, IComments
    {
        public void Add(Comment comment)
        {                        
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                conn.Execute("insert into Comments(Text, Author, BarCodeId) values(@TEXT, @AUTHOR, @BARCODEID)",
                    new
                        {
                            TEXT = comment.Text, 
                            AUTHOR = comment.Author, 
                            BARCODEID =comment.BarCodeId
                        });
            }
        }

        public void Delete(long commentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetCommentsForBarCode(Int64 barCodeId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                return conn.Query<Comment>("SELECT C.*, U.Alias as AuthorName FROM Comments C INNER JOIN Users U on C.Author = U.Id WHERE C.BarCodeId = @BARCODEID", new {BARCODEID = barCodeId});
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public class Comments : BaseRepo, IComments
    {
        public void Add(long barCodeId, Comment comment)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Execute("insert into Comments(Text) values(@TEXT); select scope_identity();", new {TEXT = comment.Text});
            }
        }

        public void Delete(long commentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetCommentsForBarCode(long barCodeId)
        {
            using(SqlConnection conn = Connection)
            {
                return conn.Query<Comment>("SELECT * FROM Comments");
            }
        }
    }
}
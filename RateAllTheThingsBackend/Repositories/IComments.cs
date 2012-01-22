using System;
using System.Collections.Generic;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IComments
    {
        void Add(Comment comment);
        void Delete(Int64 commentId);

        IEnumerable<Comment> GetCommentsForBarCode(Int64 barCodeId);
    }
}
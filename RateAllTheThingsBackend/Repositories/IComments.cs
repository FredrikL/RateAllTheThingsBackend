using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IComments
    {
        Comment Add(long barCodeId, Comment comment);
        void Delete(long commentId);
    }
}
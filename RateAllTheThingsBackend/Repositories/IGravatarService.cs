using System.Collections.Generic;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IGravatarService
    {
        IEnumerable<Comment> AddAvatarToComments(IEnumerable<Comment> comments);
    }
}
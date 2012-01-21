using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class CommentsModule:NancyModule
    {
        private readonly IBarCodes barCodes;
        private readonly IComments comments;
        private readonly IUsers users;

        public CommentsModule(IBarCodes barCodes, IComments a, IUsers users) : base("/Comment")
        {
            this.RequiresAuthentication();

            this.barCodes = barCodes;
            this.comments = a;
            this.users = users;

            Get["/"] = x =>
                           {
                               throw new NotImplementedException();
                           };

            Get["/{id}"] = x =>
                                       {
                                           if (!this.barCodes.Exists(x.id))
                                               return Response.AsJson(Enumerable.Empty<Comment>());
                                           
                                           Comment[] commentsForBarCode = this.comments.GetCommentsForBarCode(x.id).ToArray();
                                           return Response.AsJson(commentsForBarCode);
                                       };

            Post["/{id}"] = x =>
                                        {
                                            if (!this.barCodes.Exists(x.id))
                                                return Response.AsJson(Enumerable.Empty<Comment>());

                                            Comment comment = this.Bind<Comment>();
                                            comment.Author = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);

                                            this.comments.Add(comment);
                                            Comment[] commentsForBarCode = this.comments.GetCommentsForBarCode(x.id).ToArray();
                                            return Response.AsJson(commentsForBarCode);
                                        };
        }
    }
}
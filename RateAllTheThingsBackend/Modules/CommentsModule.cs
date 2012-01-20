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

        public CommentsModule(IBarCodes barCodes, IComments comments, IUsers users) : base("/Comment")
        {
            this.RequiresAuthentication();

            this.barCodes = barCodes;
            this.comments = comments;
            this.users = users;

            Get["/{barcode_id}"] = x =>
                                       {
                                           if (!this.barCodes.Exists(x.barcode_id))
                                               return Response.AsJson(Enumerable.Empty<Comment>());
                                           
                                           Comment[] commentsForBarCode = this.comments.GetCommentsForBarCode(x.barcode_id).ToArray();
                                           return Response.AsJson(commentsForBarCode);
                                       };

            Post["/{barcode_id}"] = x =>
                                        {
                                            if (!this.barCodes.Exists(x.barcode_id))
                                                return Response.AsJson(Enumerable.Empty<Comment>());

                                            Comment comment = this.Bind<Comment>();
                                            comment.Author = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);

                                            this.comments.Add(x.barcode_id, comment);
                                            Comment[] commentsForBarCode = this.comments.GetCommentsForBarCode(x.barcode_id).ToArray();
                                            return Response.AsJson(commentsForBarCode);
                                        };
        }
    }
}
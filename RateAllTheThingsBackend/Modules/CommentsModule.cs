using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class CommentsModule:NancyModule
    {
        private readonly IComments comments;

        public CommentsModule(IComments comments) : base("/Comment")
        {
            this.RequiresAuthentication();

            this.comments = comments;

            Get["/{barcode_id}"] = x =>
                                       {
                                           Comment[] commentsForBarCode = this.comments.GetCommentsForBarCode(x.barcode_id).ToArray();
                                           return Response.AsJson(commentsForBarCode);
                                       };

            Post["/{barcode_id}"] = x =>
                                        {
                                            Comment comment = this.Bind<Comment>();
                                            var added = this.comments.Add(x.barcode_id, comment);
                                            return Response.AsJson(new[] { added });
                                        };
        }
    }
}
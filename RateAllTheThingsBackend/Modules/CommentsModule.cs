using System;
using System.Collections.Generic;
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
        private readonly IEventLog eventLog;
        private readonly IGravatarService gravatarService;

        public CommentsModule(IBarCodes barCodes, IComments comments, 
            IUsers users, IEventLog eventLog, IGravatarService gravatarService) : base("/Comment")
        {
            this.RequiresAuthentication();

            this.barCodes = barCodes;
            this.comments = comments;
            this.users = users;
            this.eventLog = eventLog;
            this.gravatarService = gravatarService;

            Get["/{id}"] = x =>
                               {
                                   if (!this.barCodes.Exists(x.id))
                                       return Response.AsJson(Enumerable.Empty<Comment>());

                                   IEnumerable<Comment> commentsForBarCode = this.comments.GetCommentsForBarCode(x.id);
                                   Comment[] ret =
                                       this.gravatarService.AddAvatarToComments(commentsForBarCode).ToArray();
                                   return Response.AsJson(ret);
                               };

            Post["/{id}"] = x =>
                                {
                                    if (!this.barCodes.Exists(x.id))
                                        return Response.AsJson(Enumerable.Empty<Comment>());

                                    Comment comment = this.Bind<Comment>();
                                    comment.Author = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);

                                    this.comments.Add(comment);
                                    Comment[] commentsForBarCode = this.comments.GetCommentsForBarCode(x.id).ToArray();
                                    this.Log(x.id, comment.Author, "COMMENT", comment.Text);
                                    return Response.AsJson(commentsForBarCode);
                                };
        }

        private void Log(Int64 barCodeId, long userId, string eventName, string data = null)
        {
            this.eventLog.LogEvent(new Event()
            {
                AuthorId = userId,
                BarCodeId = barCodeId,
                Ip = this.Context.Request.UserHostAddress,
                EventName = eventName,
                Data = null
            });
        }
    }
}
using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Core
{
    public class HttpCustomBasicUnauthorizedResult : HttpUnauthorizedResult
    {
        public HttpCustomBasicUnauthorizedResult() : base() { }
        public HttpCustomBasicUnauthorizedResult(string statusDescription) : base(statusDescription) { }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            context.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic");
            base.ExecuteResult(context);
        }
    }

    public class CustomBasicAuthorizeAttribute : AuthorizeAttribute
    {
        bool _RequireSsl = true;
        public bool RequireSsl
        {
            get { return _RequireSsl; }
            set { _RequireSsl = value; }
        }

        private IUsers users;

        public CustomBasicAuthorizeAttribute()
        {
            this.users = TinyIoC.TinyIoCContainer.Current.Resolve<IUsers>();
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            if (!Authenticate(filterContext.HttpContext))
            {
                filterContext.Result = new HttpCustomBasicUnauthorizedResult();
            }
            else
            {
                if (AuthorizeCore(filterContext.HttpContext))
                {
                    HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                    cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                    cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
                }
                else
                {
                    filterContext.Result = new HttpCustomBasicUnauthorizedResult();
                }
            }
        }

        // from here on are private methods to do the grunt work of parsing/verifying the credentials

        private bool Authenticate(HttpContextBase context)
        {
            if (_RequireSsl && !context.Request.IsSecureConnection && !context.Request.IsLocal) return false;

            if (!context.Request.Headers.AllKeys.Contains("Authorization")) return false;

            string authHeader = context.Request.Headers["Authorization"];

            IPrincipal principal;
            if (TryGetPrincipal(authHeader, out principal))
            {
                HttpContext.Current.User = principal;
                return true;
            }
            return false;
        }

        private bool TryGetPrincipal(string authHeader, out IPrincipal principal)
        {
            var creds = ParseAuthHeader(authHeader);
            if (creds != null)
            {
                if (TryGetPrincipal(creds[0], creds[1], out principal)) return true;
            }

            principal = null;
            return false;
        }

        private string[] ParseAuthHeader(string authHeader)
        {
            // Check this is a Basic Auth header
            if (authHeader == null || authHeader.Length == 0 || !authHeader.StartsWith("Basic")) return null;

            // Pull out the Credentials with are seperated by ':' and Base64 encoded
            string base64Credentials = authHeader.Substring(6);
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(base64Credentials)).Split(new char[] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[0])) return null;

            // Okay this is the credentials
            return credentials;
        }

        private bool TryGetPrincipal(string userName, string password, out IPrincipal principal)
        {
            if (this.users.Auth(userName, password)) //user != null)
            {
                // once the user is verified, assign it to an IPrincipal with the identity name and applicable roles
                principal = new GenericPrincipal(new GenericIdentity(userName),new string[] {});
                return true;
            }
            else
            {
                principal = null;
                return false;
            }
        }
    }
}
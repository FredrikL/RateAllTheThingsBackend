using System;
using Nancy;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class SignupModule : NancyModule
    {
        private readonly ISignup signup;

        public SignupModule(ISignup signup) : base("/Signup")
        {
            this.signup = signup;

            Post["/"] = x =>
                            {                                
                                try
                                {
                                    var email = Request.Form.email;
                                    new System.Net.Mail.MailAddress(email);
                                    this.signup.AddSignup(email);
                                    return Response.AsRedirect("/?signup=true");
                                }
                                catch(Exception e)
                                {
                                    
                                }

                                return Response.AsRedirect("/");
                            };
        }
    }
}
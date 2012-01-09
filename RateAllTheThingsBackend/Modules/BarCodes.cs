using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;
using Nancy.Security;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class BarCodes:NancyModule
    {
        private readonly IBarCodes barcodes;

        public BarCodes(IBarCodes barcodes)
        {
            this.RequiresAuthentication();

            this.barcodes = barcodes;

            Get["/BarCode"] = x =>
                                  {
                                      return Response.AsJson(this.barcodes.All());
                                  };

            Get["/BarCode/{format}/{code}"] = x =>
                                {
                                    var barcode = this.barcodes.Get(x.format, x.code);
                                    return Response.AsJson(new[] { barcode });
                                };
        }
    }

    public class AuthenticationBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            pipelines.EnableBasicAuthentication(new BasicAuthenticationConfiguration(
                container.Resolve<IUserValidator>(),
                "RateAllTheThings"));
        }
    }
}
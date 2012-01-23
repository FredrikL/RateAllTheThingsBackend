using Nancy;

namespace RateAllTheThingsBackend.Modules
{
    public class StaticContentModule : NancyModule
    {
        public StaticContentModule()
        {
            Get["/"] = x =>
                           {
                               return View["index"];
                           };
        }
    }
}
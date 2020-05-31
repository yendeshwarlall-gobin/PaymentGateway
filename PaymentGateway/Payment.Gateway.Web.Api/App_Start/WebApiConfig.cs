using System.Web.Http;
using Payment.Gateway.Web.Api.App_Start;
using Payment.Gateway.Web.Api.CustomHandlers;

namespace Payment.Gateway.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            StructuremapWebApi.Start();
            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthorizationHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

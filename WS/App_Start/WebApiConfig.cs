using Comments.Web.Filters;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Validation.Providers;

namespace WS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "{controller}/{action}",
                defaults: new { action = "Get" }, //
                constraints: null
            );

            config.Filters.Add(new HttpsActionFilter());
            //config.Filters.Add(new AuthorizeTokenAttribute());
            config.Filters.Add(new CheckModelForNullAttributeActionFilter());
            config.Filters.Add(new ValidationActionFilter());

            config.Services.RemoveAll(
                typeof(System.Web.Http.Validation.ModelValidatorProvider),
                v => v is InvalidModelValidatorProvider);


            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}

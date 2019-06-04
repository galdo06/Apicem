using System.Collections.Generic;
using System.IO;
using Noesis.Javascript;
using System.Web;

namespace CL.JavaScript
{
    public class JavaScriptHelper
    {
        private const string ScriptFileSeparator = ";null;";


        public static Dictionary<string, object> ConvertToLatLng(string x, string y)
        {
            System.Collections.Generic.Dictionary<string, object> anewpointObj = new Dictionary<string, object>();
            // Initialize a context
            using (JavascriptContext context = new JavascriptContext())
            {
                using (var streamReader = new StreamReader(HttpContext.Current.Server.MapPath(@"~\Javascript\proj4js-compressed.js")))
                {
                    context.Run(streamReader.ReadToEnd() + ScriptFileSeparator);
                }

                // Script
                string script = @"
                                            function ConvertToLatLng(x, y) {
                                                Proj4js.defs[""EPSG:32161""] = ""+proj=lcc +lat_1=18.43333333333333 +lat_2=18.03333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=200000 +y_0=200000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs"";
                                                Proj4js.defs[""EPSG:4326""] = ""+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs"";
                                                var source = new Proj4js.Proj('EPSG:32161');
                                                var dest = new Proj4js.Proj('EPSG:4326');
                                                var anewpoint = new Proj4js.Point(x, y);
                                                Proj4js.transform(source, dest, anewpoint);
                                                return anewpoint;
                                            }

                                           var anewpoint = ConvertToLatLng(parseFloat('" + x + "'),parseFloat('" + y + "')); ";

                // Running the script
                context.Run(script);

                // Getting a parameter
                anewpointObj = (System.Collections.Generic.Dictionary<string, object>)context.GetParameter("anewpoint");
            }
            return anewpointObj;
        }

        public static Dictionary<string, object> ConvertToStatePlane(string x, string y)
        {
            System.Collections.Generic.Dictionary<string, object> anewpointObj = new Dictionary<string, object>();
            // Initialize a context
            using (JavascriptContext context = new JavascriptContext())
            {
                using (var streamReader = new StreamReader(HttpContext.Current.Server.MapPath(@"~\Javascript\proj4js-compressed.js")))
                {
                    context.Run(streamReader.ReadToEnd() + ScriptFileSeparator);
                }

                // Script
                string script = @" 
                                            function ConvertToStatePlane(x, y) {
                                                Proj4js.defs[""EPSG:32161""] = ""+proj=lcc +lat_1=18.43333333333333 +lat_2=18.03333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=200000 +y_0=200000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs"";
                                                Proj4js.defs[""EPSG:4326""] = ""+proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs"";
                                                var dest = new Proj4js.Proj('EPSG:32161');
                                                var source = new Proj4js.Proj('EPSG:4326');
                                                var anewpoint = new Proj4js.Point(x, y);
                                                Proj4js.transform(source, dest, anewpoint);
                                                return anewpoint;
                                            }

                                           var anewpoint = ConvertToStatePlane(parseFloat('" + x + "'),parseFloat('" + y + "')); ";

                // Running the script
                context.Run(script);

                // Getting a parameter
                anewpointObj = (System.Collections.Generic.Dictionary<string, object>)context.GetParameter("anewpoint");
            }
            return anewpointObj;
        }

    }
}

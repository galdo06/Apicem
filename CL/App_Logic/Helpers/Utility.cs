using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Eisk.Web.App_Logic.Helpers
{
    public static class Utility
    {
        private const string ScriptFileSeparator = ";null;";

        public static string FormatUSPhoneNumber(string PhoneNumber)
        {
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                if (PhoneNumber.Length == 11)
                    return String.Format("{0:#(###) ###-####}", Convert.ToInt64(PhoneNumber));
                else if (PhoneNumber.Length == 10)
                    return String.Format("{0:(###) ###-####}", Convert.ToInt64(PhoneNumber));
            }

            return PhoneNumber;
        }

        public static string FormatUSZipCode(string ZipCode)
        {
            if (!string.IsNullOrEmpty(ZipCode))
            {
                if (ZipCode.Length == 9)
                    return new string('0', 9 - Convert.ToInt64(ZipCode).ToString().Length) + String.Format("{0:#####-####}", Convert.ToInt32(ZipCode));
                else if (ZipCode.Length == 5)
                    return new string('0', 5 - Convert.ToInt64(ZipCode).ToString().Length) + String.Format("{0:#####}", Convert.ToInt32(ZipCode));
            }

            return ZipCode;
        }

        public static object ValueOrDefault(object s, string sDefault)
        {
            if (s == null)
                return sDefault;
            return (object)s;
        }

        public static string ReplaceZipCodePhoneCharacters(object stringIN)
        {
            if (stringIN != null)
                return stringIN.ToString().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");

            return null;
        }


        public static Dictionary<string, object> ConvertToLatLng(string x, string y, string pathToJS)
        {
            System.Collections.Generic.Dictionary<string, object> anewpointObj = new Dictionary<string, object>();
            // Initialize a context
            using (JavascriptContext context = new JavascriptContext())
            {
                using (var streamReader = new StreamReader(new Page().Server.MapPath(pathToJS+"proj4js-compressed.js")))
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

        public static Dictionary<string, object> ConvertToStatePlane(string x, string y, string pathToJS)
        {
            System.Collections.Generic.Dictionary<string, object> anewpointObj = new Dictionary<string, object>();
            // Initialize a context
            using (JavascriptContext context = new JavascriptContext())
            {
                using (var streamReader = new StreamReader(new Page().Server.MapPath(pathToJS+"proj4js-compressed.js")))
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
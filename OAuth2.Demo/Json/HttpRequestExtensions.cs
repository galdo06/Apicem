using System.Linq;
using System.Web;

namespace OAuth2.Demo.Json
{
    public static class HttpRequestExtensions
    {
        public static bool IsJsonpRequest(this HttpRequestBase request)
        {
            return request.Params.AllKeys.Contains(JsonpResult.JsonCallbackKey);
        }
    }
}
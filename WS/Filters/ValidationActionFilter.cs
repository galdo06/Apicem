using System;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WS.Models;

namespace Comments.Web.Filters
{ 

//public class ValidationActionFilter : ActionFilterAttribute {   
//    public override void OnActionExecuting(HttpActionContext context)     {     
//        var modelState = context.ModelState;         if (!modelState.IsValid)         {  
//            dynamic errors = new JsonObject();             foreach (var key in modelState.Keys)   
//            {                 var state = modelState[key];           
//                if (state.Errors.Any())                 {                 
//                    errors[key] = state.Errors.First().ErrorMessage;             
//                }             }         
//            context.Response = new HttpResponseMessage<JsonValue>(errors, HttpStatusCode.BadRequest);   
//        }   
//    }
//}


    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                     .Where(e => e.Value.Errors.Count > 0)
                     .Select(e => new BaseResponse
                     {
                         Object = e.Key,
                         Content = e.Value.Errors.First().ErrorMessage
                     }).ToArray();

                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new ObjectContent<BaseResponse[]>(errors, new JsonMediaTypeFormatter());

                actionContext.Response = response;
            }
        }
    }

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//public class ValidationActionFilter : ActionFilterAttribute
//{

//    public override void OnActionExecuting(HttpActionContext actionContext)
//    {

//        if (!actionContext.ModelState.IsValid)
//        {

//            actionContext.Response = actionContext.Request.CreateErrorResponse(
//                HttpStatusCode.BadRequest, actionContext.ModelState);
//        }
//    }
//}
    //public class ValidationActionFilter : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(HttpActionContext context)
    //    {
    //        var modelState = context.ModelState;
    //        if (!modelState.IsValid)
    //        {
    //            JsonValue errors = new JsonObject();
    //            foreach (var key in modelState.Keys)
    //            {
    //                var state = modelState[key];
    //                if (state.Errors.Any())
    //                {
    //                    errors[key] = state.Errors.First().ErrorMessage;
    //                }
    //            }
    //            context.Response = context.Request.CreateResponse<JsonValue>(
    //                 HttpStatusCode.BadRequest,errors);
    //        }
    //    }
    //}

    public interface IKeyValueProvider
    {
        string GetValue(string key);
    }

    class RequestFormKeyValueProvider : IKeyValueProvider
    {
        public string GetValue(string key)
        {
            return HttpContext.Current.Request.Form[key];
        }
    }
}
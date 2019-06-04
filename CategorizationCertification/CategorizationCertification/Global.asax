<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    protected void Application_Start(object sender, EventArgs e)
    {
        RouteTable.Routes.Add("organism-details"        , new Route("organism/{edit_mode}/{organism_id}/{organismtype_id}.aspx"     , new PageRouteHandler("~/pages/organism_details.aspx")));
        RouteTable.Routes.Add("organisms"               , new Route("organisms.aspx"                                                , new PageRouteHandler("~/pages/organisms.aspx")));
        RouteTable.Routes.Add("organismselect"          , new Route("organismselect/{project_id}.aspx"                              , new PageRouteHandler("~/pages/organismselect.aspx")));
        RouteTable.Routes.Add("organismsinproject"      , new Route("organismsinproject/{project_id}.aspx"                          , new PageRouteHandler("~/pages/organismsinproject.aspx")));
        RouteTable.Routes.Add("organismtype-details"    , new Route("organismtype/{edit_mode}/{organismtype2_id}.aspx"              , new PageRouteHandler("~/pages/organismtype_details.aspx")));
        RouteTable.Routes.Add("organismtypes"           , new Route("organismtypes.aspx"                                            , new PageRouteHandler("~/pages/organismtypes.aspx")));
        RouteTable.Routes.Add("project-details"         , new Route("project/{edit_mode}/{project_id}.aspx"                         , new PageRouteHandler("~/pages/project_details.aspx")));
        RouteTable.Routes.Add("projects"                , new Route("projects.aspx"                                                 , new PageRouteHandler("~/pages/projects.aspx")));
    }    
    
    protected void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    protected void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        Eisk.Helpers.Logger.LogError();
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {

        if (HttpContext.Current.User != null)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                if (HttpContext.Current.User.Identity.AuthenticationType != "Forms")
                    throw new InvalidOperationException("Only forms authentication is supported, not " +
                        HttpContext.Current.User.Identity.AuthenticationType);

                System.Security.Principal.IIdentity userId = HttpContext.Current.User.Identity;

                //if role info is already NOT loaded into cache, put the role info in cache
                if (System.Web.HttpContext.Current.Cache[userId.Name] == null)
                {
                    string[] roles;

                    if (userId.Name == "admin1")
                        roles = new string[1] { "admin" };//this info will be generally collected from database
                    else if (userId.Name == "member1")
                        roles = new string[1] { "member" };//this info will be generally collected from database
                    else
                        roles = new string[1] { "public" };//this info will be generally collected from database                   

                    //1 hour sliding expiring time. Adding the roles in chache. This will be used in Application_AuthenticateRequest event located in Global.ascx.cs file to attach user Principal object.
                    System.Web.HttpContext.Current.Cache.Add(userId.Name, roles, null, DateTime.MaxValue, TimeSpan.FromHours(1), System.Web.Caching.CacheItemPriority.BelowNormal, null);
                }

                //now assign the user role in the current security context
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(userId, (string[])System.Web.HttpContext.Current.Cache[userId.Name]);
            }
        }

    }


    protected void Session_Start(object sender, EventArgs e) 
    {
        
    }

    protected void Session_End(object sender, EventArgs e) 
    {
    }
       
</script>

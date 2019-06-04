<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script RunAt="server">

    protected void Application_Start(object sender, EventArgs e)
    {
        /// Por lo que veo Hay que poner los que no tienen parametros abajo.
        /// El problema es que si no los que tienen muchos parámetros entren en no tienen 20140306
        RouteTable.Routes.Add("employee-details", new Route("employee/{edit_mode}/{employee_id}.aspx", new PageRouteHandler("~/web-form-samples/details-page.aspx")));
        RouteTable.Routes.Add("scientificname-details", new Route("scientificname/{edit_mode}/{scientificname_id}.aspx", new PageRouteHandler("~/secured/admin/scientificname_details.aspx")));
        RouteTable.Routes.Add("user-details", new Route("user/{edit_mode}/{user_id}.aspx", new PageRouteHandler("~/secured/admin/user_details.aspx")));
        RouteTable.Routes.Add("userinfo-details", new Route("userinfo/{edit_mode}/{userinfo_id}.aspx", new PageRouteHandler("~/secured/member/userinfo_details.aspx")));
        RouteTable.Routes.Add("group-details", new Route("group/{edit_mode}/{group_id}.aspx", new PageRouteHandler("~/secured/admin/group_details.aspx")));
        RouteTable.Routes.Add("project-details", new Route("project/{edit_mode}/{project_id}.aspx", new PageRouteHandler("~/secured/member/project_details.aspx")));
        RouteTable.Routes.Add("tl-project-details", new Route("tl_project/{edit_mode}/{project_id}.aspx", new PageRouteHandler("~/secured/member/tl/tl_project_details.aspx")));
        //RouteTable.Routes.Add("commonname-details", new Route("commonname/{edit_mode}/{project_id}/{organism_id}/{scientificname_id}/{commonname}.aspx", new PageRouteHandler("~/secured/member/commonname_details.aspx")));
        RouteTable.Routes.Add("commonname-details", new Route("commonname/{edit_mode}/{project_id}/{organism_id}/{scientificname_id}/{commonname}.aspx", new PageRouteHandler("~/secured/member/commonname_details.aspx")));
        RouteTable.Routes.Add("commonnames", new Route("commonnames/{project_id}.aspx", new PageRouteHandler("~/secured/member/commonnames.aspx")));
        RouteTable.Routes.Add("scientificnameselect", new Route("scientificnameselect/{project_id}/{organism_id}/{scientificname_id}/{commonname}.aspx", new PageRouteHandler("~/secured/member/scientificnameselect.aspx")));
        RouteTable.Routes.Add("tl", new Route("tl/{project_id}.aspx", new PageRouteHandler("~/secured/member/tl/treelocation.aspx")));
        RouteTable.Routes.Add("tl-tree-details", new Route("tl_tree/{edit_mode}/{project_id}/{project_organism_id}/{number}/{x}/{y}/{lat}/{lon}.aspx", new PageRouteHandler("~/secured/member/tl/tl_tree_details.aspx")));
        RouteTable.Routes.Add("tl-tree-details_edit", new Route("tl_tree/{edit_mode}/{project_id}/{project_organism_id}.aspx", new PageRouteHandler("~/secured/member/tl/tl_tree_details.aspx")));
        RouteTable.Routes.Add("tl-treeinventory", new Route("tl_treeinventory/{project_id}.aspx", new PageRouteHandler("~/secured/member/tl/treeinventory.aspx")));
        RouteTable.Routes.Add("tl-reports", new Route("tl_reports/{project_id}.aspx", new PageRouteHandler("~/secured/member/tl/tl_reports.aspx")));
        RouteTable.Routes.Add("tl-report-details", new Route("tl_report/{edit_mode}/{project_id}/{headeroneachreport}/{createindex}/{reports}.aspx", new PageRouteHandler("~/secured/member/tl/tl_report_details.aspx")));
        RouteTable.Routes.Add("tl-report-details-csv", new Route("tl_report_csv/{edit_mode}/{project_id}/{reports}.aspx", new PageRouteHandler("~/secured/member/tl/tl_report_details_csv.aspx")));
        RouteTable.Routes.Add("tl-upload-csv", new Route("tl_upload_csv/{project_id}.aspx", new PageRouteHandler("~/secured/member/tl/tl_upload_csv.aspx")));
        RouteTable.Routes.Add("notifications", new Route("notifications.aspx", new PageRouteHandler("~/secured/member/notifications.aspx")));
        RouteTable.Routes.Add("change-password", new Route("changepassword.aspx", new PageRouteHandler("~/secured/member/changepassword.aspx")));
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
                        roles = new string[1] { "admin" };//this info will be generally collected from database                   

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

 
using System;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using Eisk.DataAccessLayer;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Configuration;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;
using Noesis.Javascript;
using System.IO;
using Noesis.Javascript.Headless;
using System.Reflection;
using System.Web.UI;

public partial class TreeLocation_Page : System.Web.UI.Page
{
    protected string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"];
    private readonly JavascriptContext _javascriptContext = new JavascriptContext();
    private readonly List<string> _scriptFiles;
    private const string ScriptFileSeparator = ";null;";

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.Load += new System.EventHandler(this.Page_Load);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        txtFilter.Attributes.Add("OnKeyPress", "return enterKeyPressedOnTxt(event)");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(" var defaultCenter_X = '" + ConfigurationManager.AppSettings["DefaultCenter_X"] + "'; ");
        sb.AppendLine(" var defaultCenter_Y = '" + ConfigurationManager.AppSettings["DefaultCenter_Y"] + "'; ");
        sb.AppendLine(" var editorID = '" + (new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name)).UserID + "'; ");
        //sb.AppendLine(" var projectID = '" + RouteData.Values["project_id"] + "'; ");
        sb.AppendLine(" var project = JSON.parse('" + getProjectDetails() + "') ; ");
        sb.AppendLine(" var pageUrl = '" + Page.GetRouteUrl("tl-project-details", new { edit_mode = "edit", Project_id = RouteData.Values["Project_id"] }) + "'; ");
        sb.AppendLine(" var newTreeUrl = '" + Page.GetRouteUrl("tl-tree-details", new { edit_mode = "{edit_mode}", Project_id = RouteData.Values["Project_id"], project_organism_id = "{project_organism_id}", number = "{number}", x = "{x}", y = "{y}", lat = "{lat}", lon = "{lon}" }) + "'; ");
        sb.AppendLine(" var projectorganismid = '" + ((Request.QueryString["poid"] != null && Request.QueryString["poid"].ToString() != "0") ? Request.QueryString["poid"] : "") + "'; ");
        sb.AppendLine(" var lat " + ((Request.QueryString["lat"] != null && Request.QueryString["lat"].ToString() != "0") ? "= '" + Request.QueryString["lat"] + "'" : "") + " ; ");
        sb.AppendLine(" var lon " + ((Request.QueryString["lon"] != null && Request.QueryString["lon"].ToString() != "0") ? "= '" + Request.QueryString["lon"] + "'" : "") + " ; ");
        sb.AppendLine(" var Trees = JSON.parse('" + getTrees() + "') ; ");
        sb.AppendLine(" var Perimeters = JSON.parse('" + getPerimeters() + "') ; ");
        sb.AppendLine(" var Colors = JSON.parse('" + getColors() + "') ; ");
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", sb.ToString(), true);
    }


    protected void ButtonGoToViewPage_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("tl", new { Project_id = RouteData.Values["Project_id"] });
        Response.RedirectLocation +=
              "?poid=" + Page.RouteData.Values["project_organism_id"] as string
            + "&lat=" + Request.QueryString["lat"] != null ? Request.QueryString["lat"] : "0"
            + "&lon=" + Request.QueryString["lon"] != null ? Request.QueryString["lon"] : "0";
    }


    protected string getProjectDetails()
    {

        Project project = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(RouteData.Values["project_id"]));

        ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.FirstOrDefault();
        ProjectInfo projectInfo = project.ProjectInfoes.FirstOrDefault();

        if (projectInfoTreeLocation == null)
        {
            projectInfoTreeLocation = new ProjectInfoTreeLocation();

            projectInfoTreeLocation.X = 0;
            projectInfoTreeLocation.Y = 0;
            projectInfoTreeLocation.Lat = 0;
            projectInfoTreeLocation.Lon = 0;
            projectInfoTreeLocation.Acres = 0;
            projectInfoTreeLocation.Lots1 = 0;
            projectInfoTreeLocation.Parkings = 0;
            projectInfoTreeLocation.DistanceBetweenTrees = 10;

            projectInfoTreeLocation.ProjectID = project.ProjectID;
            projectInfoTreeLocation.ProjectReference.EntityKey = project.EntityKey;

            new ProjectInfoTreeLocationBLL().CreateNewProjectInfoTreeLocation(projectInfoTreeLocation);
        }

        var proj = new
        {
            projectInfo.ProjectName,
            projectInfoTreeLocation.ProjectID,
            projectInfoTreeLocation.Acres,
            projectInfoTreeLocation.DistanceBetweenTrees,
            projectInfoTreeLocation.Lat,
            projectInfoTreeLocation.Lon,
            projectInfoTreeLocation.Parkings,
            projectInfoTreeLocation.X,
            projectInfoTreeLocation.Y,
            projectInfo.City.CityName,
            projectInfo.City.CityID,
            CityX = projectInfo.City.X,
            CityY = projectInfo.City.Y,
            CityLat = projectInfo.City.Lat,
            CityLon = projectInfo.City.Lon,
            projectInfo.ZipCode
        };

        return new JavaScriptSerializer().Serialize(proj);
    }

    protected string getTrees()
    {
        List<Project_Organisms> project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(RouteData.Values["project_id"]));

        List<object> trees = new List<object>();

        foreach (Project_Organisms project_Organism in project_Organisms)
        {
            TreeDetail treeDetail = project_Organism.TreeDetails.ToList()[0];
            var tree = new
            {
                treeDetail.ActionProposed.ActionProposedDesc,
                treeDetail.ActionProposed.ActionProposedID,
                treeDetail.ActionProposed.Color.ColorDesc,
                treeDetail.ActionProposed.Color.ColorID,
                treeDetail.ActionProposed.Color.Code,
                treeDetail.MaritimeZone,
                treeDetail.Littoral,
                treeDetail.Commentary,
                treeDetail.Condition.ConditionDesc,
                treeDetail.Condition.ConditionID,
                treeDetail.CreatedDate,
                treeDetail.CreatorUserID,
                treeDetail.Dap,
                treeDetail.Dap_Counter,
                treeDetail.EditedDate,
                treeDetail.EditorUserID,
                treeDetail.Height,
                treeDetail.Lat,
                treeDetail.Lon,
                treeDetail.X,
                treeDetail.Y,
                treeDetail.Number,
                treeDetail.ProjectOrganismID,
                treeDetail.Project_Organisms.ProjectID,
                treeDetail.Project_Organisms.OrganismID,
                treeDetail.Project_Organisms.Organism.CommonName.CommonNameDesc,
                treeDetail.Project_Organisms.Organism.CommonName.CommonNameID,
                treeDetail.Project_Organisms.Organism.ScientificName.ScientificNameDesc,
                treeDetail.Project_Organisms.Organism.ScientificName.ScientificNameID,
                treeDetail.TreeDetailsID,
                treeDetail.Varas
            };
            trees.Add(tree);
        }

        return new JavaScriptSerializer().Serialize(trees);
    }

    protected string getPerimeters()
    {
        List<Perimeter> perimeters = new PerimeterBLL().GetPerimetersByProjectID(Convert.ToInt32(RouteData.Values["project_id"]));

        List<object> perimeterObjects = new List<object>();

        foreach (Perimeter perimeter in perimeters)
        {
            List<object> perimeterPointsObjects = new List<object>();

            foreach (PerimeterPoint perimeterPoint in perimeter.PerimeterPoints)
            {
                var perimeterPointObject = new
                {
                    perimeterPoint.PerimeterID,
                    perimeterPoint.PerimeterPointID,
                    perimeterPoint.X,
                    perimeterPoint.Y
                };
                perimeterPointsObjects.Add(perimeterPointObject);
            }

            var perimeterObject = new
            {
                perimeter.ColorID,
                perimeter.Color.Code,
                perimeter.Color.ColorDesc,
                perimeter.IsMainPerimeter,
                perimeter.PerimeterID,
                perimeter.PerimeterName,
                PerimeterPoints = perimeterPointsObjects,
                perimeter.ProjectID
            };
            perimeterObjects.Add(perimeterObject);
        }

        return new JavaScriptSerializer().Serialize(perimeterObjects);
    }

    [WebMethod]
    public static void EditProjectCenter(string projectID, string JStr)
    {
        Project project = new ProjectBLL().GetProjectByProjectId2(Convert.ToInt32(projectID));

        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            var nList = new JavaScriptSerializer().Deserialize<Object>(JStr);

            ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.Count == 0 ? new ProjectInfoTreeLocation() : _DatabaseContext.ProjectInfoTreeLocations.First(instance => instance.ProjectID == project.ProjectID);

            ProjectInfo projectInfo = project.ProjectInfoes.Count == 0 ? new ProjectInfo() : _DatabaseContext.ProjectInfoes.First(instance => instance.ProjectID == project.ProjectID);

            projectInfo.ProjectName = ((Dictionary<string, object>)nList).ContainsKey("ProjectName") ? Convert.ToInt32(((Dictionary<string, object>)nList)["ProjectName"]).ToString() : projectInfo.ProjectName;
            projectInfoTreeLocation.ProjectID = ((Dictionary<string, object>)nList).ContainsKey("ProjectID") ? Convert.ToInt32(((Dictionary<string, object>)nList)["ProjectID"]) : projectInfoTreeLocation.ProjectID;
            projectInfoTreeLocation.X = ((Dictionary<string, object>)nList).ContainsKey("X") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["X"]) : projectInfoTreeLocation.X;
            projectInfoTreeLocation.Y = ((Dictionary<string, object>)nList).ContainsKey("Y") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["Y"]) : projectInfoTreeLocation.Y;
            projectInfoTreeLocation.Lat = ((Dictionary<string, object>)nList).ContainsKey("Lat") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["Lat"]) : projectInfoTreeLocation.Lat;
            projectInfoTreeLocation.Lon = ((Dictionary<string, object>)nList).ContainsKey("Lon") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["Lon"]) : projectInfoTreeLocation.Lon;
            projectInfoTreeLocation.Acres = ((Dictionary<string, object>)nList).ContainsKey("Acres") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["Acres"]) : projectInfoTreeLocation.Acres;
            projectInfoTreeLocation.Lots0 = ((Dictionary<string, object>)nList).ContainsKey("Lots0") ? Convert.ToInt32(((Dictionary<string, object>)nList)["Lots0"]) : projectInfoTreeLocation.Lots0;
            projectInfoTreeLocation.Lots1 = ((Dictionary<string, object>)nList).ContainsKey("Lots1") ? Convert.ToInt32(((Dictionary<string, object>)nList)["Lots1"]) : projectInfoTreeLocation.Lots1;
            projectInfoTreeLocation.Lots2 = ((Dictionary<string, object>)nList).ContainsKey("Lots2") ? Convert.ToInt32(((Dictionary<string, object>)nList)["Lots2"]) : projectInfoTreeLocation.Lots2;
            projectInfoTreeLocation.Lots3 = ((Dictionary<string, object>)nList).ContainsKey("Lots3") ? Convert.ToInt32(((Dictionary<string, object>)nList)["Lots3"]) : projectInfoTreeLocation.Lots3;
            projectInfoTreeLocation.Mitigation = ((Dictionary<string, object>)nList).ContainsKey("Mitigation") ? Convert.ToInt32(((Dictionary<string, object>)nList)["Mitigation"]) : projectInfoTreeLocation.Mitigation;
            projectInfoTreeLocation.SocialInterest = ((Dictionary<string, object>)nList).ContainsKey("SocialInterest") ? Convert.ToBoolean(((Dictionary<string, object>)nList)["SocialInterest"]) : projectInfoTreeLocation.SocialInterest;
            projectInfoTreeLocation.PreviouslyImpacted = ((Dictionary<string, object>)nList).ContainsKey("PreviouslyImpacted") ? Convert.ToBoolean(((Dictionary<string, object>)nList)["PreviouslyImpacted"]) : projectInfoTreeLocation.PreviouslyImpacted;
            projectInfoTreeLocation.Parkings = ((Dictionary<string, object>)nList).ContainsKey("Parkings") ? Convert.ToInt32(((Dictionary<string, object>)nList)["Parkings"]) : projectInfoTreeLocation.Parkings;
            projectInfoTreeLocation.DistanceBetweenTrees = ((Dictionary<string, object>)nList).ContainsKey("DistanceBetweenTrees") ? Convert.ToInt32(((Dictionary<string, object>)nList)["DistanceBetweenTrees"]) : projectInfoTreeLocation.DistanceBetweenTrees;
            projectInfo.ZipCode = ((Dictionary<string, object>)nList).ContainsKey("ZipCode") ? (((Dictionary<string, object>)nList)["ZipCode"]).ToString() : projectInfo.ZipCode;
            projectInfo.CityID = ((Dictionary<string, object>)nList).ContainsKey("City") ? Convert.ToInt32(((Dictionary<string, object>)nList)["City"]) : projectInfo.City.CityID;

            projectInfoTreeLocation.ProjectID = project.ProjectID;
            projectInfoTreeLocation.ProjectReference.EntityKey = project.EntityKey;

            projectInfo.ProjectID = project.ProjectID;
            projectInfo.ProjectReference.EntityKey = project.EntityKey;

            if (project.ProjectInfoTreeLocations.Count == 0)
                new ProjectInfoTreeLocationBLL().CreateNewProjectInfoTreeLocation(projectInfoTreeLocation);

            _DatabaseContext.SaveChanges();
        }
    }

    [WebMethod]
    public static object SetPerimeterFromSIP(string projectID, string JStr, string userID)
    {
        Project project = new ProjectBLL().GetProjectByProjectId2(Convert.ToInt32(projectID));

        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            var nList = new JavaScriptSerializer().Deserialize<Object>(JStr);

            ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.Count == 0 ? new ProjectInfoTreeLocation() : _DatabaseContext.ProjectInfoTreeLocations.First(instance => instance.ProjectID == project.ProjectID);
            projectInfoTreeLocation.X = ((Dictionary<string, object>)nList).ContainsKey("X") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["X"]) : projectInfoTreeLocation.X;
            projectInfoTreeLocation.Y = ((Dictionary<string, object>)nList).ContainsKey("Y") ? Convert.ToDecimal(((Dictionary<string, object>)nList)["Y"]) : projectInfoTreeLocation.Y;
           
            string url =
                        "http://www.gis.sip.pr.gov/ArcGIS/rest/services/Tools/Localizdor_JS8/MapServer/identify?f=json&geometry=%7B%22" +
                            "x%22%3A" + miX + "%2C%22" +
                            "y%22%3A" + miY + "%2C%22" +
                            "spatialReference%22%3A%7B%22wkid%22%3A32161%7D%7D&tolerance=1&returnGeometry=true&mapExtent=%7B%22" +
                            "xmin%22%3A" + miX + "%2C%22" +
                            "ymin%22%3A" + miY + "%2C%22" +
                            "xmax%22%3A" + miX + "%2C%22" +
                            "ymax%22%3A" + miY + "%2C%22" +
                            "spatialReference%22%3A%7B%22wkid%22%3A32161%7D%7D&imageDisplay=1920%2C304%2C96&geometryType=esriGeometryPoint&sr=32161&layers=all%3A0&callback=dojo.io.script.jsonp_dojoIoScript6._jsonpCallback";

            var json = new WebClient().DownloadString(url);

            var points = new JavaScriptSerializer().Deserialize<Object>(json.Replace("dojo.io.script.jsonp_dojoIoScript6._jsonpCallback(", "").Replace(");", ""));

            JObject j = JObject.FromObject(points);
            JArray jArray = GetRings(j);

            Perimeter mainPerimeter = _DatabaseContext.Perimeters.FirstOrDefault(instance => instance.IsMainPerimeter);
            if (mainPerimeter != null)
            {
                mainPerimeter.IsMainPerimeter = false;
                mainPerimeter.PerimeterName = "Perímetro";

                // Foreign Key Color
                Color blue = _DatabaseContext.Colors.First(instance => instance.ColorID == 1);
                mainPerimeter.ColorID = blue.ColorID;
                mainPerimeter.ColorReference.EntityKey = blue.EntityKey;
                //
            }

            Perimeter newPerimeter = new Perimeter();

            List<PerimeterPoint> perimeterPoints = new List<PerimeterPoint>();
            List<object> perimeterPointsObjects = new List<object>();

            // Initialize a context
            using (JavascriptContext context = new JavascriptContext())
            {
                System.Collections.Generic.Dictionary<string, object> anewpointObj = new Dictionary<string, object>();
                using (var streamReader = new StreamReader(new Page().Server.MapPath(@"~/App_Resources\client-scripts\tl\proj4js-compressed.js")))
                {
                    context.Run(streamReader.ReadToEnd() + ScriptFileSeparator);
                }

                foreach (JToken JToken1 in jArray)
                {
                    foreach (JToken JToken2 in JToken1)
                    {

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

                                           var anewpoint = ConvertToLatLng(parseFloat('" + ((JValue)(JToken2[0])).Value.ToString() + "'),parseFloat('" + ((JValue)(JToken2[1])).Value.ToString() + "')); ";

                        // Running the script
                        context.Run(script);

                        // Getting a parameter
                        anewpointObj = (System.Collections.Generic.Dictionary<string, object>)context.GetParameter("anewpoint");

                        PerimeterPoint perimeterPoint = new PerimeterPoint();

                        perimeterPoint.Y = Convert.ToDecimal(anewpointObj["y"]);
                        perimeterPoint.X = Convert.ToDecimal(anewpointObj["x"]);

                        perimeterPoint.PerimeterID = newPerimeter.PerimeterID;
                        perimeterPoint.PerimeterReference.EntityKey = newPerimeter.EntityKey;

                        newPerimeter.PerimeterPoints.Add(perimeterPoint);

                        var perimeterPointObject = new
                        {
                            perimeterPoint.PerimeterID,
                            perimeterPoint.PerimeterPointID,
                            perimeterPoint.X,
                            perimeterPoint.Y
                        };
                        perimeterPointsObjects.Add(perimeterPointObject);
                    }
                }
            }

            newPerimeter.IsMainPerimeter = true;
            newPerimeter.PerimeterName = "Perímetro Principal";

            newPerimeter.CreatedDate = DateTime.Now;
            newPerimeter.CreatorUserID = new Guid(userID);
            newPerimeter.EditedDate = DateTime.Now;
            newPerimeter.EditorUserID = new Guid(userID);


            // Foreign Key Project
            newPerimeter.ProjectID = project.ProjectID;
            newPerimeter.ProjectReference.EntityKey = project.EntityKey;
            //

            // Foreign Key Color
            Color red = _DatabaseContext.Colors.First(instance => instance.ColorID == 2);
            newPerimeter.ColorID = red.ColorID;
            newPerimeter.ColorReference.EntityKey = red.EntityKey;
            //

            _DatabaseContext.Perimeters.AddObject(newPerimeter);

            _DatabaseContext.SaveChanges();

            var perimeterObject = new
                {
                    newPerimeter.ColorID,
                    newPerimeter.Color.Code,
                    newPerimeter.Color.ColorDesc,
                    newPerimeter.IsMainPerimeter,
                    newPerimeter.PerimeterID,
                    newPerimeter.PerimeterName,
                    PerimeterPoints = perimeterPointsObjects,
                    newPerimeter.ProjectID
                };

            return perimeterObject;
        }
    }

    private static JArray GetRings(JObject j)
    {
        JArray jArrayRet = null;
        JEnumerable<JToken> t = j.Children();

        if (j.Count >= 1)
        {
            foreach (KeyValuePair<string, JToken> JToken in j)
            {
                if (JToken.Key == "rings")
                {
                    return (JArray)JToken.Value;
                }

                if (JToken.Value.Type == JTokenType.Array)
                {
                    JArray jArray = (JArray)JToken.Value;

                    foreach (JToken JToken2 in jArray)
                    {
                        jArrayRet = GetRings(((Newtonsoft.Json.Linq.JObject)(JToken2)));
                        if (jArrayRet != null)
                            return jArrayRet;
                    }
                }
                else
                    if (JToken.Value.Type == JTokenType.Object)
                    {
                        jArrayRet = GetRings(((Newtonsoft.Json.Linq.JObject)(JToken.Value)));
                        if (jArrayRet != null)
                            return jArrayRet;
                    }
            }
        }

        return jArrayRet;
    }

    [WebMethod]
    public static void DeleteTree(string projectOrganismID)
    {
        new Project_OrganismsBLL().DeleteProject_OrganismByProjectOrganismID(Convert.ToInt32(projectOrganismID));
    }

    [WebMethod]
    public static Int32 AddEditPerimeter(string projectID, string perimeterID, string perimeterName, string isMainPerimeter, string colorID, string perimeterPoints, string userID)
    {
        int perimeterIDINT;
        int projectIDINT;
        int colorIDINT;
        Perimeter perimeter;
        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            if (int.TryParse(perimeterID, out perimeterIDINT) && perimeterIDINT == 0)
            {
                perimeter = new Perimeter();

                // Foreign Key Project
                projectIDINT = Convert.ToInt32(projectID);
                Project project = _DatabaseContext.Projects.First(instance => instance.ProjectID == projectIDINT);
                perimeter.ProjectID = project.ProjectID;
                perimeter.ProjectReference.EntityKey = project.EntityKey;
                //

                // Foreign Key Color
                colorIDINT = Convert.ToInt32(colorID);
                Color color = _DatabaseContext.Colors.First(instance => instance.ColorID == colorIDINT);
                perimeter.ColorID = color.ColorID;
                perimeter.ColorReference.EntityKey = color.EntityKey;
                //

                perimeter.PerimeterName = perimeterName;
                perimeter.IsMainPerimeter = Convert.ToBoolean(isMainPerimeter);

                Dictionary<string, object> perimeterPointsList = (Dictionary<string, object>)(new JavaScriptSerializer().Deserialize<Object>(perimeterPoints));
                if (perimeterPointsList.Count > 0)
                {
                    object[] perimeterPointsObject = (object[])perimeterPointsList["perimeterPoints"];
                    foreach (Dictionary<string, object> perimeterPointObject in perimeterPointsObject)
                    {
                        PerimeterPoint perimeterPoint = new PerimeterPoint();

                        if (perimeterPointObject.Count == 2)
                        {
                            decimal obj0;
                            decimal obj1;

                            if (decimal.TryParse(perimeterPointObject.ToList()[0].Value.ToString(), out obj0))
                                if (obj0 > 0) // is Y
                                    perimeterPoint.Y = Convert.ToDecimal(obj0);
                                else if (obj0 < 0)
                                    perimeterPoint.X = Convert.ToDecimal(obj0);

                            if (decimal.TryParse(perimeterPointObject.ToList()[1].Value.ToString(), out obj1))
                                if (obj1 > 0) // is X
                                    perimeterPoint.Y = Convert.ToDecimal(obj1);
                                else if (obj1 < 0)
                                    perimeterPoint.X = Convert.ToDecimal(obj1);

                            perimeterPoint.PerimeterID = perimeter.PerimeterID;
                            perimeterPoint.PerimeterReference.EntityKey = perimeter.EntityKey;

                            perimeter.PerimeterPoints.Add(perimeterPoint);
                            // _DatabaseContext.PerimeterPoints.AddObject(perimeterPoint);
                        }
                    }
                }
                perimeter.CreatedDate = DateTime.Now;
                perimeter.CreatorUserID = new Guid(userID);
                perimeter.EditedDate = DateTime.Now;
                perimeter.EditorUserID = new Guid(userID);

                _DatabaseContext.Perimeters.AddObject(perimeter);
            }
            else
            {
                perimeter = _DatabaseContext.Perimeters.First(instance => instance.PerimeterID == perimeterIDINT);

                // Foreign Key Color
                colorIDINT = Convert.ToInt32(colorID);
                Color color = _DatabaseContext.Colors.First(instance => instance.ColorID == colorIDINT);
                perimeter.ColorID = color.ColorID;
                perimeter.ColorReference.EntityKey = color.EntityKey;
                //

                perimeter.PerimeterName = perimeterName;
                perimeter.IsMainPerimeter = Convert.ToBoolean(isMainPerimeter);

                Dictionary<string, object> perimeterPointsList = (Dictionary<string, object>)(new JavaScriptSerializer().Deserialize<Object>(perimeterPoints));
                if (perimeterPointsList.Count > 0)
                {
                    List<PerimeterPoint> oldPerimeterPoints = _DatabaseContext.PerimeterPoints.Where(instance => instance.PerimeterID == perimeterIDINT).ToList();

                    foreach (PerimeterPoint perimeterPoint in oldPerimeterPoints)
                    {
                        _DatabaseContext.PerimeterPoints.DeleteObject(perimeterPoint);
                    }

                    object[] perimeterPointsObject = (object[])perimeterPointsList["perimeterPoints"];
                    foreach (Dictionary<string, object> perimeterPointObject in perimeterPointsObject)
                    {
                        PerimeterPoint perimeterPoint = new PerimeterPoint();

                        if (perimeterPointObject.Count == 2)
                        {
                            decimal obj0;
                            decimal obj1;

                            if (decimal.TryParse(perimeterPointObject.ToList()[0].Value.ToString(), out obj0))
                                if (obj0 > 0) // is Y
                                    perimeterPoint.Y = Convert.ToDecimal(obj0);
                                else if (obj0 < 0)
                                    perimeterPoint.X = Convert.ToDecimal(obj0);

                            if (decimal.TryParse(perimeterPointObject.ToList()[1].Value.ToString(), out obj1))
                                if (obj1 > 0) // is X
                                    perimeterPoint.Y = Convert.ToDecimal(obj1);
                                else if (obj1 < 0)
                                    perimeterPoint.X = Convert.ToDecimal(obj1);

                            perimeterPoint.PerimeterID = perimeter.PerimeterID;
                            perimeterPoint.PerimeterReference.EntityKey = perimeter.EntityKey;

                            perimeter.PerimeterPoints.Add(perimeterPoint);
                            // _DatabaseContext.PerimeterPoints.AddObject(perimeterPoint);
                        }

                        //PerimeterPoint perimeterPoint = new PerimeterPoint();
                        //perimeterPoint.Y = Convert.ToDecimal(perimeterPointObject["Ya"]);
                        //perimeterPoint.X = Convert.ToDecimal(perimeterPointObject["Za"]);

                        //perimeterPoint.PerimeterID = perimeter.PerimeterID;
                        //perimeterPoint.PerimeterReference.EntityKey = perimeter.EntityKey;

                        //perimeter.PerimeterPoints.Add(perimeterPoint);
                        //// _DatabaseContext.PerimeterPoints.AddObject(perimeterPoint);
                    }
                }
                perimeter.EditedDate = DateTime.Now;
                perimeter.EditorUserID = new Guid(userID);
            }

            _DatabaseContext.SaveChanges();

            return _DatabaseContext.Perimeters.ToArray()[_DatabaseContext.Perimeters.Count() - 1].PerimeterID;
        }
    }

    protected string getColors()
    {
        List<Color> colors = new ColorBLL().GetAllColors();

        List<object> colorObjects = new List<object>();

        foreach (Color color in colors)
        {

            var colorObject = new
            {
                color.ColorID,
                color.Code,
                color.ColorDesc
            };
            colorObjects.Add(colorObject);
        }

        return new JavaScriptSerializer().Serialize(colorObjects);
    }

    [WebMethod]
    public static void DeletePerimeter(string perimeterID)
    {
        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            int perimeterIDINT = Convert.ToInt32(perimeterID);
            Perimeter perimeter = _DatabaseContext.Perimeters.First(instance => instance.PerimeterID == perimeterIDINT);
            List<PerimeterPoint> perimeterPoints = _DatabaseContext.PerimeterPoints.Where(instance => instance.PerimeterID == perimeterIDINT).ToList();

            foreach (PerimeterPoint perimeterPoint in perimeterPoints)
            {
                _DatabaseContext.PerimeterPoints.DeleteObject(perimeterPoint);
            }

            _DatabaseContext.Perimeters.DeleteObject(perimeter);

            _DatabaseContext.SaveChanges();
        }
    }


    [WebMethod]
    public static void EditTreePosition(string projectOrganismID, string x, string y, string lat, string lon, string userID)
    {
        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            int projectOrganismIDINT = Convert.ToInt32(projectOrganismID);
            TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.ProjectOrganismID == projectOrganismIDINT);
            treeDetail.X = Convert.ToDecimal(x);
            treeDetail.Y = Convert.ToDecimal(y);
            treeDetail.Lat = Convert.ToDecimal(lat);
            treeDetail.Lon = Convert.ToDecimal(lon);
            treeDetail.EditedDate = DateTime.Now;
            treeDetail.EditorUserID = new Guid(userID);

            _DatabaseContext.SaveChanges();
        }
    }

    [WebMethod]
    public static void EditTreeActionProposedID(string actionProposedID, string projectOrganismID, string userID)
    {
        int actionProposedIDINT = Convert.ToInt32(actionProposedID);
        int projectOrganismIDINT = Convert.ToInt32(projectOrganismID);
        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.ProjectOrganismID == projectOrganismIDINT);
            ActionProposed actionProposed = _DatabaseContext.ActionProposeds.First(instance => instance.ActionProposedID == actionProposedIDINT);
            treeDetail.ActionProposedID = actionProposed.ActionProposedID;
            treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;
            treeDetail.EditedDate = DateTime.Now;
            treeDetail.EditorUserID = new Guid(userID);

            _DatabaseContext.SaveChanges();
        }
    }


}


using System;
using System.Web.UI.WebControls;
using Eisk.Helpers;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Web;
using System.Web.Security;
using Eisk.Web.App_Logic.Helpers;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.DataAccessLayer;
using Noesis.Javascript;
using System.IO;
using System.Web.UI;

public partial class Tl_Project_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    protected User editor;
    protected User creator;
    protected ProjectInfoTreeLocation projectInfoTreeLocation;
    protected ProjectInfo projectInfo;
    protected string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"];

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    #region "Select handlers"

    protected void OdsProject_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewTlProject.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewTlProject.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewTlProject.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");

        if (formViewTlProject.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewTlProject_DataBound(object sender, System.EventArgs e)
    {
        DropDownList ddlDistanceBetweenTrees = (DropDownList)formViewTlProject.FindControl("ddlDistanceBetweenTrees");
        if (ddlDistanceBetweenTrees != null)
            ddlDistanceBetweenTrees.SelectedValue = projectInfoTreeLocation == null ? "0" : projectInfoTreeLocation.DistanceBetweenTrees.ToString();

        if (Page.Request.UrlReferrer != null)
            if (Page.Request.UrlReferrer.AbsolutePath.Contains("new") && !IsPostBack)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Insert successful");
            }
    }

    #endregion

    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (formViewTlProject.CurrentMode == FormViewMode.Insert)
        {
            formViewTlProject.InsertItem(true);
        }
        else
        {
            formViewTlProject.UpdateItem(true);
        }
    }


    protected void ButtonTreeLocation_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("tl", new { project_id = RouteData.Values["Project_id"] });
        Response.RedirectLocation += "?poid=0&lat=0&lon=0";
    }

    protected void ButtonEditProjectInfoTreeLocation_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("tl-project-details", new { edit_mode = "edit", Project_id = RouteData.Values["Project_id"] });
    }

    protected void ButtonEditProjectInfo_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("project-details", new { edit_mode = "edit", Project_id = RouteData.Values["Project_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/member/projects.aspx", true);
    }

    protected void ButtonGoToViewPage_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("tl-project-details", new { edit_mode = "view", Project_id = RouteData.Values["Project_id"] });
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewTlProject_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        UserBLL userBLL = new UserBLL();

        TextBox txtProject = (TextBox)formViewTlProject.FindControl("txtProject");
        ProjectActionStatus status = Validate(txtProject.Text, actionType.insert);

        if (status == ProjectActionStatus.Success)
        {
            User creator = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            e.Values["CreatedDate"] = DateTime.Now;
            e.Values["CreatorUserID"] = creator.UserID;
            e.Values["EditedDate"] = DateTime.Now;
            e.Values["EditorUserId"] = creator.UserID;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }


    private static ProjectActionStatus Validate(string Project, actionType act)
    {
        ProjectBLL ProjectBLL = new ProjectBLL();
        List<Project> Projects = ProjectBLL.GetProjectByFilter(Project.Trim());
        if (Projects.Count > 0 && act == actionType.insert)
            return ProjectActionStatus.Duplicate;
        else
            return ProjectActionStatus.Success;
    }

    public string GetErrorMessage(ProjectActionStatus status)
    {
        switch (status)
        {
            case ProjectActionStatus.Duplicate:
                return "Project Name address already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsProject_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int integer;
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
            Project project = new ProjectBLL().GetProjectByProjectID(result);

            UserBLL userBLL = new UserBLL();
            Eisk.BusinessEntities.User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            if (user != null)
            {
                // ProjectInfoTreeLocation
                TextBox txtX = (TextBox)formViewTlProject.FindControl("txtX");
                TextBox txtY = (TextBox)formViewTlProject.FindControl("txtY");
                TextBox txtLat = (TextBox)formViewTlProject.FindControl("txtLat");
                TextBox txtLon = (TextBox)formViewTlProject.FindControl("txtLon");
                TextBox txtParking = (TextBox)formViewTlProject.FindControl("txtParking");
                RadioButtonList rblCalc = (RadioButtonList)formViewTlProject.FindControl("rblCalc");
                TextBox txtAcres = (TextBox)formViewTlProject.FindControl("txtAcres");
                DropDownList ddlDistanceBetweenTrees = (DropDownList)formViewTlProject.FindControl("ddlDistanceBetweenTrees");
                TextBox txtLots0 = (TextBox)formViewTlProject.FindControl("txtLots0");
                TextBox txtLots1 = (TextBox)formViewTlProject.FindControl("txtLots1");
                TextBox txtLots2 = (TextBox)formViewTlProject.FindControl("txtLots2");
                TextBox txtLots3 = (TextBox)formViewTlProject.FindControl("txtLots3");
                RadioButtonList rblSocialInterest = (RadioButtonList)formViewTlProject.FindControl("rblSocialInterest");
                RadioButtonList rblPreviouslyImpacted = (RadioButtonList)formViewTlProject.FindControl("rblPreviouslyImpacted");

                ProjectInfoTreeLocation projectInfoTreeLocation = new ProjectInfoTreeLocation();
                projectInfoTreeLocation.X = Convert.ToDecimal(txtX.Text);
                projectInfoTreeLocation.Y = Convert.ToDecimal(txtY.Text);
                projectInfoTreeLocation.Lat = Convert.ToDecimal(txtLat.Text);
                projectInfoTreeLocation.Lon = Convert.ToDecimal(txtLon.Text);
                projectInfoTreeLocation.Parkings = Convert.ToInt32(txtParking.Text);
                projectInfoTreeLocation.Acres = Convert.ToDecimal(txtAcres.Text);
                projectInfoTreeLocation.SocialInterest = rblSocialInterest.SelectedValue == "1";
                projectInfoTreeLocation.PreviouslyImpacted = rblPreviouslyImpacted.SelectedValue == "1";

                projectInfoTreeLocation.Mitigation = Convert.ToInt32(rblCalc.SelectedValue);
                if (rblCalc.SelectedValue == "0")
                    projectInfoTreeLocation.Lots0 = projectInfoTreeLocation.Lots1 = projectInfoTreeLocation.Lots2 = projectInfoTreeLocation.Lots3 = 0;
                else
                {
                    projectInfoTreeLocation.Lots0 = Int32.TryParse(txtLots0.Text, out integer) ? integer : 0;
                    projectInfoTreeLocation.Lots1 = Int32.TryParse(txtLots1.Text, out integer) ? integer : 0;
                    projectInfoTreeLocation.Lots2 = Int32.TryParse(txtLots2.Text, out integer) ? integer : 0;
                    projectInfoTreeLocation.Lots3 = Int32.TryParse(txtLots3.Text, out integer) ? integer : 0;
                    projectInfoTreeLocation.DistanceBetweenTrees = null;
                }

                projectInfoTreeLocation.ProjectID = project.ProjectID;
                projectInfoTreeLocation.ProjectReference.EntityKey = project.EntityKey;

                new ProjectInfoTreeLocationBLL().CreateNewProjectInfoTreeLocation(projectInfoTreeLocation);
                //
            }

            Response.RedirectToRoute(new { edit_mode = "edit", Project_id = result.ToString() });
        }
    }

    protected void FormViewTlProject_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            // Display a user-friendly message 
            ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(e.Exception);
            // Indicate that the exception has been handled 
            e.ExceptionHandled = true;
            // Keep the row in edit mode 
            e.KeepInInsertMode = true;
        }
    }

    #endregion

    #region "Update handlers"

    protected void FormViewTlProject_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        Project project = new ProjectBLL().GetProjectByProjectId2((int)e.Keys["ProjectId"]);

        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            // ProjectInfoTreeLocation
            TextBox txtX = (TextBox)formViewTlProject.FindControl("txtX");
            TextBox txtY = (TextBox)formViewTlProject.FindControl("txtY");

            TextBox txtLat = (TextBox)formViewTlProject.FindControl("txtLat");
            TextBox txtLon = (TextBox)formViewTlProject.FindControl("txtLon");

            TextBox txtParkings = (TextBox)formViewTlProject.FindControl("txtParkings");
            RadioButtonList rblCalc = (RadioButtonList)formViewTlProject.FindControl("rblCalc");
            TextBox txtAcres = (TextBox)formViewTlProject.FindControl("txtAcres");
            DropDownList ddlDistanceBetweenTrees = (DropDownList)formViewTlProject.FindControl("ddlDistanceBetweenTrees");
            TextBox txtLots0 = (TextBox)formViewTlProject.FindControl("txtLots0");
            TextBox txtLots1 = (TextBox)formViewTlProject.FindControl("txtLots1");
            TextBox txtLots2 = (TextBox)formViewTlProject.FindControl("txtLots2");
            TextBox txtLots3 = (TextBox)formViewTlProject.FindControl("txtLots3");
            RadioButtonList rblSocialInterest = (RadioButtonList)formViewTlProject.FindControl("rblSocialInterest");
            RadioButtonList rblPreviouslyImpacted = (RadioButtonList)formViewTlProject.FindControl("rblPreviouslyImpacted");

            ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.Count == 0 ? new ProjectInfoTreeLocation() : _DatabaseContext.ProjectInfoTreeLocations.First(instance => instance.ProjectID == project.ProjectID);

            decimal dec;
            int integer;

            RadioButtonList rblPosition = (RadioButtonList)formViewTlProject.FindControl("rblPosition");

            if (rblPosition.SelectedValue == "0") // Nad83
            {
                projectInfoTreeLocation.X = decimal.TryParse(txtX.Text, out dec) ? Convert.ToDecimal(txtX.Text) : 0;
                projectInfoTreeLocation.Y = decimal.TryParse(txtY.Text, out dec) ? Convert.ToDecimal(txtY.Text) : 0;

                if (projectInfoTreeLocation.X != 0 && projectInfoTreeLocation.Y != 0)
                {
                    Dictionary<string, object> anewpointObj = Utility.ConvertToLatLng(projectInfoTreeLocation.X.ToString(), projectInfoTreeLocation.Y.ToString(), @"~/App_Resources/client-scripts/tl/");

                    projectInfoTreeLocation.Lat = Convert.ToDecimal(anewpointObj["y"]);
                    txtLat.Text = projectInfoTreeLocation.Lat.ToString();
                    projectInfoTreeLocation.Lon = Convert.ToDecimal(anewpointObj["x"]);
                    txtLon.Text = projectInfoTreeLocation.Lon.ToString();
                }
                else
                {
                    projectInfoTreeLocation.Lat = 0;
                    projectInfoTreeLocation.Lon = 0;
                }
            }
            else // StatePlanes
            {
                projectInfoTreeLocation.Lat = decimal.TryParse(txtLat.Text, out dec) ? Convert.ToDecimal(txtLat.Text) : 0;
                projectInfoTreeLocation.Lon = decimal.TryParse(txtLon.Text, out dec) ? Convert.ToDecimal(txtLon.Text) : 0;

                if (projectInfoTreeLocation.Lat != 0 && projectInfoTreeLocation.Lon != 0)
                {
                    Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(projectInfoTreeLocation.Lon.ToString(), projectInfoTreeLocation.Lat.ToString(), @"~/App_Resources/client-scripts/tl/");

                    projectInfoTreeLocation.X = Convert.ToDecimal(anewpointObj["x"]);
                    txtX.Text = projectInfoTreeLocation.X.ToString();
                    projectInfoTreeLocation.Y = Convert.ToDecimal(anewpointObj["y"]);
                    txtY.Text = projectInfoTreeLocation.Y.ToString();
                }
                else
                {
                    projectInfoTreeLocation.X = 0;
                    projectInfoTreeLocation.Y = 0;
                }
            }

            projectInfoTreeLocation.Parkings = Int32.TryParse(txtParkings.Text, out integer) ? Convert.ToInt32(txtParkings.Text) : 0;
            projectInfoTreeLocation.Acres = decimal.TryParse(txtAcres.Text, out dec) ? Convert.ToDecimal(txtAcres.Text) : 0;
            projectInfoTreeLocation.DistanceBetweenTrees = null;
            projectInfoTreeLocation.PreviouslyImpacted = null;

            projectInfoTreeLocation.Mitigation = Convert.ToInt32(rblCalc.SelectedValue);
            if (Convert.ToInt32(rblCalc.SelectedValue) > 1)
            {// Cálculo por perímetro
                projectInfoTreeLocation.Lots0 = projectInfoTreeLocation.Lots1 = projectInfoTreeLocation.Lots2 = projectInfoTreeLocation.Lots3 = 0;
                if (rblPreviouslyImpacted.SelectedValue == "0") // Previamente impactado = No
                {
                    projectInfoTreeLocation.DistanceBetweenTrees = Int32.TryParse(ddlDistanceBetweenTrees.SelectedValue, out integer) ? Convert.ToInt32(ddlDistanceBetweenTrees.SelectedValue) : 0;
                }
                projectInfoTreeLocation.PreviouslyImpacted = rblPreviouslyImpacted.SelectedValue == "1";
            }
            else
            {// Cálculo por solares
                projectInfoTreeLocation.Lots0 = Int32.TryParse(txtLots0.Text, out integer) ? integer : 0;
                projectInfoTreeLocation.Lots1 = Int32.TryParse(txtLots1.Text, out integer) ? integer : 0;
                projectInfoTreeLocation.Lots2 = Int32.TryParse(txtLots2.Text, out integer) ? integer : 0;
                projectInfoTreeLocation.Lots3 = Int32.TryParse(txtLots3.Text, out integer) ? integer : 0;
                projectInfoTreeLocation.DistanceBetweenTrees = null;
            }

            projectInfoTreeLocation.SocialInterest = rblSocialInterest.SelectedValue == "1";

            projectInfoTreeLocation.ProjectID = project.ProjectID;
            projectInfoTreeLocation.ProjectReference.EntityKey = project.EntityKey;

            if (project.ProjectInfoTreeLocations.Count == 0)
            {
                new ProjectInfoTreeLocationBLL().CreateNewProjectInfoTreeLocation(projectInfoTreeLocation);
            }
            else
                _DatabaseContext.SaveChanges();
            //
        }

        Type myType = (typeof(Project));
        PropertyInfo[] props = myType.GetProperties();

        string[] arrNewValues = new string[e.NewValues.Keys.Count];
        e.NewValues.Keys.CopyTo(arrNewValues, 0);

        foreach (var prop in props)
        {
            if (("System.String,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
            {
                if (!arrNewValues.Contains(prop.Name))
                {
                    e.NewValues[prop.Name] = prop.GetValue(project, null);
                }
            }
        }

        User editor = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

        e.NewValues["EditorUserId"] = editor.UserID.ToString();
        e.NewValues["EditedDate"] = DateTime.Now;
        //}
        //else
        //{
        //    ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
        //    e.Cancel = true;
        //}
    }

    protected void FormViewTlProject_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            // Display a user-friendly message 
            ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(e.Exception);
            // Indicate that the exception has been handled 
            e.ExceptionHandled = true;
            // Keep the current UI in edit mode 
            e.KeepInEditMode = true;
        }
        else
            ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Update successful");
    }

    #endregion

    protected void FormViewTlProject_DataBinding(object sender, EventArgs e)
    {
        string projectID = Page.RouteData.Values["project_id"] as string;

        if (projectID != null && projectID != "0")
        {
            Project project = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(projectID));
            this.editor = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(project.EditorUserID);
            this.creator = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(project.CreatorUserID);

            if (project.ProjectInfoTreeLocations.Count > 0)
                this.projectInfoTreeLocation = project.ProjectInfoTreeLocations.First();

            if (project.ProjectInfoes.Count > 0)
                this.projectInfo = project.ProjectInfoes.First();
        }
    }
}

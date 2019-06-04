 
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

public partial class Project_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    protected User editor;
    protected User creator;
    protected ProjectInfo projectInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        // your code that at one points sets the variable 
    }


    #region "Select handlers"

    protected void OdsProject_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewProject.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewProject.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewProject.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");

        if (formViewProject.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewProject_DataBound(object sender, System.EventArgs e)
    {

        if (Page.Request.UrlReferrer != null)
            if (Page.Request.UrlReferrer.AbsolutePath.Contains("new") && !IsPostBack)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Insert successful");
            }

        string projectID = Page.RouteData.Values["project_id"] as string;

        if (projectID != null && projectID != "0")
        {
            ProjectInfo projectInfoes = new ProjectInfoBLL().GetProjectInfoesByProjectID(Convert.ToInt32(projectID))[0];

            DropDownList ddlCity = (DropDownList)formViewProject.FindControl("ddlCity");
            if (ddlCity != null)
                ddlCity.SelectedValue = projectInfoes.CityID.ToString();
        }
    }

    #endregion

    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (formViewProject.CurrentMode == FormViewMode.Insert)
        {
            formViewProject.InsertItem(true);
        }
        else
        {
            formViewProject.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute(new { edit_mode = "edit", Project_id = RouteData.Values["Project_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/member/projects.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewProject_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        UserBLL userBLL = new UserBLL();

        TextBox txtProjectName = (TextBox)formViewProject.FindControl("txtProjectName");
        ProjectActionStatus status = Validate(txtProjectName.Text, actionType.insert);

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
        {
            UserBLL userBLL = new UserBLL();
            Eisk.BusinessEntities.User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            foreach (var item in Projects)
            {
                if (item.Group_Projects.First().GroupID == user.Group_Users.First().GroupID)
                    return ProjectActionStatus.Duplicate;
            }
            return ProjectActionStatus.Success;
        }
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
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
            Project project = new ProjectBLL().GetProjectByProjectID(result);

            UserBLL userBLL = new UserBLL();
            Eisk.BusinessEntities.User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            if (user != null)
            {
                // ProjectInfo
                TextBox txtProjectName = (TextBox)formViewProject.FindControl("txtProjectName");
                TextBox txtClient = (TextBox)formViewProject.FindControl("txtClient");
                TextBox txtAddress1 = (TextBox)formViewProject.FindControl("txtAddress1");
                TextBox txtAddress2 = (TextBox)formViewProject.FindControl("txtAddress2");
                DropDownList ddlCity = (DropDownList)formViewProject.FindControl("ddlCity");
                TextBox txtState = (TextBox)formViewProject.FindControl("txtState");
                TextBox txtZipCode = (TextBox)formViewProject.FindControl("txtZipCode");
                TextBox txtDescription = (TextBox)formViewProject.FindControl("txtDescription");
                TextBox txtComments = (TextBox)formViewProject.FindControl("txtComments");

                ProjectInfo projectInfo = new ProjectInfo();
                projectInfo.ProjectName = txtProjectName.Text;
                projectInfo.Client = txtClient.Text;
                projectInfo.Address1 = txtAddress1.Text;
                projectInfo.Address2 = txtAddress2.Text;
                projectInfo.CityID = Convert.ToInt32(ddlCity.SelectedValue);
                projectInfo.State = txtState.Text;
                projectInfo.ZipCode = txtZipCode.Text.Replace("-", "");
                projectInfo.Description = txtDescription.Text.Substring(0, Math.Min(txtDescription.Text.Length, 5000));
                projectInfo.Comments = txtComments.Text.Substring(0, Math.Min(txtComments.Text.Length, 5000));

                projectInfo.ProjectID = project.ProjectID;
                projectInfo.ProjectReference.EntityKey = project.EntityKey;

                new ProjectInfoBLL().CreateNewProjectInfo(projectInfo);
                //

                // Group_Projects
                List<Group> groupList = user.Group_Users.Select(instance => instance.Group).ToList();
                Group group = groupList[0];

                Group_Projects group_Projects = new Eisk.BusinessEntities.Group_Projects();

                group_Projects.GroupID = group.GroupID;
                group_Projects.GroupReference.EntityKey = group.EntityKey;

                group_Projects.ProjectID = project.ProjectID;
                group_Projects.ProjectReference.EntityKey = project.EntityKey;

                new Group_ProjectsBLL().CreateNewGroup_Projects(group_Projects);
                //
            }

            Response.RedirectToRoute("project-details", new { edit_mode = "edit", Project_id = result.ToString() });
        }
    }

    protected void formViewProject_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewProject_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtProjectName = (TextBox)formViewProject.FindControl("txtProjectName");
        ProjectActionStatus status = Validate(txtProjectName.Text, actionType.update);

        if (status == ProjectActionStatus.Success)
        {
            Project project = new ProjectBLL().GetProjectByProjectId2((int)e.Keys["ProjectId"]);

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                TextBox txtClient = (TextBox)formViewProject.FindControl("txtClient");
                TextBox txtAddress1 = (TextBox)formViewProject.FindControl("txtAddress1");
                TextBox txtAddress2 = (TextBox)formViewProject.FindControl("txtAddress2");
                DropDownList ddlCity = (DropDownList)formViewProject.FindControl("ddlCity");
                TextBox txtState = (TextBox)formViewProject.FindControl("txtState");
                TextBox txtZipCode = (TextBox)formViewProject.FindControl("txtZipCode");
                TextBox txtDescription = (TextBox)formViewProject.FindControl("txtDescription");
                TextBox txtComments = (TextBox)formViewProject.FindControl("txtComments");

                ProjectInfo projectInfo = _DatabaseContext.ProjectInfoes.First(instance => instance.ProjectID == project.ProjectID);
                projectInfo.ProjectName = txtProjectName.Text;
                projectInfo.Client = txtClient.Text;
                projectInfo.Address1 = txtAddress1.Text;
                projectInfo.Address2 = txtAddress2.Text;
                projectInfo.CityID = Convert.ToInt32(ddlCity.SelectedValue);
                projectInfo.State = txtState.Text;
                projectInfo.ZipCode = txtZipCode.Text.Replace("-", "");
                projectInfo.Description = txtDescription.Text.Substring(0, Math.Min(txtDescription.Text.Length, 5000));
                projectInfo.Comments = txtComments.Text.Substring(0, Math.Min(txtComments.Text.Length, 5000));

                _DatabaseContext.SaveChanges();
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
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    protected void formViewProject_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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

    protected void formViewProject_DataBinding(object sender, EventArgs e)
    {
        string projectID = Page.RouteData.Values["project_id"] as string;

        if (projectID != null && projectID != "0")
        {
            Project project = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(projectID));
            this.editor = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(project.EditorUserID);
            this.creator = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(project.CreatorUserID);
            this.projectInfo = project.ProjectInfoes.First();
        }
    }
}

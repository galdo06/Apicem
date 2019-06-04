/****************** Copyright Notice *****************
 
This code is licensed under Microsoft Public License (Ms-PL). 
You are free to use, modify and distribute any portion of this code. 
The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:

=======================================================
   
Architecture Designed and Implemented By:
Mohammad Ashraful Alam
Microsoft Most Valuable Professional, ASP.NET 2007 – 2011
Twitter: http://twitter.com/AshrafulAlam | Blog: http://blog.ashraful.net | Portfolio: http://www.ashraful.net
   
*******************************************************/
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

public partial class Project_Details : System.Web.UI.Page
{
    enum actionType
    {
        update,
        insert
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
        Response.RedirectToRoute("project-details", new { edit_mode = "edit", Project_id = RouteData.Values["Project_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/pages/projects.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewProject_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        TextBox txtProject = (TextBox)formViewProject.FindControl("txtProject");
        ProjectActionStatus status = Validate(txtProject.Text, actionType.insert);

        if (status == ProjectActionStatus.Success)
        {
            e.Values["CreatedDate"] = DateTime.Now;
            e.Values["EditedDate"] = DateTime.Now;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    private static ProjectActionStatus Validate(string scientificName, actionType act)
    {
        ProjectBLL scientificNameBLL = new ProjectBLL();
        List<Project> scientificNames = scientificNameBLL.GetProjectByFilter(scientificName.Trim());
        if (scientificNames.Count > 0 && act == actionType.insert)
            return ProjectActionStatus.Duplicate;
        else if (scientificNames.Count > 0 && scientificNames[0].ProjectName == scientificName && act == actionType.update)
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
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
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
        TextBox txtProject = (TextBox)formViewProject.FindControl("txtProject");
        ProjectActionStatus status = Validate(txtProject.Text, actionType.update);

        if (status == ProjectActionStatus.Success)
        {
            Type myType = (typeof(Project));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            ProjectBLL scientificNameBLL = new ProjectBLL();
            Project scientificName = scientificNameBLL.GetProjectByProjectId2((int)e.Keys["ProjectId"]);

            foreach (var prop in props)
            {
                if (("System.String,System.Int32,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                {
                    if (!arrNewValues.Contains(prop.Name))
                    {
                        e.NewValues[prop.Name] = prop.GetValue(scientificName, null);
                    }
                }
            }
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
}

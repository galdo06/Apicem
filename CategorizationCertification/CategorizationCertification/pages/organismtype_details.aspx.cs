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

public partial class OrganismType_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    #region "Select handlers"

    protected void OdsOrganismType_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewOrganismType.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewOrganismType.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewOrganismType.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");


        if (formViewOrganismType.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewOrganismType_DataBound(object sender, System.EventArgs e)
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
        if (formViewOrganismType.CurrentMode == FormViewMode.Insert)
        {
            formViewOrganismType.InsertItem(true);
        }
        else
        {
            formViewOrganismType.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("organismtype-details", new { edit_mode = "edit", organismtype2_id = RouteData.Values["organismtype2_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/pages/organismtypes.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewOrganismType_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        TextBox txtOrganismType = (TextBox)formViewOrganismType.FindControl("txtOrganismType");
        OrganismTypeActionStatus status = Validate(txtOrganismType.Text, actionType.insert);

        if (status == OrganismTypeActionStatus.Success)
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

    private static OrganismTypeActionStatus Validate(string organismType, actionType act)
    {
        OrganismTypeBLL organismTypeBLL = new OrganismTypeBLL();
        List<OrganismType> organismTypes = organismTypeBLL.GetOrganismTypeByFilter(organismType.Trim());
        if (organismTypes.Count > 0 && act == actionType.insert)
            return OrganismTypeActionStatus.Duplicate;
        else if (organismTypes.Count > 0 && organismTypes[0].OrganismTypeName == organismType && act == actionType.update)
            return OrganismTypeActionStatus.Duplicate;
        else
            return OrganismTypeActionStatus.Success;
    }

    public string GetErrorMessage(OrganismTypeActionStatus status)
    {
        switch (status)
        {
            case OrganismTypeActionStatus.Duplicate:
                return "Scientific Name address already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsOrganismType_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
            Response.RedirectToRoute("organismtype-details", new { edit_mode = "edit", organismtype2_id = result.ToString() });
        }
    }

    protected void formViewOrganismType_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewOrganismType_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtOrganismType = (TextBox)formViewOrganismType.FindControl("txtOrganismType");
        OrganismTypeActionStatus status = Validate(txtOrganismType.Text, actionType.update);

        if (status == OrganismTypeActionStatus.Success)
        {
            Type myType = (typeof(OrganismType));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            OrganismTypeBLL organismTypeBLL = new OrganismTypeBLL();
            OrganismType organismType = organismTypeBLL.GetOrganismTyeByOrganismTypeId2((int)e.Keys["OrganismTypeId"]);

            foreach (var prop in props)
            {
                if (("System.String,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                {
                    if (!arrNewValues.Contains(prop.Name))
                    {
                        e.NewValues[prop.Name] = prop.GetValue(organismType, null);
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

    protected void formViewOrganismType_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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

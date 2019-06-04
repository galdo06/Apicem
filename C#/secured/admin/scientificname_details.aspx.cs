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

public partial class ScientificName_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    protected User editor;
    protected User creator;
    protected ScientificName scientificName;

    #region "Select handlers"

    protected void OdsScientificName_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewScientificName.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewScientificName.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewScientificName.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");


        if (formViewScientificName.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewScientificName_DataBound(object sender, System.EventArgs e)
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
        if (formViewScientificName.CurrentMode == FormViewMode.Insert)
        {
            formViewScientificName.InsertItem(true);
        }
        else
        {
            formViewScientificName.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute(new { edit_mode = "edit", ScientificName_id = RouteData.Values["ScientificName_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/admin/scientificnames.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewScientificName_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        UserBLL userBLL = new UserBLL();

        TextBox txtScientificName = (TextBox)formViewScientificName.FindControl("txtScientificName");
        ScientificNameActionStatus status = Validate(txtScientificName.Text, actionType.insert);

        if (status == ScientificNameActionStatus.Success)
        {
            User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            e.Values["CreatedDate"] = DateTime.Now;
            e.Values["CreatorUserID"] = user.UserID;
            e.Values["EditedDate"] = DateTime.Now;
            e.Values["EditorUserId"] = user.UserID;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    private static ScientificNameActionStatus Validate(string scientificName, actionType act)
    {
        ScientificNameBLL scientificNameBLL = new ScientificNameBLL();
        List<ScientificName> scientificNames = scientificNameBLL.GetScientificNameByFilter(scientificName.Trim());
        if (scientificNames.Count > 0 && act == actionType.insert)
            return ScientificNameActionStatus.Duplicate;
        else if (scientificNames.Count > 0 && scientificNames[0].ScientificNameDesc == scientificName && act == actionType.update)
            return ScientificNameActionStatus.Duplicate;
        else
            return ScientificNameActionStatus.Success;
    }

    public string GetErrorMessage(ScientificNameActionStatus status)
    {
        switch (status)
        {
            case ScientificNameActionStatus.Duplicate:
                return "Scientific Name address already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsScientificName_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
            Response.RedirectToRoute(new { edit_mode = "edit", ScientificName_id = result.ToString() });
        }
    }

    protected void formViewScientificName_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewScientificName_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtScientificName = (TextBox)formViewScientificName.FindControl("txtScientificName");
        ScientificNameActionStatus status = Validate(txtScientificName.Text, actionType.update);

        if (status == ScientificNameActionStatus.Success)
        {
            Type myType = (typeof(ScientificName));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            ScientificNameBLL scientificNameBLL = new ScientificNameBLL();
            ScientificName scientificName = scientificNameBLL.GetScientificNameByScientificNameId2((int)e.Keys["ScientificNameId"]);

            foreach (var prop in props)
            {
                if (("System.String,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                {
                    if (!arrNewValues.Contains(prop.Name))
                    {
                        e.NewValues[prop.Name] = prop.GetValue(scientificName, null);
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

    protected void formViewScientificName_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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

    protected void formViewScientificName_DataBinding(object sender, EventArgs e)
    {
        string scientificNameID = Page.RouteData.Values["ScientificName_id"] as string;
        if (scientificNameID != null && scientificNameID != "0")
        {
            ScientificName scientificName = new ScientificNameBLL().GetScientificNameByScientificNameID(Convert.ToInt32(scientificNameID));
            this.editor = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(scientificName.EditorUserID);
            this.creator = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(scientificName.CreatorUserID);
            this.scientificName = scientificName;
        }
    }
}

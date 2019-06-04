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
using Eisk.Web.App_Logic.Helpers;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public partial class Group_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    protected User editor;
    protected User creator;
    protected Group group;

    #region "Select handlers"

    protected void OdsGroup_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewGroup.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewGroup.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewGroup.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");


        if (formViewGroup.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewGroup_DataBound(object sender, System.EventArgs e)
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
        if (formViewGroup.CurrentMode == FormViewMode.Insert)
        {
            formViewGroup.InsertItem(true);
        }
        else
        {
            formViewGroup.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute(new { edit_mode = "edit", Group_id = RouteData.Values["Group_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/admin/groups.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewGroup_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        UserBLL userBLL = new UserBLL();

        TextBox txtGroup = (TextBox)formViewGroup.FindControl("txtGroup");
        GroupActionStatus status = Validate(txtGroup.Text, actionType.insert);

        if (status == GroupActionStatus.Success)
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

    private static GroupActionStatus Validate(string scientificName, actionType act)
    {
        GroupBLL scientificNameBLL = new GroupBLL();
        List<Group> scientificNames = scientificNameBLL.GetGroupByFilter(scientificName.Trim());
        if (scientificNames.Count > 0 && act == actionType.insert)
            return GroupActionStatus.Duplicate;
        else if (scientificNames.Count > 0 && scientificNames[0].GroupName == scientificName && act == actionType.update)
            return GroupActionStatus.Duplicate;
        else
            return GroupActionStatus.Success;
    }

    public string GetErrorMessage(GroupActionStatus status)
    {
        switch (status)
        {
            case GroupActionStatus.Duplicate:
                return "Group address already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsGroup_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
            Response.RedirectToRoute(new { edit_mode = "edit", Group_id = result.ToString() });
        }
    }

    protected void formViewGroup_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewGroup_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtGroup = (TextBox)formViewGroup.FindControl("txtGroup");
        GroupActionStatus status = Validate(txtGroup.Text, actionType.update);

        if (status == GroupActionStatus.Success)
        {
            Type myType = (typeof(Group));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            GroupBLL groupBLL = new GroupBLL();
            Group group = groupBLL.GetGroupByGroupId2((int)e.Keys["GroupId"]);

            foreach (var prop in props)
            {
                if (("System.String,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                {
                    if (!arrNewValues.Contains(prop.Name))
                    {
                        e.NewValues[prop.Name] = prop.GetValue(group, null);
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

    protected void formViewGroup_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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

    protected void formViewGroup_DataBinding(object sender, EventArgs e)
    {
        string groupID = Page.RouteData.Values["Group_id"] as string;
        if (groupID != null && groupID != "0")
        {
            Group group = new GroupBLL().GetGroupByGroupID(Convert.ToInt32(groupID));
            this.editor = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(group.EditorUserID);
            this.creator = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(group.CreatorUserID);
            this.group = group;
        }
    }
}

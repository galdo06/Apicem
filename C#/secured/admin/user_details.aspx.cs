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
using Eisk.DataAccessLayer;

public partial class User_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    protected User editor;
    protected User creator;
    protected User user;

    #region "Select handlers"

    protected void OdsUser_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewUser.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewUser.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewUser.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");


        if (formViewUser.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewUser_DataBound(object sender, System.EventArgs e)
    {
        if (Page.Request.UrlReferrer != null)
        {
            if (Page.Request.UrlReferrer.AbsolutePath.Contains("new") && !IsPostBack)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Insert successful");
            }
        }

        string userID = Page.RouteData.Values["User_id"] as string;
        if (userID != null && userID != "0")
        {
            List<Role_Users> role_Users = new Role_UsersBLL().GetRole_UsersByUserID(new Guid(userID));
            List<Group_Users> group_Users = new Group_UsersBLL().GetGroup_UsersByUserID(new Guid(userID));

            DropDownList ddlRoles = (DropDownList)formViewUser.FindControl("ddlRoles");
            if (ddlRoles != null)
                ddlRoles.SelectedValue = role_Users[0].RoleID.ToString();

            DropDownList ddlGroups = (DropDownList)formViewUser.FindControl("ddlGroups");
            if (ddlGroups != null)
                ddlGroups.SelectedValue = group_Users[0].GroupID.ToString();
        }
    }

    #endregion

    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (formViewUser.CurrentMode == FormViewMode.Insert)
        {
            formViewUser.InsertItem(true);
        }
        else
        {
            formViewUser.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute(new { edit_mode = "edit", User_id = RouteData.Values["User_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/admin/users.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    private static UserActionStatus Validate(string user, actionType act)
    {
        List<User> users = new UserBLL().GetUsersByFilter(user.Trim(), null, 0, 1);
        if (users.Count > 0 && act == actionType.insert)
            return UserActionStatus.Duplicate;
        else
            return UserActionStatus.Success;
    }

    public string GetErrorMessage(UserActionStatus status)
    {
        switch (status)
        {
            case UserActionStatus.Duplicate:
                return "User Name already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    #endregion

    #region "Update handlers"

    protected void FormViewUser_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtUser = (TextBox)formViewUser.FindControl("txtUser");
        UserActionStatus status = Validate(txtUser.Text, actionType.update);

        if (status == UserActionStatus.Success)
        {
            User user = new UserBLL().GetUserByUserID((Guid)e.Keys["UserId"]);

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                int roleID = Convert.ToInt32(((DropDownList)formViewUser.FindControl("ddlRoles")).SelectedValue);
                int groupID = Convert.ToInt32(((DropDownList)formViewUser.FindControl("ddlGroups")).SelectedValue);

                Role_Users role_Users = _DatabaseContext.Role_Users.First(instance => instance.UserID == user.UserID);
                Role role = _DatabaseContext.Roles.First(instance => instance.RoleID == roleID);
                role_Users.RoleID = role.RoleID;
                role_Users.RoleReference.EntityKey = role.EntityKey;

                Group_Users group_Users = _DatabaseContext.Group_Users.First(instance => instance.UserID == user.UserID);
                Group group = _DatabaseContext.Groups.First(instance => instance.GroupID == groupID);
                group_Users.GroupID = group.GroupID;
                group_Users.GroupReference.EntityKey = group.EntityKey;

                _DatabaseContext.SaveChanges();
            }

            Type myType = (typeof(User));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            foreach (var prop in props)
            {
                if (("System.String,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                {
                    if (!arrNewValues.Contains(prop.Name))
                    {
                        e.NewValues[prop.Name] = prop.GetValue(user, null);
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

    protected void formViewUser_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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

    protected void formViewUser_DataBinding(object sender, EventArgs e)
    {
        string userID = Page.RouteData.Values["User_id"] as string;
        if (userID != null && userID != "0")
        {
            User user = new UserBLL().GetUserByUserID(new Guid(userID));
            this.editor = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(user.EditorUserID);
            this.creator = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(user.CreatorUserID);
            this.user = user;
        }
    }
}

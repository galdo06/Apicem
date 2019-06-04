
using System;
using System.Web.UI.WebControls;
using Eisk.Helpers;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Web;
using System.Web.Security;
using Eisk.Web.App_Logic.Helpers;
using System.Reflection;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;


public partial class UserInfo_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    #region "Select handlers"

    protected void OdsUserInfo_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        UserInfoBLL userInfoBLL = new UserInfoBLL();
        string UserName = (Membership.GetUser()).UserName;
        UserInfo userInfo = userInfoBLL.GetCurrentUserUserInfo(UserName);


        e.InputParameters["UserName"] = UserName;

        string editMode = Page.RouteData.Values["edit_mode"] as string;
        if (userInfo == null && editMode == "view")
            Response.RedirectToRoute("userinfo-details", new { edit_mode = "new", userinfo_id = default(Guid) });
        if (userInfo != null && editMode == null)
            Response.RedirectToRoute("userinfo-details", new { edit_mode = "view", userinfo_id = userInfo.UserID });

        // string lola = Page.GetRouteUrl("userinfo-details", new { edit_mode = "view", userinfo_id = userInfo.UserID }); 
        // if (userInfo == null && editMode == "new")
        //     Response.RedirectToRoute("userinfo-details", new { edit_mode = "new", userinfo_id = default(Guid) });

        if (editMode == "view")
            formViewUserInfo.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewUserInfo.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewUserInfo.ChangeMode(FormViewMode.Insert);
        //else
        //    throw new InvalidOperationException("error");

        if (formViewUserInfo.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewUserInfo_DataBound(object sender, System.EventArgs e)
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
        if (formViewUserInfo.CurrentMode == FormViewMode.Insert)
        {
            formViewUserInfo.InsertItem(true);
        }
        else
        {
            formViewUserInfo.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("userinfo-details", new { edit_mode = "edit", UserInfo_id = RouteData.Values["UserInfo_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("userinfo-details", new { edit_mode = "view", UserInfo_id = RouteData.Values["UserInfo_id"] });
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewUserInfo_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        UserBLL userBLL = new UserBLL();
        User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

        e.Values["EditedDate"] = DateTime.Now;
        e.Values["UserID"] = user.UserID;
    }

    public string GetErrorMessage(UserInfoActionStatus status)
    {
        switch (status)
        {
            case UserInfoActionStatus.Duplicate:
                return "Información de Usuario address already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsUserInfo_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

        if (e.ReturnValue != null)
        {
            Response.RedirectToRoute("userinfo-details", new { edit_mode = "view", userinfo_id = user.UserID });
        }
    }

    protected void formViewUserInfo_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewUserInfo_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
        UserInfo userInfo = new UserInfoBLL().GetUserInfoesByUserID(user.UserID)[0];

        Type myType = (typeof(UserInfo));
        PropertyInfo[] props = myType.GetProperties();

        string[] arrNewValues = new string[e.NewValues.Keys.Count];
        e.NewValues.Keys.CopyTo(arrNewValues, 0);

        foreach (var prop in props)
        {
            if (("System.String,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                if (!arrNewValues.Contains(prop.Name))
                    e.NewValues[prop.Name] = prop.GetValue(userInfo, null);
        }

        TextBox txtZipCode = (TextBox)formViewUserInfo.FindControl("txtZipCode");
        TextBox txtPhone = (TextBox)formViewUserInfo.FindControl("txtPhone");
        TextBox txtCellPhone = (TextBox)formViewUserInfo.FindControl("txtCellPhone");
        TextBox txtFax = (TextBox)formViewUserInfo.FindControl("txtFax");

        e.NewValues["ZipCode"] = Utility.ReplaceZipCodePhoneCharacters(txtZipCode.Text);
        e.NewValues["Phone"] = Utility.ReplaceZipCodePhoneCharacters(txtPhone.Text);
        e.NewValues["CellPhone"] = Utility.ReplaceZipCodePhoneCharacters(txtCellPhone.Text);
        e.NewValues["Fax"] = Utility.ReplaceZipCodePhoneCharacters(txtFax.Text);

        e.NewValues["EditedDate"] = DateTime.Now;

        e.NewValues["UserInfoID"] = userInfo.UserInfoID;
    }


    protected void formViewUserInfo_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        TextBox txtCellPhone = (TextBox)formViewUserInfo.FindControl("txtCellPhone");
        TextBox txtFax = (TextBox)formViewUserInfo.FindControl("txtFax");

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

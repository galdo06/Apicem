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
using System.Web.Security;

public partial class CreateUser : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        ((DropDownList)sender).SelectedIndex = 1;
    }

    #region "Select handlers"

    #endregion

    #region "Command Handlers"

    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute(new { edit_mode = "edit", user_id = RouteData.Values["user_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/admin/users.aspx");
    }

    #endregion

    #region "Insert handlers"

    #endregion

    #region "Update handlers"


    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        UserBLL userBLL = new UserBLL();
        RoleBLL roleBLL = new RoleBLL();
        GroupBLL groupBLL = new GroupBLL();

        string UserName = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName")).Text;
        int roleID = Convert.ToInt32(((DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlRoles")).SelectedValue);
        int groupID = Convert.ToInt32(((DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlGroups")).SelectedValue);

        Role role = roleBLL.GetRoleByRoleID(roleID);
        User user = userBLL.GetUserByUserName_WithoutApplication(UserName);
        Group group = groupBLL.GetGroupByGroupID(groupID);

        roleBLL.AddUserToRole(user, role);
        groupBLL.AddUserToGroup(user, group);

        Label lblEmail = (Label)CompleteWizardStep1.ContentTemplateContainer.FindControl("lblEmail");
        Label lblUserType = (Label)CompleteWizardStep1.ContentTemplateContainer.FindControl("lblUserType");
        Label lblGroup = (Label)CompleteWizardStep1.ContentTemplateContainer.FindControl("lblGroup");

        lblEmail.Text = UserName;
        lblUserType.Text = ((DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlRoles")).SelectedItem.Text;
        lblGroup.Text = ((DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlGroups")).SelectedItem.Text;

        Response.Redirect("~/secured/admin/users.aspx", true);

    }

    #endregion

    protected void CreateUserWizard1_CreateUserError(object sender, CreateUserErrorEventArgs e)
    {
        Literal ltlMessage = (Literal)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ltlMessage");
        ltlMessage.Text =
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(e.CreateUserError));
    }

    public string GetErrorMessage(MembershipCreateStatus status)
    {
        switch (status)
        {
            case MembershipCreateStatus.DuplicateUserName:
                return "E-mail address already exists. Please enter a different e-mail address.";

            case MembershipCreateStatus.DuplicateEmail:
                return "E-mail address already exists. Please enter a different e-mail address.";

            case MembershipCreateStatus.InvalidPassword:
                return "The password provided is invalid. Please enter a valid password value.";

            case MembershipCreateStatus.InvalidEmail:
                return "The e-mail address provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidAnswer:
                return "The password retrieval answer provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidQuestion:
                return "The password retrieval question provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.InvalidUserName:
                return "The user name provided is invalid. Please check the value and try again.";

            case MembershipCreateStatus.ProviderError:
                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            case MembershipCreateStatus.UserRejected:
                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

            default:
                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        }
    }

}

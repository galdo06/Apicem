 
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

public partial class ChangePassword_Details : System.Web.UI.Page
{ 
    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        var user = Membership.GetUser();
        if (txtNewPassword.Text != txtNewPassword2.Text)
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(ChangePasswordActionStatus.NewPasswordsMissmatch));
            return;
        }

        if (!user.ChangePassword(txtActualPassword.Text, txtNewPassword.Text)) 
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(ChangePasswordActionStatus.OldPasswordIncorrect));
            return;
        }

        ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Contraseña cambiada satisfactoriamente");
    }

    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("ActualPassword-details", new { edit_mode = "edit", organism_id = RouteData.Values["organism_id"], NewPassword2_id = Page.RouteData.Values["NewPassword2_id"] as string, ActualPassword = "edit" });
    }

    protected void ButtonSelect_Click(object sender, EventArgs e)
    {
        //TextBox txtActualPassword = (TextBox)formViewActualPassword.FindControl("txtActualPassword");
        //string commoName = string.IsNullOrWhiteSpace(txtActualPassword.Text) || txtActualPassword.Text == "new" ? "new" : txtActualPassword.Text;

        //int NewPassword2_id;
        //NewPassword2_id = int.TryParse(Page.RouteData.Values["NewPassword2_id"] as string, out NewPassword2_id) ? NewPassword2_id : 0;

        //Response.RedirectToRoute("NewPassword2select", new { edit_mode = "edit", organism_id = RouteData.Values["organism_id"], NewPassword2_id = NewPassword2_id, ActualPassword = commoName });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/secured/member/userinfo_details.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    public string GetErrorMessage(ChangePasswordActionStatus status)
    {
        switch (status)
        {
            case ChangePasswordActionStatus.NewPasswordsMissmatch:
                return "Las Nuevas contraseñas no son iguales. Favor de verificarlas";
            case ChangePasswordActionStatus.OldPasswordIncorrect:
                return "La Contraseña Actual es incorrecta";
            default:
                return "Error";
        }
    }
      
    #endregion

}

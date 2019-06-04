using System;
using System.Linq;
using Eisk.Helpers;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using CL.App_Logic.Entity_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.DataAccessLayer;
using Eisk.Helpers;
using Eisk.BusinessLogicLayer;

public partial class Public_Log_On : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated)
        {
            SiteLogin.RedirectToDefaultPage();
            return;
        }
        if (!IsPostBack)
        {
            var divSocialInterest = (HtmlGenericControl)Login1.FindControl("divSocialInterest");
            divSocialInterest.InnerHtml = new TermsAndConditionBLL().GetAllTermsAndConditions().OrderByDescending(i => i.CreatedDate).FirstOrDefault().TermsAndConditionsInnerText;
        }
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        bool IsValidate = Membership.ValidateUser(Login1.UserName, Login1.Password);

        if (!IsValidate)
        {
            e.Authenticated = false;
            labelMessage.Text = MessageFormatter.GetFormattedNoticeMessage("The username and/or password cannot be validated or has not been approved yet.");
        }
        else
        {
            e.Authenticated = true;
        }
    }

    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
        if (Request.QueryString["Url"] != null)
        {
            Response.Redirect("~/" + Request.QueryString["Url"].ToString());
        }
        else
        {
            Response.Redirect("../admin/users.aspx", true);
        }
    }

}

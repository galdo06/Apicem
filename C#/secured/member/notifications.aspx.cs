using Eisk.BusinessLogicLayer;
using Eisk.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eisk.Web
{
    public partial class Notification : System.Web.UI.Page
    { 
        protected void ButtonAddNewNotification_Click(object sender, System.EventArgs e)
        {
            string projectID = (this.RouteData.Values["project_id"] as string);
            Response.RedirectToRoute("Notification-details", new { edit_mode = "new", project_id = projectID, organism_id = 0, scientificname_id = 0, Notification = "new" });
        }

        protected void odsNotificationListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
            var group = user.Group_Users.Select(instance => instance.Group).ToList();
            e.InputParameters["groupID"] = group[0].GroupID;
            e.InputParameters["userID"] = user.UserID.ToString();
        }

        protected void odsCommonNameListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["organismTypeID"] = 2;
            Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
            var group = user.Group_Users.Select(instance => instance.Group).ToList();
            e.InputParameters["groupID"] = group[0].GroupID;
        }

        protected void gridViewNotifications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text.ToUpper() == "DESCONOCIDO")
                {
                    Panel pnlView = (Panel)e.Row.FindControl("pnlView");
                    Panel pnlEdit = (Panel)e.Row.FindControl("pnlEdit");
                    Panel pnlCheckBox = (Panel)e.Row.FindControl("pnlCheckBox");
                    pnlView.Visible = false;
                    pnlEdit.Visible = false;
                    pnlCheckBox.Visible = false;
                    e.Row.Cells[1].ForeColor = ColorTranslator.FromHtml("#A349A4");
                    e.Row.Cells[1].Font.Bold = true;
                }
            }
        }

        protected void odsNotificationListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {

            txtFilter.Focus();
        }
    }
}
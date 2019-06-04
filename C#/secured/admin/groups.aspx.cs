using Eisk.BusinessLogicLayer;
using Eisk.DataAccessLayer;
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
    public partial class Group : System.Web.UI.Page
    {
        protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Create a List to hold the GroupID values to delete
                List<Int32> GroupIDsToDelete = new List<Int32>();

                // Iterate through the Groups.Rows property
                foreach (GridViewRow row in gridViewGroups.Rows)
                {

                    // Access the CheckBox
                    CheckBox cb = (CheckBox)(row.FindControl("chkGroupSelector"));
                    if (cb != null && cb.Checked)
                    {
                        // Save the GroupID value for deletion
                        // First, get the GroupID for the selected row
                        Int32 GroupID = (Int32)gridViewGroups.DataKeys[row.RowIndex].Value;
                        GroupBLL groupBLL = new GroupBLL();
                        Eisk.BusinessEntities.Group group = groupBLL.GetGroupByGroupID(GroupID);

                        // Add it to the List...
                        GroupIDsToDelete.Add(GroupID);

                        // Add a confirmation message
                        ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. Group <b>{0}</b> has been deleted"), group.GroupName);

                    }
                }

                //perform the actual delete
                new GroupBLL().DeleteGroups(GroupIDsToDelete);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
            }

            //binding the grid
            gridViewGroups.PageIndex = 0;
            gridViewGroups.DataBind();

        }

        protected void ButtonAddNewGroup_Click(object sender, System.EventArgs e)
        {
            Response.RedirectToRoute("group-details", new { edit_mode = "new", group_id = 0 });
        }

        protected void gridViewGroups_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected int GetGroupUsersCount(int groupID)
        {
            using (var context = new DatabaseContext())
            {
                return context.Groups.First(instance => instance.GroupID == groupID).Group_Users.Count;
            }
        }

        protected void odsGroupListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            txtFilter.Focus();
        }
    }
}
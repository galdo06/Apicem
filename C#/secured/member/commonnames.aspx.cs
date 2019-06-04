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
    public partial class CommonName : System.Web.UI.Page
    {
        protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Create a List to hold the OrganismID values to delete
                List<Int32> OrganismIDsToDelete = new List<Int32>();

                // Iterate through the Organisms.Rows property
                foreach (GridViewRow row in gridViewCommonNames.Rows)
                {

                    // Access the CheckBox
                    CheckBox cb = (CheckBox)(row.FindControl("chkCommonNameSelector"));
                    if (cb != null && cb.Checked)
                    {
                        // Save the OrganismID value for deletion
                        // First, get the OrganismID for the selected row
                        Int32 OrganismID = (Int32)gridViewCommonNames.DataKeys[row.RowIndex].Value;
                        OrganismBLL commonNameBLL = new OrganismBLL();
                        Eisk.BusinessEntities.Organism organism = commonNameBLL.GetOrganismByOrganismID(OrganismID);

                        // Add it to the List...
                        OrganismIDsToDelete.Add(OrganismID);

                        // Add a confirmation message
                        ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. Organism <b>{0}</b> has been deleted"), organism.CommonName.CommonNameDesc);

                    }
                }

                //perform the actual delete
                new OrganismBLL().DeleteOrganisms(OrganismIDsToDelete);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
            }

            //binding the grid
            gridViewCommonNames.PageIndex = 0;
            gridViewCommonNames.DataBind();

        }

        protected void ButtonAddNewCommonName_Click(object sender, System.EventArgs e)
        {
            string projectID = (this.RouteData.Values["project_id"] as string);
            Response.RedirectToRoute("commonname-details", new { edit_mode = "new", project_id = projectID, organism_id = 0, scientificname_id = 0, commonname = "new" });
        }

        protected void odsCommonNameListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["organismTypeID"] = 2;
            Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
            var group = user.Group_Users.Select(instance => instance.Group).ToList();
            e.InputParameters["groupID"] = group[0].GroupID;
        }

        protected void gridViewCommonNames_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void odsCommonNameListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {

            txtFilter.Focus();
        }
    }
}
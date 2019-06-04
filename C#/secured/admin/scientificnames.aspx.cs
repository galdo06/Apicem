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
    public partial class ScientificName : System.Web.UI.Page
    {
        protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Create a List to hold the ScientificNameID values to delete
                List<Int32> ScientificNameIDsToDelete = new List<Int32>();

                // Iterate through the ScientificNames.Rows property
                foreach (GridViewRow row in gridViewScientificNames.Rows)
                {

                    // Access the CheckBox
                    CheckBox cb = (CheckBox)(row.FindControl("chkScientificNameSelector"));
                    if (cb != null && cb.Checked)
                    {
                        // Save the ScientificNameID value for deletion
                        // First, get the ScientificNameID for the selected row
                        Int32 ScientificNameID = (Int32)gridViewScientificNames.DataKeys[row.RowIndex].Value;
                        ScientificNameBLL scientificNameBLL = new ScientificNameBLL();
                        Eisk.BusinessEntities.ScientificName scientificName = scientificNameBLL.GetScientificNameByScientificNameID(ScientificNameID);

                        // Add it to the List...
                        ScientificNameIDsToDelete.Add(ScientificNameID);

                        // Add a confirmation message
                        ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. Scientific Name <b>{0}</b> has been deleted"), scientificName.ScientificNameDesc);

                    }
                }

                //perform the actual delete
                new ScientificNameBLL().DeleteScientificNames(ScientificNameIDsToDelete);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
            }

            //binding the grid
            gridViewScientificNames.PageIndex = 0;
            gridViewScientificNames.DataBind();

        }

        protected void ButtonAddNewScientificName_Click(object sender, System.EventArgs e)
        {
            string projectID = (this.RouteData.Values["project_id"] as string);

            Response.RedirectToRoute("scientificname-details", new { edit_mode = "new", project_id = projectID, scientificname_id = 0 });
        }

        protected void gridViewScientificNames_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void odsScientificNameListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            txtFilter.Focus();
        }
    }
}
using Eisk.BusinessLogicLayer;
using Eisk.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

public partial class OrganismType_Page : System.Web.UI.Page
{
    protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
    {
        try
        {
            // Create a List to hold the OrganismTypeID values to delete
            List<Int32> OrganismTypeIDsToDelete = new List<Int32>();

            // Iterate through the OrganismTypes.Rows property
            foreach (GridViewRow row in gridViewOrganismTypes.Rows)
            {

                // Access the CheckBox
                CheckBox cb = (CheckBox)(row.FindControl("chkOrganismTypeSelector"));
                if (cb != null && cb.Checked)
                {
                    // Save the OrganismTypeID value for deletion
                    // First, get the OrganismTypeID for the selected row
                    Int32 OrganismTypeID = (Int32)gridViewOrganismTypes.DataKeys[row.RowIndex].Value;
                    OrganismTypeBLL OrganismTypeBLL = new OrganismTypeBLL();
                    Eisk.BusinessEntities.OrganismType OrganismType = OrganismTypeBLL.GetOrganismTypeByOrganismTypeID(OrganismTypeID);

                    // Add it to the List...
                    OrganismTypeIDsToDelete.Add(OrganismTypeID);

                    // Add a confirmation message
                    ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. Organism Type <b>{0}</b> has been deleted"), OrganismType.OrganismTypeName);

                }
            }

            //perform the actual delete
            new OrganismTypeBLL().DeleteOrganismTypes(OrganismTypeIDsToDelete);
        }
        catch (Exception ex)
        {
            ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
        }

        //binding the grid
        gridViewOrganismTypes.PageIndex = 0;
        gridViewOrganismTypes.DataBind();

    }

    protected void ButtonAddNewOrganismType_Click(object sender, System.EventArgs e)
    {
        Response.RedirectToRoute("OrganismType-details", new { edit_mode = "new", organismtype2_id = 0 });
    }

    protected void gridViewOrganismTypes_RowDataBound(object sender, GridViewRowEventArgs e)
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
}
using System;
using System.Web.UI.WebControls;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Threading;
using Eisk.Helpers;
using Eisk.BusinessEntities;

public partial class Organisms_Page : System.Web.UI.Page
{
    protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
    {
        try
        {
            // Create a List to hold the OrganismID values to delete
            List<Int32> OrganismIDsToDelete = new List<Int32>();

            // Iterate through the Organisms.Rows property
            foreach (GridViewRow row in gridViewOrganisms.Rows)
            {

                // Access the CheckBox
                CheckBox cb = (CheckBox)(row.FindControl("chkOrganismSelector"));
                if (cb != null && cb.Checked)
                {
                    // Save the OrganismID value for deletion
                    // First, get the OrganismID for the selected row
                    Int32 OrganismID = (Int32)gridViewOrganisms.DataKeys[row.RowIndex].Value;
                    OrganismBLL scientificNameBLL = new OrganismBLL();
                    Eisk.BusinessEntities.Organism organism = scientificNameBLL.GetOrganismByOrganismID(OrganismID);

                    // Add it to the List...
                    OrganismIDsToDelete.Add(OrganismID);

                    // Add a confirmation message
                    ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. <b>{0}</b>(<b>{1}</b>) has been deleted"), organism.CommonName, organism.ScientificName);

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
        gridViewOrganisms.PageIndex = 0;
        gridViewOrganisms.DataBind();

    }

    public string GetOrganismTypeName(int organismTypeID)
    {
        OrganismTypeBLL organismTypeBLL = new OrganismTypeBLL();
        OrganismType organismType = organismTypeBLL.GetOrganismTypeByOrganismTypeID(organismTypeID);
        return organismType.OrganismTypeName;
    }

    protected void ButtonAddNewOrganism_Click(object sender, System.EventArgs e)
    {
        Response.RedirectToRoute("organism-details", new { edit_mode = "new", organism_id = 0, organismtype_id = 0 });
    }

    protected void ddlOrganismType_SelectedIndexChanged(object sender, EventArgs e)
    {
        odsOrganismListing.Select();
    }
}

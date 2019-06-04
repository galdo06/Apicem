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
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Threading;
using Eisk.Helpers;
using Eisk.BusinessEntities;

public partial class OrganismSelect_Page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string projectID = (Page.RouteData.Values["project_id"] as string);
        this.hfProjectID.Value = projectID;

        ProjectBLL projectBLL = new ProjectBLL();

        Project project = projectBLL.GetProjectByProjectID(Convert.ToInt32(projectID));

        lblProjectName.Text = project.ProjectName;
    }

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

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("organismsinproject", new { project_id = (Page.RouteData.Values["project_id"] as string) });
    }

    protected void gridViewOrganisms_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            ProjectBLL projectBLL = new ProjectBLL();
            Project project = projectBLL.GetProjectByProjectID(Convert.ToInt32(Page.RouteData.Values["project_id"]));

            OrganismBLL organismBLL = new OrganismBLL();
            Organism organism = organismBLL.GetOrganismByOrganismID(Convert.ToInt32(e.CommandArgument));

            Project_OrganismsBLL project_OrganismsBLL = new Project_OrganismsBLL();
            Project_Organisms project_Organism = new Project_Organisms();

            project_Organism.ProjectID = project.ProjectID;
            project_Organism.ProjectReference.EntityKey = project.EntityKey;

            project_Organism.OrganismID = organism.OrganismID;
            project_Organism.OrganismReference.EntityKey = organism.EntityKey;

            project_Organism.CreatedDate = DateTime.Now;
            project_Organism.EditedDate = DateTime.Now;

            project_OrganismsBLL.CreateNewProject_Organisms(project_Organism);

            Response.RedirectToRoute("organismsinproject", new { project_id = (Page.RouteData.Values["project_id"] as string) });
        }
    }

    protected void odsOrganismListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["projectID"] = (Page.RouteData.Values["project_id"] as string);

    }

    protected void ddlOrganismType_SelectedIndexChanged(object sender, EventArgs e)
    {
        odsOrganismListing.Select();
    }
}

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

public partial class Projects_Page : System.Web.UI.Page
{

    protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
    {
        try
        {
            // Create a List to hold the ProjectID values to delete
            List<Int32> ProjectIDsToDelete = new List<Int32>();

            // Iterate through the Projects.Rows property
            foreach (GridViewRow row in gridViewProjects.Rows)
            {

                // Access the CheckBox
                CheckBox cb = (CheckBox)(row.FindControl("chkProjectSelector"));
                if (cb != null && cb.Checked)
                {
                    // Save the ProjectID value for deletion
                    // First, get the ProjectID for the selected row
                    Int32 ProjectID = (Int32)gridViewProjects.DataKeys[row.RowIndex].Value;
                    ProjectBLL scientificNameBLL = new ProjectBLL();
                    Eisk.BusinessEntities.Project project = scientificNameBLL.GetProjectByProjectID(ProjectID);
                   
                    // Add it to the List...
                    ProjectIDsToDelete.Add(ProjectID);

                    // Add a confirmation message
                    ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. Project <b>{0}</b> has been deleted"), project.ProjectName);

                }
            }

            //perform the actual delete
            new ProjectBLL().DeleteProjects(ProjectIDsToDelete);
        }
        catch (Exception ex)
        {
            ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
        }

        //binding the grid
        gridViewProjects.PageIndex = 0;
        gridViewProjects.DataBind();

    }
    
    protected void ButtonAddNewProject_Click(object sender, System.EventArgs e)
   {
       Response.RedirectToRoute("project-details", new { edit_mode = "new", project_id = 0 });
   }
}

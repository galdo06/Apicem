using Eisk.BusinessEntities;
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
    public partial class Project : System.Web.UI.Page
    {
        protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
        {
            try
            {
                List<Eisk.BusinessEntities.Project> ProjectsToDelete = new List<Eisk.BusinessEntities.Project>();

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
                        ProjectBLL projectBLL = new ProjectBLL();
                        Eisk.BusinessEntities.Project project = projectBLL.GetProjectByProjectID(ProjectID);

                        ProjectsToDelete.Add(project);

                        // Add a confirmation message
                        ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. Project <b>{0}</b> has been deleted"), project.ProjectInfoes.First().ProjectName);

                    }
                }

              new ProjectBLL().DeleteProjectsAndObjects(ProjectsToDelete);
               
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

        protected void gridViewProjects_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected string GetProjectName(int projectID)
        {
          return  new ProjectInfoBLL().GetProjectInfoesByProjectID(projectID).FirstOrDefault().ProjectName;
        }

        protected void odsProjectListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            UserBLL userBLL = new UserBLL();
            Eisk.BusinessEntities.User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);
            if (user != null)
            {
                var group = user.Group_Users.Select(instance => instance.Group).ToList();
                e.InputParameters["groupID"] = group[0].GroupID;
            }
        }

        protected void btnTreeLocation_Click(object sender, System.EventArgs e)
        {
            Eisk.BusinessEntities.User editor = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

            int projectID = Convert.ToInt32(((Button)sender).CommandArgument);

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                Eisk.BusinessEntities.Project project = _DatabaseContext.Projects.First(instance => instance.ProjectID == projectID);
                project.EditorUserID = editor.UserID;
                project.EditedDate = DateTime.Now;

                _DatabaseContext.SaveChanges();
            }

            Response.RedirectToRoute("tl-project-details", new { edit_mode = "view", project_id = ((Button)sender).CommandArgument });
        }

        protected void btnCategorizationCertification_Click(object sender, System.EventArgs e)
        {
            // Response.RedirectToRoute("tl-project-details", new { edit_mode = "view", project_id = RouteData.Values["Project_id"] as string });
        }

        protected void odsProjectListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            txtFilter.Focus();
        }

    }
}
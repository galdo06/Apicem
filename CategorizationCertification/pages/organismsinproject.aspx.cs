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
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Threading;
using Eisk.Helpers;
using Eisk.BusinessEntities;
using Eisk.Web.App_Logic.Business_Logic_Layer;
using System.Text;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

public partial class OrganismsInProject_Page : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string projectID = (Page.RouteData.Values["project_id"] as string);
        this.hfProjectID.Value = projectID;

        ProjectBLL projectBLL = new ProjectBLL();
        Project project = projectBLL.GetProjectByProjectID(Convert.ToInt32(projectID));
        project.EditedDate = DateTime.Now;
        projectBLL.UpdateProject(project);

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
                    Project_OrganismsBLL project_OrganismsBLL = new Project_OrganismsBLL();
                    Eisk.BusinessEntities.Project_Organisms project_Organisms = project_OrganismsBLL.GetProject_OrganismByProjectIDOrganismID(Convert.ToInt32((Page.RouteData.Values["project_id"] as string)), OrganismID);

                    // Add it to the List...
                    OrganismIDsToDelete.Add(project_Organisms.ProjectOrganismID);

                    // Add a confirmation message
                    ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Delete successful. <b>{0}</b> - <b>{1}</b>(<b>{2}</b>) has been deleted"), project_Organisms.Organism.OrganismType.OrganismTypeName, project_Organisms.Organism.CommonName, project_Organisms.Organism.ScientificName);

                }
            }

            //perform the actual delete
            new Project_OrganismsBLL().DeleteProject_Organisms(OrganismIDsToDelete);
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
        Response.RedirectToRoute("organismselect", new { project_id = hfProjectID.Value });
    }

    protected void odsOrganismListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["projectID"] = (Page.RouteData.Values["project_id"] as string);
    }

    protected void ddlOrganismType_SelectedIndexChanged(object sender, EventArgs e)
    {
        odsOrganismListing.Select();
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute("projects");
    }

    protected void btnExportData_Click(object sender, EventArgs e)
    {
        string projectID = (Page.RouteData.Values["project_id"] as string);
        List<ExportData> ExportDataList = new OrganismBLL().GetExportData("", Convert.ToInt32(projectID));
        List<OrganismType> OrganismTypes = new OrganismTypeBLL().GetAllOrganismTypesList();
        Project project = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(projectID));

        string name = Translate(project.ProjectName);
        name = Translate(name);

        string path = System.Web.HttpContext.Current.Server.MapPath(@System.Configuration.ConfigurationManager.AppSettings["ProjectsRoot"]) + projectID.ToString();

        // check folder exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            System.IO.File.WriteAllText(path + "/" + name + ".xlsx", "");
        }

        FileInfo newFile = new FileInfo(path + @"\" + name + ".xlsx");
        File.Delete(path + @"\" + name + ".xlsx");
        using (ExcelPackage pck = new ExcelPackage(newFile))
        {
            foreach (OrganismType OrganismType in OrganismTypes)
            {
                List<ExportData> OrganismTypeList = ExportDataList.Where(instance => instance.OrganismTypeID == OrganismType.OrganismTypeID).ToList();

                if (OrganismTypeList.Count > 0)
                {
                    //Add the Content sheet
                    var ws = pck.Workbook.Worksheets.Add(OrganismType.OrganismTypeName);

                    ws.Cells["A1"].Value = OrganismType.OrganismTypeName;
                    ws.Cells["A1"].Style.Font.Bold = true;
                    ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A1:B1"].Merge = true;
                    ws.Cells["A1:B1"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    
                    //Headers
                    ws.Cells["A2"].Value = "Common Name";
                    ws.Cells["A2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    ws.Cells["B2"].Value = "Scientific Name";
                    ws.Cells["B2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    ws.Cells["A2:B2"].Style.Font.Bold = true;

                    ws.Column(1).Width = 35.00d;
                    ws.Column(2).Width = 35.00d;

                    int row = 3;
                    foreach (ExportData Organism in OrganismTypeList)
                    {
                        ws.Cells["A" + row].Value = Organism.CommonName;
                        ws.Cells["A" + row].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells["A" + row].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        ws.Cells["B" + row].Value = Organism.ScientificName;
                        ws.Cells["B" + row].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        row++;
                    }

                    ws.Cells["A3:B" + (row - 1)].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                }
            }
            pck.Save();
            pck.Dispose();
        }

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.ContentType = "application/vnd.ms-excel";
        response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".xlsx");
        response.TransmitFile(path + @"/" + name + ".xlsx");
        response.End();

    }

    private static readonly Char[] ReplacementChars = new[] { 'á', 'é', 'í', 'ó', 'ü', 'ú', 'ñ', 'Á', 'É', 'Í', 'Ó', 'Ú', 'Ü', 'Ñ' };
    private static readonly Dictionary<Char, Char> ReplacementMappings = new Dictionary<Char, Char> 
                                                               { 
                                                                 { 'á', 'a'}, 
                                                                 { 'é', 'e'}, 
                                                                 { 'í', 'i'}, 
                                                                 { 'ó', 'o'}, 
                                                                 { 'ü', 'u'}, 
                                                                 { 'ú', 'u'}, 
                                                                 { 'ñ', 'n'}, 
                                                                 { 'Á', 'A'}, 
                                                                 { 'É', 'E'}, 
                                                                 { 'Í', 'I'}, 
                                                                 { 'Ó', 'O'}, 
                                                                 { 'Ü', 'U'}, 
                                                                 { 'Ú', 'U'}, 
                                                                 { 'Ñ', 'N'} 
                                                                };

    private static string Translate(String source)
    {
        var startIndex = 0;
        var currentIndex = 0;
        var result = new StringBuilder(source.Length);

        while ((currentIndex = source.IndexOfAny(ReplacementChars, startIndex)) != -1)
        {
            result.Append(source.Substring(startIndex, currentIndex - startIndex));
            result.Append(ReplacementMappings[source[currentIndex]]);

            startIndex = currentIndex + 1;
        }

        if (startIndex == 0)
            return source.ToString().Replace("-", "").Replace(@"\", "").Replace(@"/", "").Replace("?", "").Trim();

        result.Append(source.Substring(startIndex));

        return result.ToString().Replace("-", "").Replace(@"\", "").Replace(@"/", "").Replace("?", "").Trim();
    }

}

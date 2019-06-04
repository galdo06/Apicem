using CL.App_Logic.Helpers;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tl_Report_Details_CSV : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string projectID = (Page.RouteData.Values["project_id"] as string);

        string repsIdsToCreate = (Page.RouteData.Values["reports"] as string);

        // Validar 1 o más arboles
        List<Project_Organisms> project_Organisms = new List<Project_Organisms>();

        if (repsIdsToCreate == "a")
            project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(projectID));
        else if (repsIdsToCreate == "b")
            project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(projectID)).Where(i => i.TreeDetails.First().ActionProposedID == 2 || i.TreeDetails.First().ActionProposedID == 2).ToList();
        else
            return;

        Eisk.BusinessEntities.Project project = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(projectID));

        string name = Reports.Translate(project.ProjectInfoes.First().ProjectName).Replace(" ","_");

         if (repsIdsToCreate == "b")
            name += "(Proteccion_Poda)"; 

        string path = System.Web.HttpContext.Current.Server.MapPath(@System.Configuration.ConfigurationManager.AppSettings["ProjectsRoot"]) + projectID.ToString();

        // check folder exists
        if (File.Exists(path))
            File.Delete(path);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            System.IO.File.WriteAllText(path + "/" + name + ".csv", "");
        }

        FileInfo newFile = new FileInfo(path + @"\" + name + ".csv");
        File.Delete(path + @"\" + name + ".csv");

        foreach (Project_Organisms project_Organism in project_Organisms)
        {
            AddTreeToAutoCADImportFile(project_Organism, path + @"\" + name + ".csv");
        }

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.ContentType = "application/ms-excel";
        response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
        response.WriteFile(path + @"/" + name + ".csv");

        response.End();
    }

    public static void AddTreeToAutoCADImportFile(Project_Organisms project_Organism, string path)
    {
        try
        {
            TreeDetail treeDetail = project_Organism.TreeDetails.First();

            string toWrite =
                treeDetail.Number
                + ","
                + String.Format("{0:0.####################################}", treeDetail.Y)
                + ","
                + String.Format("{0:0.####################################}", treeDetail.X)
                + ",0,"
                + project_Organism.Organism.CommonName.CommonNameDesc;

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(toWrite);
                writer.Flush();
                writer.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error on AddTree method: " + ex.Message);
        }
    }
}

//response.TransmitFile(path + @"/" + name + ".xlsx");

//response.ContentType  =  "application/octet-stream" 
//response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"") 
//response.Clear() 

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

public partial class Tl_Report_Details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string projectID = (Page.RouteData.Values["project_id"] as string);

        List<string> repsIdsToCreate = (Page.RouteData.Values["reports"] as string).Split(',').ToList();

        List<Rep> repsToCreate = new List<Rep>();
        foreach (string repID in repsIdsToCreate)
        {
            repsToCreate.Add(new RepBLL().GetRepByRepID(Convert.ToInt32(repID)));
        }

        bool isFullHeader = Convert.ToBoolean(Page.RouteData.Values["headeroneachreport"] as string);// cbHeaderOnEachReport.Checked;

        Eisk.BusinessEntities.Project project = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(projectID));
        Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
        Eisk.BusinessEntities.UserInfo userInfo = user.UserInfoes.First(instance => instance.UserID == user.UserID);

        DateTime time = DateTime.Now;

        string name = Reports.Translate(project.ProjectInfoes.First().ProjectName);

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
            ProjectInfo projectInfo = new ProjectInfoBLL().GetProjectInfoesByProjectID(Convert.ToInt32(projectID)).First();
            List<Project_Organisms> project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(projectID));

            int totalPages = 0;
            int currentPageNumber = 0;
            int maxLines = Convert.ToInt32(@System.Configuration.ConfigurationManager.AppSettings["reportsMaxLines"]);

            List<TreeDetail> treeDetails = new List<TreeDetail>();
            List<TreeDetail> treeComentaries = new List<TreeDetail>();
            List<TreesSummary> actionProposedID0Items = new List<TreesSummary>();
            List<TreesSummary> actionProposedID1Items = new List<TreesSummary>();
            List<TreesSummary> actionProposedID2Items = new List<TreesSummary>();
            List<TreesSummary> actionProposedID3Items = new List<TreesSummary>();
            List<TreesSummary> actionProposedID4Items = new List<TreesSummary>();

            int treeDetailsCount = 0;
            int actionProposedID0ItemsCount = 0;
            int actionProposedID1ItemsCount = 0;
            int actionProposedID2ItemsCount = 0;
            int actionProposedID3ItemsCount = 0;
            int actionProposedID4ItemsCount = 0;

            List<Index> idexes = new List<Index>();

            if (repsToCreate.Select(instance => instance.RepID).Contains(1))
            {
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 1).Select(instance => instance.RepName).First(),
                         1,
                         totalPages + 1,
                         totalPages + 1
                    ));
                totalPages += 1;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(2))
            {
                treeDetails = new TreeDetailBLL().GetTreeDetailsByProjectID(Convert.ToInt32(projectID));
                treeDetailsCount += Reports.GetPageCountOrDefault(maxLines, treeDetails.Count);
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 2).Select(instance => instance.RepName).First(),
                        2,
                        totalPages + 1,
                         totalPages + treeDetailsCount
                    ));
                totalPages += treeDetailsCount;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(3))
            {
                actionProposedID0Items = new TreesSummaryBLL().GetItems(project_Organisms, 0, true);
                actionProposedID0ItemsCount = Reports.GetPageCountOrDefault(maxLines, actionProposedID0Items.Count);
                ;
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 3).Select(instance => instance.RepName).First(),
                        3,
                        totalPages + 1,
                         totalPages + actionProposedID0ItemsCount
                    ));
                totalPages += actionProposedID0ItemsCount;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(4))
            {
                actionProposedID1Items = new TreesSummaryBLL().GetItems(project_Organisms, 1, true);
                actionProposedID1ItemsCount += Reports.GetPageCountOrDefault(maxLines, actionProposedID1Items.Count);
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 4).Select(instance => instance.RepName).First(),
                        4,
                        totalPages + 1,
                         totalPages + actionProposedID1ItemsCount
                    ));
                totalPages += actionProposedID1ItemsCount;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(5))
            {
                actionProposedID2Items = new TreesSummaryBLL().GetItems(project_Organisms, 2, true);
                actionProposedID2ItemsCount += Reports.GetPageCountOrDefault(maxLines, actionProposedID2Items.Count);
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 5).Select(instance => instance.RepName).First(),
                        5,
                        totalPages + 1,
                         totalPages + actionProposedID2ItemsCount
                    ));
                totalPages += actionProposedID2ItemsCount;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(6))
            {
                actionProposedID3Items = new TreesSummaryBLL().GetItems(project_Organisms, 3, true);
                actionProposedID3ItemsCount += Reports.GetPageCountOrDefault(maxLines, actionProposedID3Items.Count);
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 6).Select(instance => instance.RepName).First(),
                        6,
                        totalPages + 1,
                         totalPages + actionProposedID3ItemsCount
                    ));
                totalPages += actionProposedID3ItemsCount;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(7))
            {
                actionProposedID4Items = new TreesSummaryBLL().GetItems(project_Organisms, 4, true);
                actionProposedID4ItemsCount += Reports.GetPageCountOrDefault(maxLines, actionProposedID4Items.Count);
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 7).Select(instance => instance.RepName).First(),
                        7,
                        totalPages + 1,
                         totalPages + actionProposedID4ItemsCount
                    ));
                totalPages += actionProposedID4ItemsCount;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(8))
            {
                idexes.Add(
                    new Index(
                        repsToCreate.Where(instance => instance.RepID == 8).Select(instance => instance.RepName).First(),
                         1,
                         totalPages + 1,
                         totalPages + 1
                    ));
                totalPages += 1;
            }
            if (repsToCreate.Select(instance => instance.RepID).Contains(2)) // Se repite el ID de inventario de arboles para anejar comentarios muy largos 
            {
                treeComentaries = treeDetails.AsQueryable().DynamicOrderBy("Number").Where(instance => instance.Commentary.Trim().Length > 100).ToList();

                int totalTreeDetailsLines = 0;
                int pageCount = 0;
                foreach (var treeDetail in treeComentaries)
                {
                    int lines = (int)Math.Ceiling((double)treeDetail.Commentary.Length / 200D);
                    if (totalTreeDetailsLines + lines > maxLines * pageCount)
                    {
                        pageCount++;
                    }

                    totalTreeDetailsLines += lines;
                }

                if (treeComentaries.Count > 0)
                {
                    idexes.Add(
                        new Index(
                            "Comentarios (Continuación)",
                            0,
                            totalPages + 1,
                             totalPages + pageCount
                        ));
                }
                //int pageCount = (int)Math.Ceiling((double)totalTreeDetailsLines / (double)maxLines);
                totalPages += pageCount;
            }

            bool hasIndex = Convert.ToBoolean(Page.RouteData.Values["createindex"] as string);
            if (hasIndex)//cbCreateIndex.Checked)
            {
                Reports.Index(isFullHeader, currentPageNumber, totalPages, "Tabla de Contenido", project, userInfo, idexes, time, pck);
            }

            foreach (Int32 reportID in repsToCreate.Select(instance => instance.RepID))
            {
                switch (reportID)
                {
                    case 1:
                        {
                            Reports.ProjectInfo(isFullHeader, hasIndex, currentPageNumber, totalPages, project, userInfo, time, pck);
                            currentPageNumber += 1;
                        }
                        break;
                    case 2:
                        {
                            Reports.TreeInventory(isFullHeader, hasIndex, currentPageNumber, totalPages, project, userInfo, treeDetails.AsQueryable().DynamicOrderBy("Number").ToList(), time, pck, maxLines);
                            currentPageNumber += treeDetailsCount;
                        }
                        break;
                    case 3:
                        {// actionProposedID = 0; ALL
                            Reports.TreesSummary(isFullHeader, hasIndex, currentPageNumber, totalPages, project, userInfo, actionProposedID0Items, time, pck, maxLines);
                            currentPageNumber += actionProposedID0ItemsCount;
                        }
                        break;
                    case 4:
                        {// actionProposedID = 1; Corte y Remoción
                            Reports.ActionProposedSummary(isFullHeader, hasIndex, currentPageNumber, totalPages, "Resumen de Corte y Remoción", project, userInfo, actionProposedID1Items, time, pck, maxLines);
                            currentPageNumber += actionProposedID1ItemsCount;
                        }
                        break;
                    case 5:
                        {// actionProposedID = 2; Protección
                            Reports.ActionProposedSummary(isFullHeader, hasIndex, currentPageNumber, totalPages, "Resumen de Protección", project, userInfo, actionProposedID2Items, time, pck, maxLines);
                            currentPageNumber += actionProposedID2ItemsCount;
                        }
                        break;
                    case 6:
                        {// actionProposedID = 3; Poda
                            Reports.ActionProposedSummary(isFullHeader, hasIndex, currentPageNumber, totalPages, "Resumen de Poda", project, userInfo, actionProposedID3Items, time, pck, maxLines);
                            currentPageNumber += actionProposedID3ItemsCount;
                        }
                        break;
                    case 7:
                        {// actionProposedID = 4; Transplante
                            Reports.ActionProposedSummary(isFullHeader, hasIndex, currentPageNumber, totalPages, "Resumen de Transplante", project, userInfo, actionProposedID4Items, time, pck, maxLines);
                            currentPageNumber += actionProposedID4ItemsCount;
                        }
                        break;
                    case 8:
                        {
                            using (ExcelPackage pck2 = new OfficeOpenXml.ExcelPackage())
                            {
                                ExcelWorksheet wsTemplate = null;
                                pck2.Load(File.OpenRead( System.Web.HttpContext.Current.Server.MapPath(@"~\App_Resources\Excel\Totales.xlsx")));
                                wsTemplate = pck2.Workbook.Worksheets.First();
                                Reports.ProjectResults(isFullHeader, hasIndex, currentPageNumber, totalPages, project_Organisms, project, userInfo, time, pck, wsTemplate);
                                currentPageNumber += 1;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            if (treeComentaries.Count > 0 && treeDetailsCount > 0)
            {
                Reports.Comentaries(isFullHeader, hasIndex, currentPageNumber, totalPages, "Comentarios (Continuación)", project, userInfo, treeComentaries, time, pck, maxLines);
            }

            pck.Save();
            pck.Dispose();
        }

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.ContentType = "application/ms-excel";
        response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".xlsx");
        response.WriteFile(path + @"/" + name + ".xlsx");

        response.End();
    }
}

//response.TransmitFile(path + @"/" + name + ".xlsx");

//response.ContentType  =  "application/octet-stream" 
//response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"") 
//response.Clear() 

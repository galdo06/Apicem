using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using Eisk.Helpers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using CL.App_Logic.Helpers;
using Eisk.DataAccessLayer;
using System.Web.UI;
using System.Threading;

namespace Eisk.Web
{
    public partial class Report : System.Web.UI.Page
    {
        protected string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            // scriptManager.RegisterPostBackControl(this.lbtGenerateReports);
        }

        protected void ButtonGoToViewPage_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("tl", new { Project_id = RouteData.Values["Project_id"] });
        }

        protected void lbtGenerateReports_Click(object sender, EventArgs e)
        {
            ltlMessage.Text = "";

            try
            {
                //  Create a List to hold the ProjectID values to delete
                List<Rep> repsToCreate = new List<Rep>();
                StringBuilder sbReports = new StringBuilder();

                // Iterate through the Projects.Rows property
                foreach (GridViewRow row in gridViewReports.Rows)
                {
                    // Access the CheckBox
                    CheckBox cb = (CheckBox)(row.FindControl("chkReportSelector"));
                    if (cb != null && cb.Checked)
                    {
                        Rep rep = new RepBLL().GetRepByRepID((Int32)gridViewReports.DataKeys[row.RowIndex].Value);
                        repsToCreate.Add(rep);
                        sbReports.Append(rep.RepID + ",");
                    }
                }

                // Al menos un reporte seleccionado
                if (repsToCreate.Count == 0)
                {
                    ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Generación de reportes cancelada. Seleccione al menos un reporte para generar.");
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " hideDiv(); ", false);
                    return;
                }

                // Validar 1 o más arboles
                string projectID = (Page.RouteData.Values["project_id"] as string);
                List<Project_Organisms> project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(projectID));
                if (project_Organisms.Count == 0)
                {
                    ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Generación de reportes cancelada. Entre al menos un arbol al proyecto para permitir la generación de reportes.");
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " hideDiv(); ", false);
                    return;
                }

                // Validar Desconocido y/o Accion Propuesta Det Luego
                string message;
                ValidateItems(project_Organisms, out message);
                if (!(message == ""))
                {
                    ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Generación de reportes cancelada. Al menos un arbol " + message);
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " hideDiv(); ", false);
                    return;
                }

                System.Threading.Thread.Sleep(3000);

                hfReports.Value = Page.GetRouteUrl("tl-report-details",
                    new
                    {
                        edit_mode = "edit",
                        project_id = RouteData.Values["Project_id"],
                        headeroneachreport = cbHeaderOnEachReport.Checked,
                        createindex = cbCreateIndex.Checked,
                        reports = sbReports.ToString().Substring(0, sbReports.ToString().Length - 1)
                    });

                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " EndRequest(null,null); ", false);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
            }

        }

        protected void gridViewReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        private static void ValidateItems(List<Project_Organisms> project_Organisms, out string message)
        {
            List<TreesSummary> itemsDeterminarLuegos = new TreesSummaryBLL().GetItems(project_Organisms, 5, true);
            List<TreesSummary> itemsDescoconidos = new TreesSummaryBLL().GetItems(project_Organisms, 0, false);

            message = "";
            if (itemsDeterminarLuegos.Count > 0)
                message += "con acción propuesta " + '"' + "Determinar Luego" + '"' + "";

            if (itemsDeterminarLuegos.Count > 0 && itemsDescoconidos.Count > 0)
                message += " y al menos un arbol ";
            else if (itemsDeterminarLuegos.Count > 0 && itemsDescoconidos.Count == 0)
                message += ".";

            if (itemsDescoconidos.Count > 0)
                message += "con nombre común " + '"' + "Descoconido" + '"' + ".";

            if (itemsDeterminarLuegos.Count == 0 && itemsDescoconidos.Count == 0)
                message += String.Empty;
        }


        protected void lbGenerateCSV_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
            string projectID = new Page().RouteData.Values["project_id"] as string;

            // Validar 1 o más arboles
            List<Project_Organisms> project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(projectID));
            if (project_Organisms.Count == 0)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Generación de reportes cancelada. Entre al menos un arbol al proyecto para permitir la generación de reportes.");
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " hideDiv(); ", false);
                return;
            }
            
            hfReports.Value = Page.GetRouteUrl("tl-report-details-csv",
                new
                {
                    edit_mode = "edit",
                    project_id = RouteData.Values["Project_id"], 
                    reports = "a" 
                });

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " EndRequest(null,null); ", false);
        }

        
        protected void lbGenerateCSVProtec_Click(object sender, EventArgs e)
        {
            string projectID = new Page().RouteData.Values["project_id"] as string;

            // Validar 1 o más arboles
            List<Project_Organisms> project_OrganismsProtecPoda = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(Convert.ToInt32(projectID)).Where(i => i.TreeDetails.First().ActionProposedID == 2 || i.TreeDetails.First().ActionProposedID == 2).ToList();
            if (project_OrganismsProtecPoda.Count == 0)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Generación de reportes cancelada. Entre al menos un arbol al proyecto para permitir la generación de reportes.");
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " hideDiv(); ", false);
                return;
            }

            hfReports.Value = Page.GetRouteUrl("tl-report-details-csv",
                new
                {
                    edit_mode = "edit",
                    project_id = RouteData.Values["Project_id"],
                    reports = "b"
                });

            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", " EndRequest(null,null); ", false);
        }
    }
}
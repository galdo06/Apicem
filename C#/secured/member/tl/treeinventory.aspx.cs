using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using Eisk.DataAccessLayer;
using Eisk.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eisk.Web
{
    public partial class TreeInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" var editorID = '" + (new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name)).UserID + "'; ");
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", sb.ToString(), true);

            ScriptManager.GetCurrent(this).RegisterPostBackControl(lbFilter);
            if (!Page.IsPostBack)
            {
                ddlActionProposed.Value = Request.QueryString["ac"] ?? ddlActionProposed.Value;
                ddlOrderBy.SelectedValue = Request.QueryString["ob"] ?? ddlOrderBy.SelectedValue;

                if (!string.IsNullOrEmpty(Request.QueryString["r"]))
                    rbReverse.Checked = Request.QueryString["r"] == "1";

                if (!string.IsNullOrEmpty(Request.QueryString["c"]))
                    cbCepas.Checked = Request.QueryString["c"] == "1";

                if (!string.IsNullOrEmpty(Request.QueryString["l"]))
                    chkLittoral.Checked = Request.QueryString["l"] == "1";

                if (!string.IsNullOrEmpty(Request.QueryString["m"]))
                    chkMaritimeZone.Checked = Request.QueryString["m"] == "1";

                if (!string.IsNullOrEmpty(Request.QueryString["mr"]))
                {
                    int x;
                    if (int.TryParse(Request.QueryString["mr"].ToString(), out x))
                    {
                        gridViewTreeDetails.PageSize = x;
                        ddlPageSize.SelectedValue = x.ToString();
                    }
                }

                txtFilter.Text = Request.QueryString["f"] ?? txtFilter.Text;
            }

            SetDdlsColors(ddlActionProposed);
            SetDdlsColors(ddlActionProposedChange);
        }

        private void SetDdlsColors(System.Web.UI.HtmlControls.HtmlSelect ddl)
        {

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                List<ActionProposed> acts = _DatabaseContext.ActionProposeds.ToList();
                foreach (ListItem item in ddl.Items)
                {
                    if (item.Value == "0")
                        item.Attributes.Add("style", "background-color:#FFF");
                    else
                        item.Attributes.Add("style", "background-color:#" + acts.Where(i => i.ActionProposedID.ToString() == item.Value).First().Color.Code);
                }

                if (ddlActionProposed.SelectedIndex == 0)
                    ddlActionProposed.Attributes.Add("style", "background-color:#FFF");
                else
                    ddlActionProposed.Attributes.Add("style", "background-color:#" + acts.Where(i => i.ActionProposedID.ToString() == ddlActionProposed.Items[ddlActionProposed.SelectedIndex].Value).First().Color.Code);
            }
        }
        private void SetDdlsColors(DropDownList ddl)
        {

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                List<ActionProposed> acts = _DatabaseContext.ActionProposeds.ToList();
                foreach (ListItem item in (ddl as DropDownList).Items)
                {
                    if (item.Value == "0")
                        item.Attributes.Add("style", "background-color:#FFF");
                    else
                        item.Attributes.Add("style", "background-color:#" + acts.Where(i => i.ActionProposedID.ToString() == item.Value).First().Color.Code);
                }

            }
        }
        protected bool ProjectCenterSet()
        {
            string projectID = (Page.RouteData.Values["project_id"] as string);
            List<ProjectInfoTreeLocation> projectInfoTreeLocation = new ProjectInfoTreeLocationBLL().GetProjectInfoTreeLocationsByProjectID(Convert.ToInt32(projectID));
            return (projectInfoTreeLocation.Count > 0 && projectInfoTreeLocation.First().X != 0);
        }

        protected void ButtonDeleteSelected_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Create a List to hold the ProjectID values to delete
                List<Int32> TreeDetailIDsToDelete = new List<Int32>();
                List<Int32> ProjectOrganismsIDsToDelete = new List<Int32>();
                List<Int32> DapIDsToDelete = new List<Int32>();

                // Iterate through the Projects.Rows property
                foreach (GridViewRow row in gridViewTreeDetails.Rows)
                {

                    // Access the CheckBox
                    CheckBox cb = (CheckBox)(row.FindControl("chkTreeDetailsSelector"));
                    if (cb != null && cb.Checked)
                    {
                        // Save the ProjectID value for deletion
                        // First, get the ProjectID for the selected row
                        Int32 treeDetailsID = (Int32)gridViewTreeDetails.DataKeys[row.RowIndex].Value;
                        Eisk.BusinessEntities.TreeDetail treeDetail = new TreeDetailBLL().GetTreeDetailByTreeDetailsID(treeDetailsID);

                        List<Int32> tempDaps = new DapBLL().GetDapsByTreeDetailsID(treeDetailsID).Select(instance => instance.DapID).ToList();

                        // Add it to the List...
                        ProjectOrganismsIDsToDelete.Add(treeDetail.ProjectOrganismID);
                        TreeDetailIDsToDelete.Add(treeDetail.TreeDetailsID);
                        DapIDsToDelete.AddRange(tempDaps);
                    }
                }

                using (DatabaseContext _DatabaseContext = new DatabaseContext())
                {


                    foreach (int dapID in DapIDsToDelete)
                    {
                        Dap dap = _DatabaseContext.Daps.First(instance => instance.DapID == dapID);
                        _DatabaseContext.Daps.DeleteObject(dap);

                        _DatabaseContext.SaveChanges();
                    }
                    foreach (int treeDetailsID in TreeDetailIDsToDelete)
                    {
                        TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.TreeDetailsID == treeDetailsID);
                        _DatabaseContext.TreeDetails.DeleteObject(treeDetail);

                        _DatabaseContext.SaveChanges();
                    }
                    foreach (int ProjectOrganismID in ProjectOrganismsIDsToDelete)
                    {
                        Project_Organisms project_Organism = _DatabaseContext.Project_Organisms.First(instance => instance.ProjectOrganismID == ProjectOrganismID);
                        _DatabaseContext.Project_Organisms.DeleteObject(project_Organism);

                        _DatabaseContext.SaveChanges();
                    }

                    //Sort Numbers after deleting
                    int projectIDINT = Convert.ToInt32(RouteData.Values["Project_id"]);
                    List<TreeDetail> treeDetails =
                        _DatabaseContext.TreeDetails
                            .Where(instance => instance.Project_Organisms.ProjectID == projectIDINT)
                            .OrderBy(instance => instance.Number)
                            .ToList();

                    int number = 1;
                    foreach (TreeDetail tDetail in treeDetails)
                    {
                        tDetail.Number = number;
                        number++;
                    }
                    //

                    _DatabaseContext.SaveChanges();
                }

                // Add a confirmation message
                ltlMessage.Text += String.Format(MessageFormatter.GetFormattedSuccessMessage("Eliminación de árboles satisfactoria. <b>{0}</b> árboles eliminados"), ProjectOrganismsIDsToDelete.Count);
            }
            catch (Exception ex)
            {
                ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(ex);
            }

            //binding the grid
            gridViewTreeDetails.PageIndex = 0;
            gridViewTreeDetails.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gridViewTreeDetails.DataBind();
        }

        protected void ButtonGoToViewPage_Click(object sender, EventArgs e)
        {
            Response.RedirectToRoute("tl", new { Project_id = RouteData.Values["Project_id"] });
        }

        protected void ButtonAddNewTreeDetails_Click(object sender, System.EventArgs e)
        {
            ProjectInfoTreeLocation ProjectInfoTreeLocation = new ProjectInfoTreeLocationBLL().GetProjectInfoTreeLocationsByProjectID(Convert.ToInt32(RouteData.Values["Project_id"]))[0];
            int count = new Project_OrganismsBLL().GetTotalCountForAllProject_OrganismsByProjectID(Convert.ToInt32(RouteData.Values["Project_id"]));

            Response.RedirectToRoute("tl-tree-details", new
            {
                edit_mode = "new",
                Project_id = RouteData.Values["Project_id"],
                project_organism_id = 0,
                number = count + 1,
                x = ProjectInfoTreeLocation.X,
                y = ProjectInfoTreeLocation.Y,
                lat = ProjectInfoTreeLocation.Lat,
                lon = ProjectInfoTreeLocation.Lon
            });
        }

        protected void gridViewTreeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCommonName = (Label)e.Row.FindControl("lblCommonName");
                Label lblScientificName = (Label)e.Row.FindControl("lblScientificName");
                if (lblCommonName.Text.ToUpper() == "DESCONOCIDO")
                {
                    lblScientificName.ForeColor = ColorTranslator.FromHtml("#A349A4");
                    lblScientificName.Font.Bold = true;
                    lblCommonName.ForeColor = ColorTranslator.FromHtml("#A349A4");
                    lblCommonName.Font.Bold = true;
                }

                //DropDownList ddlActionProposed = (DropDownList)e.Row.FindControl("ddlActionProposed");
                //var actions = new DatabaseContext().ActionProposeds.ToList();
                //foreach (ListItem item in ddlActionProposed.Items)
                //{
                //    item.Attributes.Add("style", "background-color:#" + actions.Where(i => i.ActionProposedID.ToString() == item.Value).First().Color.Code);
                //}
            }
        }

        protected void odsTreeDetailsListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["projectID"] = RouteData.Values["Project_id"];

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["sri"]))
                {
                    e.InputParameters["startRowIndex"] = Convert.ToInt32(Request.QueryString["sri"]);
                    e.Arguments.StartRowIndex = Convert.ToInt32(Request.QueryString["sri"]);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["mr"]))
                {
                    int x;
                    if (int.TryParse(Request.QueryString["mr"].ToString(), out x))
                    {
                        e.InputParameters["maximumRows"] = Convert.ToInt32(Request.QueryString["mr"]);
                        e.Arguments.MaximumRows = Convert.ToInt32(Request.QueryString["mr"]);
                    }

                }
                if (!string.IsNullOrEmpty(Request.QueryString["pi"]))
                {
                    gridViewTreeDetails.PageIndex = Convert.ToInt32(Request.QueryString["pi"]);

                }

            }
        }

        protected void rblActionProposed_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int actionProposedID = Convert.ToInt32(rb.ID.Substring(rb.ID.LastIndexOf("_") + 1, rb.ID.Length - rb.ID.LastIndexOf("_") - 1)) + 1;
            int projectOrganismID = Convert.ToInt32(rb.GroupName.Substring(rb.GroupName.LastIndexOf("_") + 1, rb.GroupName.Length - rb.GroupName.LastIndexOf("_") - 1));
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.ProjectOrganismID == projectOrganismID);
                ActionProposed actionProposed = _DatabaseContext.ActionProposeds.First(instance => instance.ActionProposedID == actionProposedID);
                treeDetail.ActionProposedID = actionProposed.ActionProposedID;
                treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;

                _DatabaseContext.SaveChanges();
            }
        }

        protected void ddlActionProposedChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            int actionProposedChangeID = Convert.ToInt32(ddlActionProposedChange.SelectedValue);

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                foreach (GridViewRow row in gridViewTreeDetails.Rows)
                {
                    // Save the ProjectID value for deletion
                    // First, get the ProjectID for the selected row
                    Int32 treeDetailsID = (Int32)gridViewTreeDetails.DataKeys[row.RowIndex].Value;
                    TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.TreeDetailsID == treeDetailsID);
                    ActionProposed actionProposed = _DatabaseContext.ActionProposeds.First(instance => instance.ActionProposedID == actionProposedChangeID);
                    treeDetail.ActionProposedID = actionProposed.ActionProposedID;
                    treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;

                    _DatabaseContext.SaveChanges();
                }
            }
            ddlActionProposedChange.SelectedIndex = 0;
        }

        protected void lbFilter_Click(object sender, EventArgs e)
        {

            gridViewTreeDetails.PageIndex = 0;
            gridViewTreeDetails.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gridViewTreeDetails.DataBind();


            txtFilter.Focus();
        }

        protected void odsTreeDetailsListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            txtFilter.Focus();
        }

        protected string getColor(ActionProposed ap)
        {
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                return _DatabaseContext.Colors.Where(i => i.ColorID == ap.ColorID).First().Code;
            }
        }

        protected void gridViewTreeDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "lb")
            {
                string project_organism_id = ((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).CommandArgument;

                var project_organism = new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID(Convert.ToInt32(project_organism_id));
                var treeDetails = project_organism.TreeDetails.First();

                string x = Page.GetRouteUrl("tl-tree-details", new
                  {
                      edit_mode = "edit",
                      Project_id = project_organism.ProjectID.ToString(),
                      project_organism_id = project_organism.ProjectOrganismID.ToString(),
                      number = treeDetails.Number.ToString(),
                      x = treeDetails.X.ToString(),
                      y = treeDetails.Y.ToString(),
                      lat = treeDetails.Lat.ToString(),
                      lon = treeDetails.Lon.ToString()
                  });
                x +=
                      "?s=i"
                    + "&ac=" + ddlActionProposed.Value as string
                    + "&ob=" + ddlOrderBy.SelectedValue as string
                    + "&r=" + (rbReverse.Checked ? "1" : "0") as string
                    + "&c=" + (cbCepas.Checked ? "1" : "0") as string
                    + "&l=" + (chkLittoral.Checked ? "1" : "0") as string
                    + "&m=" + (chkMaritimeZone.Checked ? "1" : "0") as string
                    + "&sri=" + (gridViewTreeDetails.PageIndex * gridViewTreeDetails.PageSize).ToString()
                    + "&mr=" + gridViewTreeDetails.PageSize as string
                    + "&pi=" + gridViewTreeDetails.PageIndex as string
                    + (!string.IsNullOrEmpty(txtFilter.Text) ? "&f=" + txtFilter.Text as string : "");

                Response.Redirect(x);
            }
        }

        protected void ddlActionProposedChange_DataBound(object sender, EventArgs e)
        {
            SetDdlsColors(sender as DropDownList);
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridViewTreeDetails.PageSize = Convert.ToInt32(((DropDownList)sender).SelectedValue);

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            pnlCloneRange.Style["display"] = (rbtCloneRange.Checked) ? "block" : "none";

            pnlCloneError.Style["display"] = "none";
            txtCloneFrom.Text = "";
            txtCloneTo.Text = "";
            txtCloneBase.Text = "";

            lblNumber.Text = "";
            lblNombreComun.Text = "";
            lblVaras.Text = "";
            lblDap.Text = "";
            lblAltura.Text = "";
            lblAcciónPropuesta.Text = "";
            lblCondicion.Text = "";
            lblLitoral.Text = "";
            lblMaritimoTerrestre.Text = "";

            chkCloneNombre.Checked = true;
            chkCloneVaras.Checked = true;
            chkCloneDap.Checked = true;
            chkAltura.Checked = true;
            chkAcciónPropuesta.Checked = true;
            chkCondicion.Checked = true;
            chkLitoral.Checked = true;
            chkMaritimoTerrestre.Checked = true;
            chkComentarios.Checked = true;

            txtCloneBase.Style["background-color"] = "#FFFFFF";
            txtCloneBase.Enabled = true;
            btnCloneBase.Visible = true;
            btnCloneBaseClear.Visible = false;

            pnlLoading.Style["display"] = "block";
            pnlClone.Style["display"] = "block";
        }

        protected void btnCloneSelected_Click(object sender, EventArgs e)
        {
            pnlCloneError.Style["display"] = "none";
            lblCloneError.Text = "";
            string invalidMessage = "";
            string successMessage = "";

            bool dapOrVaras = false;
            if (pnlCloneDap.Visible)
                dapOrVaras = chkCloneDap.Checked;
            else
                dapOrVaras = chkCloneVaras.Checked;

            #region Validate Characteristics
            if (
                !chkCloneNombre.Checked &&
                !dapOrVaras &&
                !chkAltura.Checked &&
                !chkAcciónPropuesta.Checked &&
                !chkCondicion.Checked &&
                !chkLitoral.Checked &&
                !chkMaritimoTerrestre.Checked &&
                !chkComentarios.Checked
                )
            {
                invalidMessage += " • Seleccione al menos una característica <br />";
            }
            #endregion

            #region Validate Árbol a ser Clonado
            Project_Organisms project_OrganismBase;
            if (!Project_OrganismsBLL.TryParse_ByNumber(txtCloneBase.Text, out project_OrganismBase))
            {
                invalidMessage = " • Número del Árbol a ser Copiado Inválido<br />";
            }
            #endregion

            List<Project_Organisms> ProjectOrganismsIDsToClone = new List<Project_Organisms>();
            Project_Organisms project_OrganismFROM = null;
            Project_Organisms project_OrganismTO = null;

            if (!rbtCloneRange.Checked && rbtCloneSelect.Checked)
            {// Selected
                #region validate selected

                // Iterate through the Projects.Rows property
                foreach (GridViewRow row in gridViewTreeDetails.Rows)
                {
                    // Access the CheckBox
                    CheckBox cb = (CheckBox)(row.FindControl("chkTreeDetailsSelector"));
                    if (cb != null && cb.Checked)
                    {
                        // Save the ProjectID value for deletion
                        // First, get the ProjectID for the selected row
                        Int32 treeDetailsID = (Int32)gridViewTreeDetails.DataKeys[row.RowIndex].Value;
                        Eisk.BusinessEntities.TreeDetail treeDetail = new TreeDetailBLL().GetTreeDetailByTreeDetailsID(treeDetailsID);
                        ProjectOrganismsIDsToClone.Add(treeDetail.Project_Organisms);
                    }
                }

                if (ProjectOrganismsIDsToClone.Count == 0)
                {
                    invalidMessage += " • Para copiar las características a los árboles previamente seleccionados debe seleccionar al menos uno <br />";
                }
                #endregion
            }
            else
            {// Range
                #region validate range

                if (!Project_OrganismsBLL.TryParse_ByNumber(txtCloneFrom.Text, out project_OrganismFROM))
                {
                    invalidMessage += " • Número del Árbol " + '"' + "Desde" + '"' + " Inválido <br />";
                }

                if (!Project_OrganismsBLL.TryParse_ByNumber(txtCloneTo.Text, out project_OrganismTO))
                {
                    invalidMessage += " • Número del Árbol " + '"' + "Hasta" + '"' + " Inválido <br />";
                }

                if (project_OrganismFROM != null && project_OrganismTO != null)
                    if (project_OrganismFROM.TreeDetails.First().Number > project_OrganismTO.TreeDetails.First().Number)
                    {
                        invalidMessage += " • Número del Árbol " + '"' + "Desde" + '"' + " menor que " + '"' + "Hasta" + '"' + " <br />";
                    }
                #endregion
            }

            if (!string.IsNullOrEmpty(invalidMessage))
            {
                pnlCloneError.Style["display"] = "block";
                pnlLoading.Style["display"] = "block";
                pnlClone.Style["display"] = "block";
                pnlCloneRange.Style["display"] = (rbtCloneRange.Checked) ? "block" : "none";
                lblCloneError.Text = "<strong>Favor validar los siguientes detalles:</strong><br />" + invalidMessage;
                return;
            }
            else
            {
                using (DatabaseContext _DatabaseContext = new DatabaseContext())
                {
                    var treeDetailBASE = project_OrganismBase.TreeDetails.First();
                    if (!rbtCloneRange.Checked && rbtCloneSelect.Checked)
                    {// Selected                         
                        foreach (var projectOrganism in ProjectOrganismsIDsToClone)
                        {
                            var projectOrganismTHIS = _DatabaseContext.Project_Organisms.Where(i => i.ProjectOrganismID == projectOrganism.ProjectOrganismID).First();
                            var treeDetailTHIS = _DatabaseContext.TreeDetails.First(i => i.ProjectOrganismID == projectOrganismTHIS.ProjectOrganismID);

                            if (chkCloneNombre.Checked)
                            {
                                Organism organism = new OrganismBLL().GetOrganismByOrganismID(project_OrganismBase.OrganismID);
                                projectOrganismTHIS.OrganismID = organism.OrganismID;
                                projectOrganismTHIS.OrganismReference.EntityKey = organism.EntityKey;
                            }

                            foreach (var item in _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetailTHIS.TreeDetailsID).ToList())
                                _DatabaseContext.Daps.DeleteObject(item);

                            if (chkCloneVaras.Checked)
                                treeDetailTHIS.Varas = treeDetailBASE.Varas;

                            if (chkCloneDap.Checked)
                            {
                                treeDetailTHIS.Dap = treeDetailBASE.Dap;
                                treeDetailTHIS.Dap_Counter = treeDetailBASE.Dap_Counter;

                                foreach (var dap in treeDetailBASE.Daps)
                                {
                                    Dap dapTHIS = new Dap();
                                    dapTHIS.DapValue = dap.DapValue;
                                    dapTHIS.TreeDetailsID = treeDetailTHIS.TreeDetailsID;
                                    dapTHIS.TreeDetailReference.EntityKey = treeDetailTHIS.EntityKey;

                                    _DatabaseContext.Daps.AddObject(dapTHIS);
                                }
                            }

                            if (chkAltura.Checked)
                                treeDetailTHIS.Height = treeDetailBASE.Height;

                            if (chkAcciónPropuesta.Checked)
                            {
                                ActionProposed actionProposed = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(treeDetailBASE.ActionProposedID));
                                treeDetailTHIS.ActionProposedID = actionProposed.ActionProposedID;
                                treeDetailTHIS.ActionProposedReference.EntityKey = actionProposed.EntityKey;
                            }

                            if (chkCondicion.Checked)
                            {
                                Condition condition = new ConditionBLL().GetConditionByConditionID(Convert.ToInt32(treeDetailBASE.ConditionID));
                                treeDetailTHIS.ConditionID = condition.ConditionID;
                                treeDetailTHIS.ConditionReference.EntityKey = condition.EntityKey;
                            }

                            if (chkLitoral.Checked)
                                treeDetailTHIS.Littoral = treeDetailBASE.Littoral;

                            if (chkMaritimoTerrestre.Checked)
                                treeDetailTHIS.MaritimeZone = treeDetailBASE.MaritimeZone;

                            if (chkComentarios.Checked)
                                treeDetailTHIS.Commentary = treeDetailBASE.Commentary;
                        }
                        successMessage = "Las siguientes cararcterísticas del árbol #" + txtCloneBase.Text + " han sido copiadas satisfactóriamente a los " + ProjectOrganismsIDsToClone.Count + " árboles seleccionados: ";
                    }
                    else
                    {// Range
                        int from = project_OrganismFROM.TreeDetails.FirstOrDefault().Number.Value;
                        int to = project_OrganismTO.TreeDetails.FirstOrDefault().Number.Value;
                        for (int i = from; i <= to; i++)
                        {
                            var projectOrganismTHIS = _DatabaseContext.Project_Organisms.Where(instance => instance.TreeDetails.FirstOrDefault().Number == i).First();
                            var treeDetailTHIS = _DatabaseContext.TreeDetails.First(instance => instance.ProjectOrganismID == projectOrganismTHIS.ProjectOrganismID);

                            if (chkCloneNombre.Checked)
                            {
                                Organism organism = new OrganismBLL().GetOrganismByOrganismID(project_OrganismBase.OrganismID);
                                projectOrganismTHIS.OrganismID = organism.OrganismID;
                                projectOrganismTHIS.OrganismReference.EntityKey = organism.EntityKey; 
                            }

                            foreach (var item in _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetailTHIS.TreeDetailsID).ToList())
                                _DatabaseContext.Daps.DeleteObject(item);

                            if (chkCloneVaras.Checked)
                            {
                                treeDetailTHIS.Varas = treeDetailBASE.Varas; 
                            }

                            if (chkCloneDap.Checked)
                            {
                                treeDetailTHIS.Dap = treeDetailBASE.Dap;
                                treeDetailTHIS.Dap_Counter = treeDetailBASE.Dap_Counter;

                                foreach (var dap in treeDetailBASE.Daps)
                                {
                                    Dap dapTHIS = new Dap();
                                    dapTHIS.DapValue = dap.DapValue;
                                    dapTHIS.TreeDetailsID = treeDetailTHIS.TreeDetailsID;
                                    dapTHIS.TreeDetailReference.EntityKey = treeDetailTHIS.EntityKey;

                                    _DatabaseContext.Daps.AddObject(dapTHIS);
                                } 
                            }

                            if (chkAltura.Checked)
                            {
                                treeDetailTHIS.Height = treeDetailBASE.Height; 
                            }

                            if (chkAcciónPropuesta.Checked)
                            {
                                ActionProposed actionProposed = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(treeDetailBASE.ActionProposedID));
                                treeDetailTHIS.ActionProposedID = actionProposed.ActionProposedID;
                                treeDetailTHIS.ActionProposedReference.EntityKey = actionProposed.EntityKey; 
                            }

                            if (chkCondicion.Checked)
                            {
                                Condition condition = new ConditionBLL().GetConditionByConditionID(Convert.ToInt32(treeDetailBASE.ConditionID));
                                treeDetailTHIS.ConditionID = condition.ConditionID;
                                treeDetailTHIS.ConditionReference.EntityKey = condition.EntityKey; 
                            }

                            if (chkLitoral.Checked)
                            {
                                treeDetailTHIS.Littoral = treeDetailBASE.Littoral; 
                            }

                            if (chkMaritimoTerrestre.Checked)
                            {
                                treeDetailTHIS.MaritimeZone = treeDetailBASE.MaritimeZone; 
                            }

                            if (chkComentarios.Checked)
                            {
                                treeDetailTHIS.Commentary = treeDetailBASE.Commentary; 
                            }
                        }
                        successMessage = "Las siguientes cararcterísticas del árbol <b>#" + txtCloneBase.Text + "</b> han sido copiadas satisfactóriamente a los árboles desde el árbol <b>#" + from + "</b> al árbol <b>#" + to + "</b>: <br />";
                    }

                    if (chkCloneNombre.Checked)
                        successMessage += "<b> • Nombre Común - Nombre Científico</b><br />";
                    if (chkCloneVaras.Checked)
                        successMessage += "<b> • Varas</b><br />";
                    if (chkCloneDap.Checked)
                        successMessage += "<b> • D.A.P</b><br />";
                    if (chkAltura.Checked)
                        successMessage += "<b> • Altura (pies)</b><br />";
                    if (chkAcciónPropuesta.Checked)
                        successMessage += "<b> • Acción Propuesta</b><br />";
                    if (chkCondicion.Checked)
                        successMessage += "<b> • Condición</b><br />";
                    if (chkLitoral.Checked)
                        successMessage += "<b> • Árbol en la Servidumbre de Vigilancia de Litoral</b><br />";
                    if (chkMaritimoTerrestre.Checked)
                        successMessage += "<b> • Árbol en la Zona Marítimo Terrestre</b><br />";
                    if (chkComentarios.Checked)
                        successMessage += "<b> • Comentarios</b>";

                    _DatabaseContext.SaveChanges();
                    pnlCloneRange.Style["display"] = (rbtCloneRange.Checked) ? "block" : "none";

                    gridViewTreeDetails.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                    gridViewTreeDetails.DataBind();

                    ltlMessage.Text += MessageFormatter.GetFormattedSuccessMessage(successMessage);

                    btnClear_Click(null, null);
                    pnlClone.Style["display"] = "none";
                    pnlLoading.Style["display"] = "none";
                }
            }
        }

        protected void btnCloneBase_Click(object sender, EventArgs e)
        {
            pnlCloneError.Style["display"] = "none";
            lblCloneError.Text = "";
            string message = "";

            Project_Organisms project_Organism;

            if (!Project_OrganismsBLL.TryParse_ByNumber(txtCloneBase.Text, out project_Organism))
            {
                message = " • Número del Árbol a ser Copiado Inválido<br />";
            }
            else
            {
                var treeInfo = project_Organism.TreeDetails.First();

                lblNumber.Text = treeInfo.Number.Value.ToString();
                lblNombreComun.Text = project_Organism.Organism.CommonName.CommonNameDesc;
                lblVaras.Text = treeInfo.Varas.Value.ToString();
                pnlCloneVarasChk.Visible = treeInfo.Varas.Value != 0;
                pnlCloneVaras.Visible = treeInfo.Varas.Value != 0;
                lblDap.Text = String.Format("{0:0.####################################}", treeInfo.Dap) + "' ";
                pnlCloneDapChk.Visible = treeInfo.Dap != 0;
                pnlCloneDap.Visible = treeInfo.Dap != 0;
                lblAltura.Text = String.Format("{0:0.####################################}", treeInfo.Height.Value) + "'' ";
                lblAcciónPropuesta.Text = treeInfo.ActionProposed.ActionProposedDesc;
                lblCondicion.Text = treeInfo.Condition.ConditionDesc;
                lblLitoral.Text = treeInfo.Littoral ? "Si" : "No";
                lblMaritimoTerrestre.Text = treeInfo.MaritimeZone ? "Si" : "No";

                txtCloneBase.Style["background-color"] = "#E3E9EF";
                txtCloneBase.Enabled = false;
            }

            if (!string.IsNullOrEmpty(message))
            {
                pnlCloneError.Style["display"] = "block";
                pnlLoading.Style["display"] = "block";
                pnlClone.Style["display"] = "block";
                lblCloneError.Text = "<strong>Favor validar los siguientes detalles:</strong><br />" + message;
                return;
            }

            btnCloneBase.Visible = false;
            btnCloneBaseClear.Visible = true;
            pnlCloneRange.Style["display"] = (rbtCloneRange.Checked) ? "block" : "none";
        }

        protected void btnCloneBaseClear_Click(object sender, EventArgs e)
        {
            txtCloneBase.Text = "";

            lblNumber.Text = "";
            lblNombreComun.Text = "";
            lblVaras.Text = "";
            lblDap.Text = "";
            lblAltura.Text = "";
            lblAcciónPropuesta.Text = "";
            lblCondicion.Text = "";
            lblLitoral.Text = "";
            lblMaritimoTerrestre.Text = "";

            txtCloneBase.Style["background-color"] = "#FFFFFF";
            txtCloneBase.Enabled = true;
            btnCloneBase.Visible = true;
            btnCloneBaseClear.Visible = false;

            pnlCloneRange.Style["display"] = (rbtCloneRange.Checked) ? "block" : "none";
        }

    }
}

using System;
using System.Web.UI.WebControls;
using Eisk.Helpers;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Web;
using System.Web.Security;
using Eisk.Web.App_Logic.Helpers;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.DataAccessLayer;
using System.Text;
using System.Drawing;
using System.Web.Configuration;
using System.Configuration;

public partial class Tl_Tree_Details : System.Web.UI.Page
{
    enum actionType
    {
        insert,
        update
    }

    class TempDap
    {
        private int _Number;
        public int Number { get { return _Number; } }

        private decimal _Dap;
        public decimal Dap { get { return _Dap; } }

        public TempDap(int number, decimal dap)
        {
            _Number = number;
            _Dap = dap;
        }
    }

    protected User editor;
    protected User creator;
    protected Project_Organisms tree;
    protected TreeDetail treeDetail;
    protected Organism organism;
    protected string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"];

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.Load += new System.EventHandler(this.Page_Load);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(" var webPath = '" + System.Configuration.ConfigurationManager.AppSettings["webPath"] + "'; ");
        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "codeBehindToJS", sb.ToString(), true);
    }

    protected string GetDaps()
    {
        Project_Organisms projectOrganisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID(Convert.ToInt32(Page.RouteData.Values["project_organism_id"]));
        List<Dap> tempDaps = new DapBLL().GetDapsByTreeDetailsID(projectOrganisms.TreeDetails.First().TreeDetailsID).ToList();

        string daps = "";
        foreach (Dap tempDap in tempDaps)
        {
            daps += Convert.ToDouble(tempDap.DapValue).ToString() + ",";
        }
        daps = (daps.Count() > 1) ? daps.Substring(0, daps.Count() - 1) : daps;

        return daps;
    }


    #region "Select handlers"

    protected void OdsProject_Organism_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "edit")
            formViewTlProject_Organism.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewTlProject_Organism.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");

        if (formViewTlProject_Organism.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewTlProject_Organism_DataBound(object sender, System.EventArgs e)
    {
        int projectID = Convert.ToInt32(Page.RouteData.Values["project_id"]);
        TextBox txtCommonName = (TextBox)formViewTlProject_Organism.FindControl("txtCommonName");
        TextBox txtScientificName = (TextBox)formViewTlProject_Organism.FindControl("txtScientificName");
        HiddenField hfOrganism = (HiddenField)formViewTlProject_Organism.FindControl("hfOrganism");

        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {

            // 
            List<Organism> organisms =
            (
                from groupTreeDetailsList in
                    _DatabaseContext.TreeDetails
                    .DynamicOrderBy("Number DESC")
                where
                    (groupTreeDetailsList.Project_Organisms.ProjectID == projectID)
                select
                    (groupTreeDetailsList.Project_Organisms.Organism)
            )
            .ToList();

            organisms = organisms.Distinct().ToList();
            organisms = organisms.Take(6).ToList();
            //

            for (int i = 0; i < organisms.Count(); i++)
            {
                Panel pnlButtons = (Panel)formViewTlProject_Organism.FindControl("pnlButtons" + ((i > 2) ? 2 : 1));
                Button button = (Button)formViewTlProject_Organism.FindControl("btnTree" + i.ToString());
                string name = organisms[i].CommonName.CommonNameDesc + "/" + organisms[i].ScientificName.ScientificNameDesc;
                button.Text = name.Length > 30 ? name.Substring(0, 30) + "..." : name;
                button.Visible = true;
                pnlButtons.Visible = true;
                button.OnClientClick = " document.getElementById('" + txtCommonName.ClientID + "').value = '" + organisms[i].CommonName.CommonNameDesc + "'; " +
                                       " document.getElementById('" + txtScientificName.ClientID + "').value = '" + organisms[i].ScientificName.ScientificNameDesc + "'; " +
                                       " document.getElementById('" + hfOrganism.ClientID + "').value = '" + organisms[i].OrganismID + "'; " +
                                       " return false; ";
            }

        }

        if (Page.Request.UrlReferrer != null)
            if (Page.Request.UrlReferrer.AbsolutePath.Contains("new") && !IsPostBack)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Insert successful");
            }
    }

    #endregion

    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (formViewTlProject_Organism.CurrentMode == FormViewMode.Insert)
        {
            RangeValidator rvVaras = (RangeValidator)formViewTlProject_Organism.FindControl("rvVaras");
            rvVaras.Enabled = false;
            rvVaras.IsValid = true;

            RangeValidator rvDapCounter = (RangeValidator)formViewTlProject_Organism.FindControl("rvDapCounter");
            rvDapCounter.Enabled = false;
            rvDapCounter.IsValid = true;

            RangeValidator rvDap = (RangeValidator)formViewTlProject_Organism.FindControl("rvDap");
            rvDap.Enabled = false;
            rvDap.IsValid = true;

            RequiredFieldValidator rfvDap = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvDap");
            rfvDap.Enabled = false;
            rfvDap.IsValid = true;

            RequiredFieldValidator rfvCommonName = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvCommonName");
            rfvCommonName.Enabled = false;
            rfvCommonName.IsValid = true;

            RequiredFieldValidator rfvScientificName = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvScientificName");
            rfvScientificName.Enabled = false;
            rfvScientificName.IsValid = true;

            RequiredFieldValidator rfvVaras = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvVaras");
            rfvVaras.Enabled = false;
            rfvVaras.IsValid = true;

            RequiredFieldValidator rfvHeight = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvHeight");
            rfvHeight.Enabled = false;
            rfvHeight.IsValid = true;

            formViewTlProject_Organism.InsertItem(true);
        }
        else
        {
            RangeValidator rvVaras = (RangeValidator)formViewTlProject_Organism.FindControl("rvVaras");
            rvVaras.Enabled = false;
            rvVaras.IsValid = true;

            RangeValidator rvDapCounter = (RangeValidator)formViewTlProject_Organism.FindControl("rvDapCounter");
            rvDapCounter.Enabled = false;
            rvDapCounter.IsValid = true;

            RangeValidator rvDap = (RangeValidator)formViewTlProject_Organism.FindControl("rvDap");
            rvDap.Enabled = false;
            rvDap.IsValid = true;

            RequiredFieldValidator rfvDap = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvDap");
            rfvDap.Enabled = false;
            rfvDap.IsValid = true;

            RequiredFieldValidator rfvCommonName = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvCommonName");
            rfvCommonName.Enabled = false;
            rfvCommonName.IsValid = true;

            RequiredFieldValidator rfvScientificName = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvScientificName");
            rfvScientificName.Enabled = false;
            rfvScientificName.IsValid = true;

            RequiredFieldValidator rfvVaras = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvVaras");
            rfvVaras.Enabled = false;
            rfvVaras.IsValid = true;

            RequiredFieldValidator rfvHeight = (RequiredFieldValidator)formViewTlProject_Organism.FindControl("rfvHeight");
            rfvHeight.Enabled = false;
            rfvHeight.IsValid = true;

            formViewTlProject_Organism.UpdateItem(true);
        }
    }

    protected void ButtonGoToViewPage_Click(object sender, EventArgs e)
    {
        string source = Request.QueryString["s"];
        if (source != null && source == "i")
        {
            Response.RedirectToRoute("tl-treeinventory", new { Project_id = RouteData.Values["Project_id"] });
            Response.RedirectLocation += Request.Url.Query;
        }
        else
        {
            Response.RedirectToRoute("tl", new { Project_id = RouteData.Values["Project_id"] });
            Response.RedirectLocation +=
                  "?poid=" + Page.RouteData.Values["project_organism_id"] as string
                + "&lat=" + Page.RouteData.Values["lat"] as string
                + "&lon=" + Page.RouteData.Values["lon"] as string;
        }
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewTlProject_Organism_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        UserBLL userBLL = new UserBLL();

        //TextBox txtProject = (TextBox)formViewTlProject_Organism.FindControl("txtProject");
        ProjectActionStatus status = ProjectActionStatus.Success;// Validate(txtProject.Text, actionType.insert);

        if (status == ProjectActionStatus.Success)
        {
            User creator = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            e.Values["CreatedDate"] = DateTime.Now;
            e.Values["CreatorUserID"] = creator.UserID;
            e.Values["EditedDate"] = DateTime.Now;
            e.Values["EditorUserId"] = creator.UserID;
            e.Values["ProjectID"] = RouteData.Values["Project_id"].ToString();
            e.Values["OrganismID"] = ((HiddenField)formViewTlProject_Organism.FindControl("hfOrganism")).Value;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    private static ProjectActionStatus Validate(string Project, actionType act)
    {
        ProjectBLL ProjectBLL = new ProjectBLL();
        List<Project> Projects = ProjectBLL.GetProjectByFilter(Project.Trim());
        if (Projects.Count > 0 && act == actionType.insert)
            return ProjectActionStatus.Duplicate;
        else
            return ProjectActionStatus.Success;
    }

    public string GetErrorMessage(ProjectActionStatus status)
    {
        switch (status)
        {
            case ProjectActionStatus.Duplicate:
                return "Project Name address already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsProject_Organism_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
        //string lat = "";
        //string lon = "";
        if (result != 0)
        {
            Project_Organisms project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID(result);
            ProjectInfoTreeLocation projectInfoTreeLocation = new ProjectInfoTreeLocationBLL().GetProjectInfoTreeLocationsByProjectID(project_Organisms.ProjectID)[0];
            bool fromInventory = false;

            UserBLL userBLL = new UserBLL();
            Eisk.BusinessEntities.User user = userBLL.GetUserByUserName((HttpContext.Current.User.Identity).Name);

            if (user != null)
            {
                // TreeDetail
                TextBox txtVaras = (TextBox)formViewTlProject_Organism.FindControl("txtVaras");
                HiddenField hfDap = (HiddenField)formViewTlProject_Organism.FindControl("hfDap");
                TextBox txtDap = (TextBox)formViewTlProject_Organism.FindControl("txtDap");
                TextBox txtDapCounter = (TextBox)formViewTlProject_Organism.FindControl("txtDapCounter");
                TextBox txtHeight = (TextBox)formViewTlProject_Organism.FindControl("txtHeight");
                TextBox txtCommentary = (TextBox)formViewTlProject_Organism.FindControl("txtCommentary");

                Label lblNumber = (Label)formViewTlProject_Organism.FindControl("lblNumber");
                TextBox txtX = (TextBox)formViewTlProject_Organism.FindControl("txtX");
                TextBox txtY = (TextBox)formViewTlProject_Organism.FindControl("txtY");
                TextBox txtLat = (TextBox)formViewTlProject_Organism.FindControl("txtLat");
                TextBox txtLon = (TextBox)formViewTlProject_Organism.FindControl("txtLon");

                RadioButtonList rblActionProposed = (RadioButtonList)formViewTlProject_Organism.FindControl("rblActionProposed");
                RadioButtonList rblCondition = (RadioButtonList)formViewTlProject_Organism.FindControl("rblCondition");

                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                // Edgardo Ramos - 20130928
                CheckBox chkLittoral = (CheckBox)formViewTlProject_Organism.FindControl("chkLittoral");
                CheckBox chkMaritimeZone = (CheckBox)formViewTlProject_Organism.FindControl("chkMaritimeZone");
                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                TreeDetail treeDetail = new TreeDetail();

                if (!string.IsNullOrEmpty(txtVaras.Text) && txtVaras.Text != "0") // Is Cepa
                {
                    treeDetail.Dap = 0;
                    treeDetail.Dap_Counter = 0;
                }
                else if (txtDapCounter.Text == "1") // Solo un Dap
                {
                    //treeDetail.Dap = Convert.ToDecimal(txtDap.Text);// new DapBLL().GetDap(0); 
                    //treeDetail.Dap_Counter = Convert.ToInt32(txtDapCounter.Text); //new DapBLL().GetDapCount(0);
                    treeDetail.Dap = Convert.ToDecimal(hfDap.Value);// new DapBLL().GetDap(0); 
                    treeDetail.Dap_Counter = Convert.ToInt32(1); //new DapBLL().GetDapCount(0);
                }
                else // Mas de un Dap
                {
                    string[] daps = hfDap.Value.Split(',');
                    double dapTotal = 0;
                    foreach (string dap in daps)
                    {
                        dapTotal += Convert.ToDouble(dap);
                    }//dapTotal;// new DapBLL().GetDap(0); 
                    treeDetail.Dap = Convert.ToDecimal(Math.Round(Math.Sqrt(Math.Pow(dapTotal, 2D) / Convert.ToDouble(daps.Count())) * 100) / 100);
                    treeDetail.Dap_Counter = Convert.ToInt32(txtDapCounter.Text); //new DapBLL().GetDapCount(0);
                }

                treeDetail.Varas = Convert.ToInt32(string.IsNullOrEmpty(txtVaras.Text) ? "0" : txtVaras.Text);

                treeDetail.Height = Convert.ToDecimal(txtHeight.Text);
                treeDetail.Commentary = (txtCommentary.Text.Length > txtCommentary.MaxLength) ? txtCommentary.Text.Substring(0, txtCommentary.MaxLength) : txtCommentary.Text;

                treeDetail.Number = Convert.ToInt32(lblNumber.Text);


                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                // Edgardo Ramos - 20130928
                decimal dec;
                RadioButtonList rblPosition = (RadioButtonList)formViewTlProject_Organism.FindControl("rblPosition");

                if (rblPosition.SelectedValue == "0") // Nad83
                {
                    treeDetail.X = decimal.TryParse(txtX.Text, out dec) ? Convert.ToDecimal(txtX.Text) : 0;
                    treeDetail.Y = decimal.TryParse(txtY.Text, out dec) ? Convert.ToDecimal(txtY.Text) : 0;

                    if (treeDetail.X != 0 && treeDetail.Y != 0)
                    {
                        Dictionary<string, object> anewpointObj = Utility.ConvertToLatLng(treeDetail.X.ToString(), treeDetail.Y.ToString(), @"~/App_Resources/client-scripts/tl/");

                        treeDetail.Lat = Convert.ToDecimal(anewpointObj["y"]);
                        txtLat.Text = treeDetail.Lat.ToString();
                        treeDetail.Lon = Convert.ToDecimal(anewpointObj["x"]);
                        txtLon.Text = treeDetail.Lon.ToString();
                    }
                    else
                    {
                        treeDetail.Lat = 0;
                        treeDetail.Lon = 0;
                    }
                }
                else // StatePlanes
                {
                    treeDetail.Lat = decimal.TryParse(txtLat.Text, out dec) ? Convert.ToDecimal(txtLat.Text) : 0;
                    treeDetail.Lon = decimal.TryParse(txtLon.Text, out dec) ? Convert.ToDecimal(txtLon.Text) : 0;

                    if (treeDetail.Lat != 0 && treeDetail.Lon != 0)
                    {
                        Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(treeDetail.Lon.ToString(), treeDetail.Lat.ToString(), @"~/App_Resources/client-scripts/tl/");

                        treeDetail.X = Convert.ToDecimal(anewpointObj["x"]);
                        txtX.Text = treeDetail.X.ToString();
                        treeDetail.Y = Convert.ToDecimal(anewpointObj["y"]);
                        txtY.Text = treeDetail.Y.ToString();
                    }
                    else
                    {
                        treeDetail.X = 0;
                        treeDetail.Y = 0;
                    }
                }

                //treeDetail.X = Convert.ToDecimal(txtX.Text);
                //treeDetail.Y = Convert.ToDecimal(txtY.Text);
                //treeDetail.Lat = Convert.ToDecimal(txtLat.Text);
                //treeDetail.Lon = Convert.ToDecimal(txtLon.Text);

                treeDetail.Littoral = chkLittoral.Checked;
                treeDetail.MaritimeZone = chkMaritimeZone.Checked;

                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                treeDetail.CreatedDate = DateTime.Now;
                treeDetail.CreatorUserID = user.UserID;
                treeDetail.EditedDate = DateTime.Now;
                treeDetail.EditorUserID = user.UserID;

                ActionProposed actionProposed = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(rblActionProposed.SelectedValue));
                treeDetail.ActionProposedID = actionProposed.ActionProposedID;
                treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;

                Condition condition = new ConditionBLL().GetConditionByConditionID(Convert.ToInt32(rblCondition.SelectedValue));
                treeDetail.ConditionID = condition.ConditionID;
                treeDetail.ConditionReference.EntityKey = condition.EntityKey;

                treeDetail.ProjectOrganismID = project_Organisms.ProjectOrganismID;
                treeDetail.Project_OrganismsReference.EntityKey = project_Organisms.EntityKey;

                int treeDetailID = new TreeDetailBLL().CreateNewTreeDetail(treeDetail);

                if (treeDetail.Varas != null && treeDetail.Varas > 0) // Is Cepa
                {
                    //
                }
                else if (txtDapCounter.Text == "1") // Solo un Dap
                {
                    Dap dap = new Dap();
                    dap.DapValue = Convert.ToDecimal(hfDap.Value);
                    dap.TreeDetailsID = treeDetailID;
                    dap.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                    new DapBLL().CreateNewDap(dap);
                }
                else // Mas de un Dap
                {
                    string[] daps = hfDap.Value.Split(',');
                    foreach (string dapString in daps)
                    {
                        Dap dap = new Dap();
                        dap.DapValue = Convert.ToDecimal(dapString);
                        dap.TreeDetailsID = treeDetailID;
                        dap.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                        new DapBLL().CreateNewDap(dap);
                    }
                }

                fromInventory = Convert.ToDecimal(projectInfoTreeLocation.X) == Convert.ToDecimal(treeDetail.X) && Convert.ToDecimal(projectInfoTreeLocation.Y) == Convert.ToDecimal(treeDetail.Y);
                //
            }

            if (fromInventory)
            {
                Response.RedirectToRoute("tl-treeinventory", new { project_id = RouteData.Values["project_id"] });
            }
            else
            {
                Response.RedirectToRoute("tl", new { Project_id = RouteData.Values["Project_id"] });
                Response.RedirectLocation += "?poid=" + project_Organisms.ProjectOrganismID + "&lat=0&lon=0";
            }

            //Response.RedirectToRoute("tl-tree-details_edit", new { edit_mode = "edit", project_id = Page.RouteData.Values["project_id"] as string, project_organism_id = result });
        }
    }

    protected void FormViewTlProject_Organism_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {

        if (e.Exception != null)
        {
            // Display a user-friendly message 
            ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(e.Exception);
            // Indicate that the exception has been handled 
            e.ExceptionHandled = true;
            // Keep the row in edit mode 
            e.KeepInInsertMode = true;
        }
    }

    #endregion

    #region "Update handlers"

    protected void FormViewTlProject_Organism_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        User editor = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

        Project_Organisms project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID((int)e.Keys["ProjectOrganismID"]);

        using (DatabaseContext _DatabaseContext = new DatabaseContext())
        {
            // TreeDetail
            TextBox txtVaras = (TextBox)formViewTlProject_Organism.FindControl("txtVaras");
            HiddenField hfDap = (HiddenField)formViewTlProject_Organism.FindControl("hfDap");
            TextBox txtHeight = (TextBox)formViewTlProject_Organism.FindControl("txtHeight");
            TextBox txtCommentary = (TextBox)formViewTlProject_Organism.FindControl("txtCommentary");

            Label lblNumber = (Label)formViewTlProject_Organism.FindControl("lblNumber");
            TextBox txtX = (TextBox)formViewTlProject_Organism.FindControl("txtX");
            TextBox txtY = (TextBox)formViewTlProject_Organism.FindControl("txtY");
            TextBox txtLat = (TextBox)formViewTlProject_Organism.FindControl("txtLat");
            TextBox txtLon = (TextBox)formViewTlProject_Organism.FindControl("txtLon");

            RadioButtonList rblActionProposed = (RadioButtonList)formViewTlProject_Organism.FindControl("rblActionProposed");
            RadioButtonList rblCondition = (RadioButtonList)formViewTlProject_Organism.FindControl("rblCondition");

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Edgardo Ramos - 20130928
            CheckBox chkLittoral = (CheckBox)formViewTlProject_Organism.FindControl("chkLittoral");
            CheckBox chkMaritimeZone = (CheckBox)formViewTlProject_Organism.FindControl("chkMaritimeZone");
            CheckBox cbCepa = (CheckBox)formViewTlProject_Organism.FindControl("cbCepa");
            // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.ProjectOrganismID == project_Organisms.ProjectOrganismID);

            //Delete existing Daps
            foreach (Dap item in _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetail.TreeDetailsID).ToList())
            {
                _DatabaseContext.Daps.DeleteObject(item);
            }

            if (cbCepa.Checked)// Is Cepa
            {
                treeDetail.Dap = 0;
                treeDetail.Dap_Counter = 0;

                treeDetail.Varas = Convert.ToInt32(string.IsNullOrEmpty(txtVaras.Text) ? "0" : txtVaras.Text);
            }
            else
            {
                treeDetail.Varas = 0;

                var dapsList = hfDap.Value.Split(',').ToList();
                if (dapsList.Count == 1) // Solo un Dap
                {
                    decimal dap = Convert.ToDecimal(dapsList[0]);

                    treeDetail.Dap = dap;
                    treeDetail.Dap_Counter = 1;

                    Dap dapObj = new Dap();
                    dapObj.DapValue = dap;
                    dapObj.TreeDetailsID = treeDetail.TreeDetailsID;
                    dapObj.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                    _DatabaseContext.Daps.AddObject(dapObj);
                }
                else // 1 o Mas de un Dap
                {
                    string[] daps = hfDap.Value.Split(',');
                    double dapTotal = 0;
                    foreach (string dap in daps)
                    {
                        dapTotal += Convert.ToDouble(dap);
                    }
                    treeDetail.Dap = Convert.ToDecimal(Math.Round(Math.Sqrt(Math.Pow(dapTotal, 2D) / Convert.ToDouble(daps.Count())) * 100) / 100);
                    treeDetail.Dap_Counter = daps.Count();

                    foreach (string dapString in daps)
                    {
                        Dap dap = new Dap();
                        dap.DapValue = Convert.ToDecimal(dapString);
                        dap.TreeDetailsID = treeDetail.TreeDetailsID;
                        dap.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                        _DatabaseContext.Daps.AddObject(dap);
                    }
                }
            }

            treeDetail.Height = Convert.ToDecimal(txtHeight.Text);
            treeDetail.Commentary = (txtCommentary.Text.Length > txtCommentary.MaxLength) ? txtCommentary.Text.Substring(0, txtCommentary.MaxLength) : txtCommentary.Text;

            treeDetail.Number = Convert.ToInt32(lblNumber.Text);

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Edgardo Ramos - 20130928
            decimal dec;
            RadioButtonList rblPosition = (RadioButtonList)formViewTlProject_Organism.FindControl("rblPosition");

            if (rblPosition.SelectedValue == "0") // Nad83
            {
                treeDetail.X = decimal.TryParse(txtX.Text, out dec) ? Convert.ToDecimal(txtX.Text) : 0;
                treeDetail.Y = decimal.TryParse(txtY.Text, out dec) ? Convert.ToDecimal(txtY.Text) : 0;

                if (treeDetail.X != 0 && treeDetail.Y != 0)
                {
                    Dictionary<string, object> anewpointObj = Utility.ConvertToLatLng(treeDetail.X.ToString(), treeDetail.Y.ToString(), @"~/App_Resources/client-scripts/tl/");

                    treeDetail.Lat = Convert.ToDecimal(anewpointObj["y"]);
                    txtLat.Text = treeDetail.Lat.ToString();
                    treeDetail.Lon = Convert.ToDecimal(anewpointObj["x"]);
                    txtLon.Text = treeDetail.Lon.ToString();
                }
                else
                {
                    treeDetail.Lat = 0;
                    treeDetail.Lon = 0;
                }
            }
            else // StatePlanes
            {
                treeDetail.Lat = decimal.TryParse(txtLat.Text, out dec) ? Convert.ToDecimal(txtLat.Text) : 0;
                treeDetail.Lon = decimal.TryParse(txtLon.Text, out dec) ? Convert.ToDecimal(txtLon.Text) : 0;

                if (treeDetail.Lat != 0 && treeDetail.Lon != 0)
                {
                    Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(treeDetail.Lon.ToString(), treeDetail.Lat.ToString(), @"~/App_Resources/client-scripts/tl/");

                    treeDetail.X = Convert.ToDecimal(anewpointObj["x"]);
                    txtX.Text = treeDetail.X.ToString();
                    treeDetail.Y = Convert.ToDecimal(anewpointObj["y"]);
                    txtY.Text = treeDetail.Y.ToString();
                }
                else
                {
                    treeDetail.X = 0;
                    treeDetail.Y = 0;
                }
            }

            //treeDetail.X = Convert.ToDecimal(txtX.Text);
            //treeDetail.Y = Convert.ToDecimal(txtY.Text);
            //treeDetail.Lat = Convert.ToDecimal(txtLat.Text);
            //treeDetail.Lon = Convert.ToDecimal(txtLon.Text);

            treeDetail.Littoral = chkLittoral.Checked;
            treeDetail.MaritimeZone = chkMaritimeZone.Checked;

            // ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            treeDetail.EditedDate = DateTime.Now;
            treeDetail.EditorUserID = editor.UserID;

            ActionProposed actionProposed = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(rblActionProposed.SelectedValue));
            treeDetail.ActionProposedID = actionProposed.ActionProposedID;
            treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;

            Condition condition = new ConditionBLL().GetConditionByConditionID(Convert.ToInt32(rblCondition.SelectedValue));
            treeDetail.ConditionID = condition.ConditionID;
            treeDetail.ConditionReference.EntityKey = condition.EntityKey;

            treeDetail.ProjectOrganismID = project_Organisms.ProjectOrganismID;
            treeDetail.Project_OrganismsReference.EntityKey = project_Organisms.EntityKey;

            _DatabaseContext.SaveChanges();
            //
        }

        Type myType = (typeof(Project_Organisms));
        PropertyInfo[] props = myType.GetProperties();

        string[] arrNewValues = new string[e.NewValues.Keys.Count];
        e.NewValues.Keys.CopyTo(arrNewValues, 0);

        foreach (var prop in props)
        {
            if (("System.String,System.Int,System.DateTime,System.Guid,System.Boolean").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int, Boolean o DateTime
            {
                if (!arrNewValues.Contains(prop.Name))
                {
                    e.NewValues[prop.Name] = prop.GetValue(project_Organisms, null);
                }
            }
        }

        e.NewValues["ProjectID"] = RouteData.Values["Project_id"].ToString();
        e.NewValues["OrganismID"] = ((HiddenField)formViewTlProject_Organism.FindControl("hfOrganism")).Value;

        e.NewValues["EditorUserId"] = editor.UserID.ToString();
        e.NewValues["EditedDate"] = DateTime.Now;
    }

    protected void FormViewTlProject_Organism_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            // Display a user-friendly message 
            ltlMessage.Text = ExceptionManager.DoLogAndGetFriendlyMessageForException(e.Exception);
            // Indicate that the exception has been handled 
            e.ExceptionHandled = true;
            // Keep the current UI in edit mode 
            e.KeepInEditMode = true;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Update successful");

            string source = Request.QueryString["s"];
            if (source != null && source == "i")
            {
                Response.RedirectToRoute("tl-treeinventory", new { Project_id = RouteData.Values["Project_id"] });
                Response.RedirectLocation += Request.Url.Query;
            }
            else
            {
                Response.RedirectToRoute("tl", new { Project_id = RouteData.Values["Project_id"] });
                Response.RedirectLocation += "?poid=" + Page.RouteData.Values["project_organism_id"] as string + "&lat=0&lon=0";
            }
        }
    }

    #endregion

    protected void FormViewTlProject_Organism_DataBinding(object sender, EventArgs e)
    {
        string project_organismID = Page.RouteData.Values["project_organism_id"] as string;

        if (project_organismID != null && project_organismID != "0")
        {
            this.tree = new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID(Convert.ToInt32(project_organismID));
            this.editor = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(tree.EditorUserID);
            this.creator = new Eisk.BusinessLogicLayer.UserBLL().GetUserByUserID(tree.CreatorUserID);

            if (tree.TreeDetails.Count > 0)
                this.treeDetail = tree.TreeDetails.First();

            if (tree.Organism != null)
                this.organism = tree.Organism;

        }
    }

    protected void rblPosition_SelectedIndexChanged(object sender, EventArgs e)
    {
        Panel pnlNad83 = (Panel)formViewTlProject_Organism.FindControl("pnlNad83");
        Panel pnlState = (Panel)formViewTlProject_Organism.FindControl("pnlState");

        pnlNad83.Visible = ((RadioButtonList)sender).SelectedValue == "0";
        pnlState.Visible = ((RadioButtonList)sender).SelectedValue != "0";
    }


    protected void rblActionProposed_DataBound(object sender, EventArgs e)
    {
        int select = 5;

        string project_organismID = Page.RouteData.Values["project_organism_id"] as string;

        if (project_organismID != null && project_organismID != "0")
        {
            select = (int)treeDetail.ActionProposedID;
        }

        foreach (ListItem li in ((RadioButtonList)sender).Items)
        {
            ActionProposed ap = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(li.Value));
            li.Attributes.Add("style", "background-color: #" + ap.Color.Code + ";");
            if (li.Value == select.ToString())
            {
                li.Selected = true;
            }
        }
    }

    protected void rblCondition_DataBound(object sender, EventArgs e)
    {
        int select = 1;

        string project_organismID = Page.RouteData.Values["project_organism_id"] as string;

        if (project_organismID != null && project_organismID != "0")
        {
            select = (int)treeDetail.ConditionID;
        }

        foreach (ListItem li in ((RadioButtonList)sender).Items)
        {
            Condition ap = new ConditionBLL().GetConditionByConditionID(Convert.ToInt32(li.Value));
            if (li.Value == select.ToString())
            {
                li.Selected = true;
            }
        }
    }

    protected void btnAddDap_Click(object sender, EventArgs e)
    {
        GridView gvDaps = (GridView)formViewTlProject_Organism.FindControl("gvDaps");
        TextBox txtDapAdd = (TextBox)formViewTlProject_Organism.FindControl("txtDapAdd");
        Panel pnlAddDap = (Panel)formViewTlProject_Organism.FindControl("pnlAddDap");

        GridViewRowCollection rows = gvDaps.Rows;
        List<TempDap> objs = SetGV(rows, -1);

        objs.Add(new TempDap(gvDaps.Rows.Count + 1, Convert.ToDecimal(txtDapAdd.Text)));

        gvDaps.DataSource = objs;
        gvDaps.DataBind();

        txtDapAdd.Text = "";
        txtDapAdd.Focus();

        pnlAddDap.Height = Unit.Point(188 + (objs.Count * 27));
    }


    private List<TempDap> SetGV(GridViewRowCollection rows, int rowToDelete)
    {
        GridView gvDaps = (GridView)formViewTlProject_Organism.FindControl("gvDaps");
        List<TempDap> objs = new List<TempDap>();
        int count = rows.Count;
        for (int i = 0; i < count; i++)
        {
            GridViewRow row = gvDaps.Rows[i];
            string value = row.Cells[1].Text;

            objs.Add(new TempDap((rowToDelete != -1 && i >= rowToDelete) ? i : i + 1, Convert.ToDecimal(value)));
        }

        return objs;
    }

    protected void gvDaps_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView gvDaps = (GridView)formViewTlProject_Organism.FindControl("gvDaps");
        TextBox txtDapAdd = (TextBox)formViewTlProject_Organism.FindControl("txtDapAdd");
        Panel pnlAddDap = (Panel)formViewTlProject_Organism.FindControl("pnlAddDap");

        int RowIndex = ((GridViewRow)(((LinkButton)e.CommandSource).NamingContainer)).RowIndex;

        GridViewRowCollection rows = gvDaps.Rows;
        List<TempDap> objs = SetGV(rows, RowIndex);

        objs.RemoveAt(RowIndex);

        gvDaps.DataSource = objs;
        gvDaps.DataBind();

        txtDapAdd.Text = "";
        txtDapAdd.Focus();

        if (objs.Count == 0)
            pnlAddDap.Height = Unit.Point(215);
        else
            pnlAddDap.Height = Unit.Point(Convert.ToInt32(pnlAddDap.Height.Value) - 27);
    }

    protected void btnSaveAndBack_Click(object sender, EventArgs e)
    {
        GridView gvDaps = (GridView)formViewTlProject_Organism.FindControl("gvDaps");

        HiddenField hfDap = (HiddenField)formViewTlProject_Organism.FindControl("hfDap");
        TextBox txtDap = (TextBox)formViewTlProject_Organism.FindControl("txtDap");
        TextBox txtDapCounter = (TextBox)formViewTlProject_Organism.FindControl("txtDapCounter");

        GridViewRowCollection rows = gvDaps.Rows;
        List<TempDap> tempDaps = SetGV(rows, -1);

        double total = 0;
        string daps = "";
        foreach (TempDap tempDap in tempDaps)
        {
            total += Convert.ToDouble(tempDap.Dap);
            daps += Convert.ToDouble(tempDap.Dap).ToString() + ",";
        }
        daps = (daps.Count() > 1) ? daps.Substring(0, daps.Count() - 1) : daps;

        decimal dap = tempDaps.Count > 1 ? Convert.ToDecimal(Math.Round(Math.Sqrt(Math.Pow(total, 2D) / Convert.ToDouble(tempDaps.Count)) * 100) / 100) : Convert.ToDecimal(total);

        hfDap.Value = daps;
        txtDap.Text = dap.ToString();
        txtDapCounter.Text = tempDaps.Count == 0 ? "1" : tempDaps.Count.ToString();

        txtDap.Attributes.Add("style", "background-color: " + (tempDaps.Count <= 1 ? "#FFFFFF;" : "#E3E9EF;"));
        txtDap.Enabled = tempDaps.Count <= 1 ? true : false;

        btnBack_Click(sender, e);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        ((Panel)formViewTlProject_Organism.FindControl("pnlAddDap")).Visible = false;
        ((Panel)formViewTlProject_Organism.FindControl("pnlLoading")).Visible = false;

        ResetRblActionProposedColors();
        SetScientificName();
    }

    protected void btnAddTrunk_Click(object sender, EventArgs e)
    {
        Panel pnlAddDap = (Panel)formViewTlProject_Organism.FindControl("pnlAddDap");
        Panel pnlLoading = (Panel)formViewTlProject_Organism.FindControl("pnlLoading");
        GridView gvDaps = (GridView)formViewTlProject_Organism.FindControl("gvDaps");
        HiddenField hfDap = (HiddenField)formViewTlProject_Organism.FindControl("hfDap");
        TextBox txtDapAdd = (TextBox)formViewTlProject_Organism.FindControl("txtDapAdd");

        pnlLoading.Visible = true;
        pnlAddDap.Visible = true;

        string reqDap = hfDap.Value;

        if (!string.IsNullOrEmpty(reqDap) && hfDap.Value != "0")
        {
            List<object> objs = new List<object>();
            string[] daps = reqDap.Split(',');

            for (int i = 0; i < daps.Count(); i++)
            {
                objs.Add(new { Number = i + 1, Dap = daps[i] });
            }

            gvDaps.DataSource = objs;
            gvDaps.DataBind();

            pnlAddDap.Height = Unit.Point(188 + (gvDaps.Rows.Count * 27));
        }
        else
        {
            gvDaps.DataSource = null;
            gvDaps.DataBind();

            pnlAddDap.Height = Unit.Point(215);
        }

        txtDapAdd.Focus();
    }

    protected void btnResetTrunkCounter_Click(object sender, EventArgs e)
    {
        CheckBox cbCepa = new CheckBox();
        cbCepa.Checked = false;
        cbCepa_CheckedChanged(cbCepa, e);
    }

    protected void cbCepa_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbCepa = (CheckBox)sender;

        TextBox txtVaras = (TextBox)formViewTlProject_Organism.FindControl("txtVaras");
        TextBox txtDap = (TextBox)formViewTlProject_Organism.FindControl("txtDap");
        TextBox txtDapCounter = (TextBox)formViewTlProject_Organism.FindControl("txtDapCounter");

        HiddenField hfDap = (HiddenField)formViewTlProject_Organism.FindControl("hfDap");

        Panel pnlDap = (Panel)formViewTlProject_Organism.FindControl("pnlDap");
        Panel pnlVaras = (Panel)formViewTlProject_Organism.FindControl("pnlVaras");

        txtVaras.Text = "0";
        txtDap.Text = "0";
        hfDap.Value = "0";
        txtDapCounter.Text = "1";
        pnlDap.Visible = !cbCepa.Checked;
        pnlVaras.Visible = cbCepa.Checked;
        txtDap.Attributes.Add("style", "background-color: #FFFFFF;");
        txtDap.Enabled = !cbCepa.Checked;

        ResetRblActionProposedColors();
        SetScientificName();
    }

    protected void ResetRblActionProposedColors()
    {
        RadioButtonList rblActionProposed = (RadioButtonList)formViewTlProject_Organism.FindControl("rblActionProposed");

        foreach (ListItem li in rblActionProposed.Items)
        {
            ActionProposed ap = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(li.Value));
            li.Attributes.Add("style", "background-color: #" + ap.Color.Code + ";");
        }
    }

    protected void SetScientificName()
    {
        HiddenField hfOrganism = (HiddenField)formViewTlProject_Organism.FindControl("hfOrganism");

        if (!string.IsNullOrEmpty(hfOrganism.Value))
        {
            TextBox txtScientificName = (TextBox)formViewTlProject_Organism.FindControl("txtScientificName");
            Organism organism = new OrganismBLL().GetOrganismByOrganismID(Convert.ToInt32(hfOrganism.Value));
            txtScientificName.Text = organism.ScientificName.ScientificNameDesc;
        }
    }

}

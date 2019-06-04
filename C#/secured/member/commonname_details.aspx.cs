 
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

public partial class CommonName_Details : System.Web.UI.Page
{
    enum actionType
    {
        update,
        insert
    }

    protected Organism organism;

    #region "Select handlers"

    protected void OdsOrganism_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewCommonName.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewCommonName.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewCommonName.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");


        if (formViewCommonName.CurrentMode == FormViewMode.Insert)
        {
            e.Cancel = true;
        }
    }

    protected void FormViewCommonName_DataBound(object sender, System.EventArgs e)
    {
        if (Page.Request.UrlReferrer != null)
        {
            if (Page.Request.UrlReferrer.AbsolutePath.Contains("new") && !IsPostBack && Page.RouteData.Values["organism_id"] as string != "0")
            {
                ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Insert successful");
            }
        }

        if (Page.RouteData.Values["organism_id"] as string == "0")
        {
            if (!string.IsNullOrWhiteSpace(Page.RouteData.Values["commonname"] as string) && Page.RouteData.Values["commonname"] as string != "new")
            {
                TextBox txtCommonName = (TextBox)formViewCommonName.FindControl("txtCommonName");
                txtCommonName.Text = Page.RouteData.Values["commonname"] as string;
            }
        }

        TextBox txtScientificName = (TextBox)formViewCommonName.FindControl("txtScientificName");
        if (txtScientificName != null)
        {
            txtScientificName.Text = Page.RouteData.Values["scientificname_id"] as string == "0" ?
                "<--SELECCIONAR-->" :
                (new ScientificNameBLL().GetScientificNameByScientificNameID(Convert.ToInt32(Page.RouteData.Values["scientificname_id"] as string))).ScientificNameDesc;
        }

    }

    #endregion

    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (formViewCommonName.CurrentMode == FormViewMode.Insert)
        {
            formViewCommonName.InsertItem(true);
        }
        else
        {
            formViewCommonName.UpdateItem(true);
        }
    }

    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        string projectID = (this.RouteData.Values["project_id"] as string);

        Response.RedirectToRoute("commonname-details", new { edit_mode = "edit", project_id = projectID, organism_id = RouteData.Values["organism_id"], scientificname_id = Page.RouteData.Values["scientificname_id"] as string, commonname = "edit" });
    }

    protected void ButtonSelect_Click(object sender, EventArgs e)
    {
        TextBox txtCommonName = (TextBox)formViewCommonName.FindControl("txtCommonName");
        string commoName = string.IsNullOrWhiteSpace(txtCommonName.Text) || txtCommonName.Text == "new" ? "new" : txtCommonName.Text;

        int scientificname_id;
        scientificname_id = int.TryParse(Page.RouteData.Values["scientificname_id"] as string, out scientificname_id) ? scientificname_id : 0;

        string projectID = (this.RouteData.Values["project_id"] as string);
        Response.RedirectToRoute("scientificnameselect", new { edit_mode = "edit", project_id = projectID, organism_id = RouteData.Values["organism_id"], scientificname_id = scientificname_id, commonname = commoName });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/secured/member/commonnames.aspx", true); 
        string projectID = (this.RouteData.Values["project_id"] as string);

        Response.RedirectToRoute("commonnames", new { project_id = projectID });   
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewCommonName_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        TextBox txtCommonName = (TextBox)formViewCommonName.FindControl("txtCommonName");
        TextBox txtScientificName = (TextBox)formViewCommonName.FindControl("txtScientificName");
        CommonNameActionStatus status = Validate(txtCommonName.Text, txtScientificName.Text, actionType.insert);

        if (status == CommonNameActionStatus.Success)
        {
            User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

            int groupID = new Group_UsersBLL().GetGroup_UsersByUserID(user.UserID)[0].GroupID;

            CommonName commonName = new CommonNameBLL().GetOrCreateCommonName(txtCommonName.Text, user);
            ScientificName scientificName = new ScientificNameBLL().GetScientificNameByScientificNameID(Convert.ToInt32(Page.RouteData.Values["scientificname_id"] as string));
            OrganismType organismType = new OrganismTypeBLL().GetOrganismTypeByOrganismTypeID(2);
            Group group = new GroupBLL().GetGroupByGroupID(groupID);

            e.Values["CommonNameID"] = commonName.CommonNameID;
            e.Values["ScientificNameID"] = scientificName.ScientificNameID;
            e.Values["OrganismTypeID"] = organismType.OrganismTypeID;
            e.Values["GroupID"] = group.GroupID;

            e.Values["CreatedDate"] = DateTime.Now;
            e.Values["CreatorUserID"] = user.UserID;
            e.Values["EditedDate"] = DateTime.Now;
            e.Values["EditorUserID"] = user.UserID;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    private static CommonNameActionStatus Validate(string commonName, string scientificName, actionType act)
    {
        User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

        int groupID = new Group_UsersBLL().GetGroup_UsersByUserID(user.UserID)[0].GroupID;

        Organism organism = new OrganismBLL().GetOrganismByScientificNameCommonName(scientificName.Trim(), commonName.Trim());
        if (organism != null && organism.GroupID == groupID)
        {
            if (act == actionType.insert)
            {
                return CommonNameActionStatus.Duplicate;
            }
            else
                return CommonNameActionStatus.Success;
        }
        else
            return CommonNameActionStatus.Success;
    }

    public string GetErrorMessage(CommonNameActionStatus status)
    {
        switch (status)
        {
            case CommonNameActionStatus.Duplicate:
                return "Common Name already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsOrganism_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        string projectID = (this.RouteData.Values["project_id"] as string);

        if (result != 0)
        {
            Response.RedirectToRoute("commonname-details", new { edit_mode = "edit", project_id = projectID, organism_id = result, scientificname_id = Page.RouteData.Values["scientificname_id"] as string, commonname = "edit" });
        }
    }

    protected void formViewCommonName_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewCommonName_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtCommonName = (TextBox)formViewCommonName.FindControl("txtCommonName");
        TextBox txtScientificName = (TextBox)formViewCommonName.FindControl("txtScientificName");
        CommonNameActionStatus status = Validate(txtCommonName.Text, txtScientificName.Text, actionType.update);

        if (status == CommonNameActionStatus.Success)
        {
            Type myType = (typeof(Organism));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            Organism organism = new OrganismBLL().GetOrganismByOrganismID((int)e.Keys["OrganismId"]);

            User editor = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);

            CommonName commonName = new CommonNameBLL().GetOrCreateCommonName(txtCommonName.Text, editor);
            ScientificName scientificName = new ScientificNameBLL().GetScientificNameByScientificName(txtScientificName.Text);
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                if (commonName.CommonNameID == organism.CommonNameID)
                {
                    CommonName dbContCommonName = _DatabaseContext.CommonNames.First(instance => instance.CommonNameID == organism.CommonNameID);
                    dbContCommonName.CommonNameDesc = txtCommonName.Text;
                    dbContCommonName.EditorUserID = editor.UserID;
                    dbContCommonName.EditedDate = DateTime.Now;
                }
                else
                {
                    Organism dbContOrganism = _DatabaseContext.Organisms.First(instance => instance.OrganismID == organism.OrganismID);
                    dbContOrganism.CommonNameID = commonName.CommonNameID;
                    dbContOrganism.CommonNameReference.EntityKey = commonName.EntityKey;
                }

                if (scientificName.ScientificNameID != organism.ScientificNameID)
                {
                    Organism dbContOrganism = _DatabaseContext.Organisms.First(instance => instance.OrganismID == organism.OrganismID);
                    dbContOrganism.ScientificNameID = scientificName.ScientificNameID;
                    dbContOrganism.ScientificNameReference.EntityKey = scientificName.EntityKey;
                }

                _DatabaseContext.SaveChanges();
            }

            foreach (var prop in props)
            {
                if (("System.String,System.Int32,System.Int,System.DateTime,System.Guid").IndexOf((prop.PropertyType).FullName) >= 0) // Si la propiedad es de tipo Guid, String, Int o DateTime
                {
                    if (!arrNewValues.Contains(prop.Name))
                    {
                        e.NewValues[prop.Name] = prop.GetValue(organism, null);
                    }
                }
            }

            e.NewValues["ScientificNameID"] = scientificName.ScientificNameID.ToString();
            e.NewValues["CommonNameID"] = commonName.CommonNameID.ToString();
            e.NewValues["EditorUserID"] = editor.UserID.ToString();
            e.NewValues["EditedDate"] = DateTime.Now;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    protected void formViewCommonName_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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
            ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Update successful");
    }

    #endregion

    protected void formViewCommonName_DataBinding(object sender, EventArgs e)
    {
        if (Page.RouteData.Values["organism_id"] as string != "0")
        {
            organism = new OrganismBLL().GetOrganismByOrganismID(Convert.ToInt32(Page.RouteData.Values["organism_id"] as string));
        }
    }
}

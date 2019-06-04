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

public partial class Organism_Details : System.Web.UI.Page
{
    enum actionType
    {
        update,
        insert
    }

    #region "Select handlers"

    protected void OdsOrganism_Details_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        string editMode = Page.RouteData.Values["edit_mode"] as string;

        if (editMode == "view")
            formViewOrganism.ChangeMode(FormViewMode.ReadOnly);
        else if (editMode == "edit")
            formViewOrganism.ChangeMode(FormViewMode.Edit);
        else if (editMode == "new")
            formViewOrganism.ChangeMode(FormViewMode.Insert);
        else
            throw new InvalidOperationException("error");


        if (formViewOrganism.CurrentMode == FormViewMode.Insert)
            e.Cancel = true;
    }

    protected void FormViewOrganism_DataBound(object sender, System.EventArgs e)
    {
        if (Page.Request.UrlReferrer != null)
            if (Page.Request.UrlReferrer.AbsolutePath.Contains("new") && !IsPostBack)
            {
                ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage("Insert successful");
            }

        DropDownList ddlOrganismType = (DropDownList)formViewOrganism.FindControl("ddlOrganismType");
        if (ddlOrganismType != null)
            ddlOrganismType.SelectedValue = (Page.RouteData.Values["organismtype_id"] as string);
    }

    #endregion

    #region "Command Handlers"

    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        if (formViewOrganism.CurrentMode == FormViewMode.Insert)
        {
            formViewOrganism.InsertItem(true);
        }
        else
        {
            formViewOrganism.UpdateItem(true);
        }
    }


    protected void ButtonEdit_Click(object sender, EventArgs e)
    {
        Response.RedirectToRoute(new { edit_mode = "edit", Organism_id = RouteData.Values["Organism_id"], organismtype_id = RouteData.Values["organismtype_id"] });
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/pages/organisms.aspx", true);
    }

    #endregion

    #region "Insert handlers"

    protected void FormViewOrganism_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        TextBox txtCommonName = (TextBox)formViewOrganism.FindControl("txtCommonName");
        TextBox txtScientificName = (TextBox)formViewOrganism.FindControl("txtScientificName");
        DropDownList ddlOrganismType = (DropDownList)formViewOrganism.FindControl("ddlOrganismType");
        OrganismActionStatus status = Validate(txtCommonName.Text, txtScientificName.Text, Convert.ToInt32(ddlOrganismType.SelectedValue), actionType.insert);

        if (status == OrganismActionStatus.Success)
        {
            e.Values["OrganismTypeID"] = ddlOrganismType.SelectedValue;
            e.Values["CreatedDate"] = DateTime.Now;
            e.Values["EditedDate"] = DateTime.Now;
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    private static OrganismActionStatus Validate(string commonName, string scientificName, int organismTypeID, actionType act)
    {
        OrganismBLL organismBLL = new OrganismBLL();

        Organism organismCommonName = organismBLL.GetOrganismByScientificName(scientificName.Trim());
        if (organismCommonName != null)
        {
            if (organismCommonName.ScientificName.Trim() == scientificName.Trim() && act == actionType.insert)
            {
                return OrganismActionStatus.DuplicateScientificName;
            }
            else if (organismCommonName.CommonName.Trim() == commonName.Trim() && organismCommonName.ScientificName.Trim() == scientificName.Trim() && organismCommonName.OrganismTypeID == organismTypeID)
                return OrganismActionStatus.DuplicateCommonName;
            else
                return OrganismActionStatus.Success;
        }
        else
            return OrganismActionStatus.Success;
    }

    public string GetErrorMessage(OrganismActionStatus status)
    {
        switch (status)
        {
            case OrganismActionStatus.DuplicateCommonName:
                return "Organism already exists. Please enter a different one.";
            case OrganismActionStatus.DuplicateScientificName:
                return "Organism Scientific Name already exists. Please enter a different one.";
            default:
                return "Error";
        }
    }

    protected void OdsOrganism_Details_Inserted(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
    {
        int result = Convert.ToInt32(e.ReturnValue, System.Globalization.CultureInfo.CurrentCulture.NumberFormat);

        if (result != 0)
        {
            Response.RedirectToRoute(new { edit_mode = "edit", Organism_id = result.ToString() });
        }
    }

    protected void formViewOrganism_ItemInserted(object sender, FormViewInsertedEventArgs e)
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

    protected void FormViewOrganism_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        TextBox txtCommonName = (TextBox)formViewOrganism.FindControl("txtCommonName");
        TextBox txtScientificName = (TextBox)formViewOrganism.FindControl("txtScientificName");
        DropDownList ddlOrganismType = (DropDownList)formViewOrganism.FindControl("ddlOrganismType");
        OrganismActionStatus status = Validate(txtCommonName.Text, txtScientificName.Text, Convert.ToInt32(ddlOrganismType.SelectedValue), actionType.update);

        if (status == OrganismActionStatus.Success)
        {
            Type myType = (typeof(Organism));
            PropertyInfo[] props = myType.GetProperties();

            string[] arrNewValues = new string[e.NewValues.Keys.Count];
            e.NewValues.Keys.CopyTo(arrNewValues, 0);

            OrganismType organismType = new OrganismTypeBLL().GetOrganismTypeByOrganismTypeID(Convert.ToInt32(ddlOrganismType.SelectedValue));
            Organism organism = new OrganismBLL().GetOrganismByOrganismId2((int)e.Keys["OrganismId"]);
            
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

            e.NewValues["OrganismTypeID"] = organismType.OrganismTypeID;
            Page.RouteData.Values["organismtype_id"] = organismType.OrganismTypeID.ToString();
        }
        else
        {
            ltlMessage.Text = MessageFormatter.GetFormattedErrorMessage(GetErrorMessage(status));
            e.Cancel = true;
        }
    }

    protected void formViewOrganism_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
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
}

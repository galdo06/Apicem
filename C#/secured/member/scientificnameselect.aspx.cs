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
using Eisk.BusinessEntities;
using Eisk.DataAccessLayer;
using System.Linq;

public partial class ScientificNameSelect_Page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Page.RouteData.Values["organism_id"] as string))
        {
            if (Page.RouteData.Values["organism_id"] as string != "0")
            {
                Organism organism = new OrganismBLL().GetOrganismByOrganismID(Convert.ToInt32(Page.RouteData.Values["organism_id"] as string));
                lblScientificName.Text = organism.ScientificName.ScientificNameDesc;
                lblCommonName.Text = organism.CommonName.CommonNameDesc;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Page.RouteData.Values["commonname"] as string) && Page.RouteData.Values["commonname"] as string != "new")
                {
                    lblCommonName.Text = Page.RouteData.Values["commonname"] as string;
                }
                else
                {
                    pnlCommonName.Visible = false;
                }
                pnlScientificName.Visible = false;
            }
        }
    }

    protected void ButtonGoToListPage_Click(object sender, EventArgs e)
    {
        string projectID = (this.RouteData.Values["project_id"] as string);

        if (!string.IsNullOrEmpty(Page.RouteData.Values["organism_id"] as string) && Page.RouteData.Values["organism_id"] as string != "0")
        {
            Response.RedirectToRoute("commonname-details", new { edit_mode = "edit", project_id = projectID, organism_id = Page.RouteData.Values["organism_id"] as string, scientificname_id = Page.RouteData.Values["scientificname_id"] as string, commonname = Page.RouteData.Values["commonname"] as string });
        }
        else
        {
            int scientificname_id;
            scientificname_id = int.TryParse(Page.RouteData.Values["scientificname_id"] as string, out scientificname_id) ? scientificname_id : 0;

            string commonname = !string.IsNullOrEmpty(Page.RouteData.Values["commonname"] as string) ? Page.RouteData.Values["commonname"] as string : "new";

            Response.RedirectToRoute("commonname-details", new { edit_mode = "new", project_id = projectID, organism_id = 0, scientificname_id = scientificname_id, commonname = Page.RouteData.Values["commonname"] as string });
        }
    }

    protected void gridViewScientificNames_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string projectID = (this.RouteData.Values["project_id"] as string);

        if (e.CommandName != "Page")
        {
            string scientificNameIDString = e.CommandArgument.ToString();

            if (!string.IsNullOrEmpty(Page.RouteData.Values["organism_id"] as string))
            {
                int organismID = Convert.ToInt32(Page.RouteData.Values["organism_id"] as string);
                int scientificNameID = Convert.ToInt32(scientificNameIDString);

                if (Page.RouteData.Values["organism_id"] as string != "0")
                {
                    //using (DatabaseContext _DatabaseContext = new DatabaseContext())
                    //{
                    //    Organism organism = _DatabaseContext.Organisms.First(instance => instance.OrganismID == organismID);
                    //    ScientificName scientificName = _DatabaseContext.ScientificNames.First(instance => instance.ScientificNameID == scientificNameID);

                    //    organism.ScientificNameID = scientificName.ScientificNameID;
                    //    organism.ScientificNameReference.EntityKey = scientificName.EntityKey;
                    //    organism.EditedDate = DateTime.Now;

                    //    _DatabaseContext.SaveChanges();
                    //}
                        Response.RedirectToRoute("commonname-details", new { edit_mode = "edit", project_id = projectID, organism_id = organismID.ToString(), scientificname_id = scientificNameID, commonname = Page.RouteData.Values["commonname"] as string });
                }
                else
                {
                        Response.RedirectToRoute("commonname-details", new { edit_mode = "new", project_id = projectID, organism_id = organismID.ToString(), scientificname_id = scientificNameID, commonname = Page.RouteData.Values["commonname"] as string });
                }
            }
        }
    }

    protected void odsScientificNameListing_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

    }

    protected void ddlScientificNameType_SelectedIndexChanged(object sender, EventArgs e)
    {
        odsScientificNameListing.Select();
    }
}

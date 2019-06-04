 
using System;
using System.Web.UI.WebControls;

namespace Eisk.TestHelpers
{
    public sealed class UITestDataHelper
    {
        public static void LoadUITestDataFromFormView(FormView formViewEmployee)
        {
            TextBox txtFirstName = (TextBox)formViewEmployee.FindControl("txtFirstName");
            TextBox txtLastName = (TextBox)formViewEmployee.FindControl("txtLastName");
            TextBox txtHireDate = (TextBox)formViewEmployee.FindControl("txtHireDate");
            TextBox txtAddress = (TextBox)formViewEmployee.FindControl("txtAddress");
            TextBox txtHomePhone = (TextBox)formViewEmployee.FindControl("txtHomePhone");
            DropDownList ddlCountry = (DropDownList)formViewEmployee.FindControl("ddlCountry");

            //since in read-only mode there is no text box control
            if (formViewEmployee.CurrentMode != FormViewMode.ReadOnly)
            {

                ////using data value in form in custom way

                //populating per-fill data
                if (formViewEmployee.CurrentMode == FormViewMode.Insert)
                {
                    txtFirstName.Text = "Ashraful";
                    txtLastName.Text = "Alam";
                    txtHireDate.Text = DateTime.Now.ToString();
                    txtAddress.Text = "One Microsoft Way";
                    txtHomePhone.Text = "912200022";
                    ddlCountry.Items.FindByText("USA").Selected = true;
                }
            }

        }
    }
}
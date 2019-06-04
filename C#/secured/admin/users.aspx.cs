
using Eisk.BusinessEntities;
using Eisk.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eisk.Web
{
    public partial class User : System.Web.UI.Page
    {
        protected void ButtonAddNewUser_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("~/secured/admin/createuser.aspx");
        }

        protected Role GetRole(Guid userID)
        {
            return GetRoles(userID)[0];
        }

        protected List<Role> GetRoles(Guid userID)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users
                    .FirstOrDefault(s => s.UserID == userID)
                    .Role_Users
                        .Select(instance2 => instance2.Role)
                        .ToList();
            }
        }

        protected Eisk.BusinessEntities.Group GetGroup(Guid userID)
        {
            return GetGroups(userID)[0];
        }

        protected List<Eisk.BusinessEntities.Group> GetGroups(Guid userID)
        {
            using (var context = new DatabaseContext())
            {
                return context.Users
                    .FirstOrDefault(s => s.UserID == userID)
                      .Group_Users
                        .Select(instance2 => instance2.Group)
                        .ToList();
            }
        }

        protected void odsUserListing_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            txtFilter.Focus();
        }
    }
}
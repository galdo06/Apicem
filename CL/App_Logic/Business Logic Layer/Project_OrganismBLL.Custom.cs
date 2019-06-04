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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;

namespace Eisk.BusinessLogicLayer
{
    public partial class Project_OrganismsBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Project_Organisms GetProject_OrganismByProjectIDOrganismID(int projectID, int organismID)
        {
            return (
                from project_Organisms
                        in _DatabaseContext.Project_Organisms
                where project_Organisms.OrganismID == organismID
                && project_Organisms.ProjectID == projectID
                select project_Organisms).ToList()[0];

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public void DeleteProject_OrganismByProjectOrganismID(int projectOrganismID)
        {
            int projectOrganismIDINT = Convert.ToInt32(projectOrganismID);
            Project_Organisms project_Organism = _DatabaseContext.Project_Organisms.First(instance => instance.ProjectOrganismID == projectOrganismIDINT);

            DeleteProject_OrganismByProjectOrganismID(project_Organism);
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public void DeleteProject_OrganismByProjectOrganismID(Project_Organisms project_Organism)
        {
            int treeDetailsIDINT = project_Organism.TreeDetails.ToArray()[0].TreeDetailsID;
            TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.TreeDetailsID == treeDetailsIDINT);

            List<Dap> tempDaps = _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetailsIDINT).ToList();

            foreach (Dap dap in tempDaps)
            { 
                _DatabaseContext.Daps.DeleteObject(dap);
            }

            _DatabaseContext.TreeDetails.DeleteObject(treeDetail);
            _DatabaseContext.Project_Organisms.DeleteObject(project_Organism);

            _DatabaseContext.SaveChanges();

            //Sort Numbers after deleting
            int projectIDINT = Convert.ToInt32(project_Organism.ProjectID);
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
    }
}

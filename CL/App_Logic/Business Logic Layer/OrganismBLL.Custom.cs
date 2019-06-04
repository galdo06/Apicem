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
using Eisk.Web.App_Logic.Business_Logic_Layer;
using System.Web;

namespace Eisk.BusinessLogicLayer
{
    public partial class OrganismBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByNames(string CommonName = default(string), string ScientificName = default(string), int organismTypeID = 0)
        {
            return _DatabaseContext.Organisms.FirstOrDefault(
                instance => instance.CommonName.CommonNameDesc.Trim() == CommonName
                    && instance.ScientificName.ScientificNameDesc.Trim() == ScientificName
                    && instance.OrganismTypeID == organismTypeID);
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByScientificNameCommonName(string ScientificName = default(string), string CommonName = default(string))
        {
            return _DatabaseContext.Organisms.FirstOrDefault(instance =>
                        instance.ScientificName.ScientificNameDesc.Trim().ToUpper() == ScientificName.ToUpper()
                     && instance.CommonName.CommonNameDesc.Trim().ToUpper() == CommonName.ToUpper());
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByScientificName(string ScientificName = default(string))
        {
            return _DatabaseContext.Organisms.FirstOrDefault(instance =>
                     instance.ScientificName.ScientificNameDesc.Trim().ToUpper() == ScientificName.ToUpper());
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByCommonName(string CommonName = default(string))
        {
            return _DatabaseContext.Organisms.FirstOrDefault(instance =>
                     instance.CommonName.CommonNameDesc.Trim().ToUpper() == CommonName.ToUpper());
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismInProjectByNames(string CommonName = default(string), string ScientificName = default(string), int projectID = 0)
        {
            ProjectBLL projectBLL = new ProjectBLL();
            Project project = projectBLL.GetProjectByProjectID(projectID);

            Organism organism = _DatabaseContext.Organisms.FirstOrDefault(instance => instance.CommonName.CommonNameDesc == CommonName && instance.ScientificName.ScientificNameDesc == ScientificName);

            Organism projectOrganism = _DatabaseContext.Project_Organisms.First(instance => instance.ProjectID == projectID && instance.OrganismID == organism.OrganismID).Organism;

            return projectOrganism;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByOrganismId2(int organismID)
        {
            //Validate Input
            if (organismID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("organismID");
            Organism organism = (_DatabaseContext.Organisms.FirstOrDefault(instance => instance.OrganismID == organismID));

            var OrganismTypeName = organism.OrganismType.OrganismTypeName;
            var CommonNameDesc = organism.CommonName.CommonNameDesc;
            var ScientificNameDesc = organism.ScientificName.ScientificNameDesc;
            return organism;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Organism> GetOrganismByFilter(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "commonName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<Organism> organismsList = (
                from organisms in
                    _DatabaseContext.Organisms
                where
                    (
                        (
                            string.IsNullOrEmpty(organism)
                            || organisms.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                        &&
                        (
                            organismTypeID == 0
                            || organisms.OrganismTypeID == organismTypeID
                        )
                    )
                select (organisms)
            )
            .OrderBy(instance => instance.CommonName.CommonNameDesc)
            .Skip(startRowIndex)
            .Take(maximumRows)
            .ToList();

            if (organismsList.Count > 0)
            {
                foreach (Organism org in organismsList)
                {
                    var organismType = org.OrganismType;
                    var commonName = org.CommonName;
                    var scientificName = org.ScientificName;
                }
            }
            return organismsList;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Organism> GetOrganismByFilter2(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int groupID = 0)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "commonName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

           var organismsList = (
                from organisms in
                    _DatabaseContext.Organisms
                where
                    (
                        (
                            string.IsNullOrEmpty(organism)
                            || organisms.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                        &&
                        (
                            organismTypeID == 0
                            || organisms.OrganismTypeID == organismTypeID
                        )
                        &&
                        (
                            groupID == 0
                            || organisms.GroupID == groupID
                        )
                    )
                select (organisms)
            )
            .Distinct()
            .OrderBy(instance => instance.CommonName.CommonNameDesc);

           HttpContext.Current.Items["organismsList_Count"] = organismsList.Count();

            var returnList = organismsList.Skip(startRowIndex).Take(maximumRows).ToList();

            // Hack para que se cree una instancia y no explote
            if (returnList.Count > 0)
            {
                foreach (Organism org in returnList)
                {
                    var organismType = org.OrganismType;
                    var commonName = org.CommonName;
                    var scientificName = org.ScientificName;
                }
            }

            return returnList;
        }

        public int GetTotalCountForAllOrganisms2(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int groupID = 0)
        {

            var count = (int)HttpContext.Current.Items["organismsList_Count"];
            HttpContext.Current.Items.Remove("organismsList_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
             
            var organismsVar = (
                from organisms in
                    _DatabaseContext.Organisms
                where
                    (
                       (
                            string.IsNullOrEmpty(organism)
                            || organisms.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                        &&
                        (
                            organismTypeID == 0
                            || organisms.OrganismTypeID == organismTypeID
                        )
                        &&
                        (
                            groupID == 0
                            || organisms.GroupID == groupID
                        )
                    )
                select (organisms)
            );

            List<Organism> organismsList = organismsVar.ToList();
            if (organismsList.Count > 0)
            {
                var x = organismsList[0].OrganismType.OrganismTypeName;
            }
            return organismsList.Count();
             */
        }

        public int GetTotalCountForAllOrganisms(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0)
        {
            var organismsVar = (
                from organisms in
                    _DatabaseContext.Organisms
                where
                    (
                       (
                            string.IsNullOrEmpty(organism)
                            || organisms.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                        &&
                        (
                            organismTypeID == 0
                            || organisms.OrganismTypeID == organismTypeID
                        )
                    )
                select (organisms)
            );

            List<Organism> organismsList = organismsVar.ToList();
            if (organismsList.Count > 0)
            {
                var x = organismsList[0].OrganismType.OrganismTypeName;
            }
            return organismsList.Count();
        }


        public void DeleteProject_Organisms(List<int> organismIDsToDelete, int projectID)
        {
            Project_OrganismsBLL Project_OrganismsBLL = new Project_OrganismsBLL();
            foreach (int organismID in organismIDsToDelete)
            {
                Project_Organisms Project_Organisms = Project_OrganismsBLL.GetProject_OrganismByProjectIDOrganismID(organismID, projectID);
                Project_OrganismsBLL.DeleteProject_Organisms(Project_Organisms);
            }
        }
        // --------------------------------------------------------------------------------------------------------------------------------------------------------------

        public int GetTotalCountForAllOrganismsInProject(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int projectID = 0)
        {
            var count = (int)HttpContext.Current.Items["organisms_Count"];
            HttpContext.Current.Items.Remove("organisms_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
             
            // if (string.IsNullOrEmpty(orderBy))
            // orderBy = "commonName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var organisms = (
                from projectOrganisms in
                    _DatabaseContext.Project_Organisms
                where
                      (projectOrganisms.ProjectID == projectID)
                select (projectOrganisms.Organism)
            )
            .Where
            (
                instance2 =>
                (
                     string.IsNullOrEmpty(organism)
                    || instance2.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                    || instance2.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                )
                &&
                (
                    organismTypeID == 0
                    || instance2.OrganismTypeID == organismTypeID
                )
            );

            return organisms.Count();
             */
        }

        /*
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Organism> GetOrganismInProjectByFilter(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int projectID = 0)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "commonName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var organisms = (
                from projectOrganisms in
                    _DatabaseContext.Project_Organisms
                where
                      (projectOrganisms.ProjectID == projectID)
                select (projectOrganisms.Organism)
            )
            .Where
            (
                instance2 =>
                (
                     string.IsNullOrEmpty(organism)
                    || instance2.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                    || instance2.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                )
                &&
                (
                    organismTypeID == 0
                    || instance2.OrganismTypeID == organismTypeID
                )
            )
            .OrderBy(instance => instance.CommonName)
            .OrderByDescending(instance => instance.EditedDate);

            HttpContext.Current.Items["organisms_Count"] = organisms.Count();

            var returnOrganismsList = organisms.Skip(startRowIndex).Take(maximumRows).ToList();

            return returnOrganismsList.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Organism> GetOrganismSelectByFilter(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int projectID = 0)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "commonName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var projectOrganismsList = (
                from projectOrganisms in
                    _DatabaseContext.Project_Organisms
                where
                      (projectOrganisms.ProjectID == projectID)
                select (projectOrganisms.Organism)
            );

            var organismsList = (
                from organisms in _DatabaseContext.Organisms
                where !(from projectOrganisms in projectOrganismsList
                        select projectOrganisms.OrganismID)
                       .Contains(organisms.OrganismID)
                       && (
                            organismTypeID == 0
                            || organisms.OrganismTypeID == organismTypeID
                            )
                       &&
                       (
                            string.IsNullOrEmpty(organism)
                            || organisms.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                select organisms
            )
            .OrderBy(instance => instance.CommonName);

            HttpContext.Current.Items["organisms_Count"] = organismsList.Count();

            var returnOrganismsList = organismsList.Skip(startRowIndex).Take(maximumRows).ToList();

            return returnOrganismsList.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public int GetTotalCountForAllOrganismsSelect(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int projectID = 0)
        {
            var count = (int)HttpContext.Current.Items["organisms_Count"];
            HttpContext.Current.Items.Remove("organisms_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
                        
            ////if (string.IsNullOrEmpty(orderBy))
            ////    orderBy = "commonName";

            //if (startRowIndex < 0)
            //    throw (new ArgumentOutOfRangeException("startRowIndex"));

            //if (maximumRows < 0)
            //    throw (new ArgumentOutOfRangeException("maximumRows"));

            //var projectOrganismsList = (
            //    from projectOrganisms in
            //        _DatabaseContext.Project_Organisms
            //    where
            //          (projectOrganisms.ProjectID == projectID)
            //    select (projectOrganisms.Organism)
            //);

            //var organismsList = (
            //    from organisms in _DatabaseContext.Organisms
            //    where !(from projectOrganisms in projectOrganismsList
            //            select projectOrganisms.OrganismID)
            //           .Contains(organisms.OrganismID)
            //           && (
            //                organismTypeID == 0
            //                || organisms.OrganismTypeID == organismTypeID
            //                )
            //           &&
            //           (
            //                string.IsNullOrEmpty(organism)
            //                || organisms.CommonName.CommonNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
            //                || organisms.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(organism.ToUpper().Trim())
            //            )
            //    select organisms
            //);
            //
            //return organismsList.ToList().Count;
        }
        */
        
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<ExportData> GetExportData(string orderBy = default(string), int projectID = 0)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "commonName";

            //List<Project_Organisms> projectOrganismsList =
            List<ExportData> exportDataList = (
               from projectOrganisms in
                   _DatabaseContext.Project_Organisms
               where
                     (projectOrganisms.ProjectID == projectID)
               select projectOrganisms
           ).Select(
               projectOrganism =>
                   new ExportData()
                   {
                       ProjectID = projectOrganism.ProjectID,
                       ProjectName = projectOrganism.Project.ProjectInfoes.FirstOrDefault().ProjectName,
                       OrganismTypeID = projectOrganism.Organism.OrganismTypeID,
                       OrganismTypeName = projectOrganism.Organism.OrganismType.OrganismTypeName,
                       OrganismID = projectOrganism.Organism.OrganismID,
                       CommonName = projectOrganism.Organism.CommonName.CommonNameDesc,
                       ScientificName = projectOrganism.Organism.ScientificName.ScientificNameDesc
                   }).ToList();

            return exportDataList;
        }



    }
}

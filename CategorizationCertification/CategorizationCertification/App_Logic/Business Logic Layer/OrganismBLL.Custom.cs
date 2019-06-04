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

namespace Eisk.BusinessLogicLayer
{
    public partial class OrganismBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByNames(string CommonName = default(string), string ScientificName = default(string), int organismTypeID = 0)
        {
            return _DatabaseContext.Organisms.FirstOrDefault(
                instance => instance.CommonName.Trim() == CommonName
                    && instance.ScientificName.Trim() == ScientificName
                    && instance.OrganismTypeID == organismTypeID);
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismByScientificName(string ScientificName = default(string))
        {
            return _DatabaseContext.Organisms.FirstOrDefault(instance =>
                     instance.ScientificName.Trim() == ScientificName);
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Organism GetOrganismInProjectByNames(string CommonName = default(string), string ScientificName = default(string), int projectID = 0)
        {
            ProjectBLL projectBLL = new ProjectBLL();
            Project project = projectBLL.GetProjectByProjectID(projectID);

            Organism organism = _DatabaseContext.Organisms.FirstOrDefault(instance => instance.CommonName == CommonName && instance.ScientificName == ScientificName);

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

            var ass = organism.OrganismType.OrganismTypeName;
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

            var organismsVar = (
                from organisms in
                    _DatabaseContext.Organisms
                where
                    (
                       (
                            string.IsNullOrEmpty(organism)
                            || organisms.CommonName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                        &&
                        (
                            organismTypeID == 0
                            || organisms.OrganismTypeID == organismTypeID
                        )
                    )
                select (organisms)
            )
            .OrderBy(instance => instance.CommonName)
            .Skip(startRowIndex)
            .Take(maximumRows);

            List<Organism> organismsList = organismsVar.ToList();
            if (organismsList.Count > 0)
            {
                var x = organismsList[0].OrganismType.OrganismTypeName;
            }
            return organismsList;
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
                            || organisms.CommonName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
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
                    || instance2.CommonName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                    || instance2.ScientificName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                )
                &&
                (
                    organismTypeID == 0
                    || instance2.OrganismTypeID == organismTypeID
                )
            );

            return organisms.Count();
        }

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
                    || instance2.CommonName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                    || instance2.ScientificName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                )
                &&
                (
                    organismTypeID == 0
                    || instance2.OrganismTypeID == organismTypeID
                )
            )

            .OrderBy(instance => instance.CommonName)
            .OrderByDescending(instance => instance.EditedDate)
            .Skip(startRowIndex)
            .Take(maximumRows);

            return organisms.ToList();
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
                            || organisms.CommonName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                select organisms
            )
            .OrderBy(instance => instance.CommonName)
            .Skip(startRowIndex)
            .Take(maximumRows);

            return organismsList.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public int GetTotalCountForAllOrganismsSelect(string organism = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int organismTypeID = 0, int projectID = 0)
        {
            //if (string.IsNullOrEmpty(orderBy))
            //    orderBy = "commonName";

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
                            || organisms.CommonName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                            || organisms.ScientificName.ToUpper().Trim().Contains(organism.ToUpper().Trim())
                        )
                select organisms
            );

            return organismsList.ToList().Count;
        }

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
                        ProjectName = projectOrganism.Project.ProjectName,
                        OrganismTypeID = projectOrganism.Organism.OrganismTypeID,
                        OrganismTypeName = projectOrganism.Organism.OrganismType.OrganismTypeName,
                        OrganismID = projectOrganism.Organism.OrganismID,
                        CommonName = projectOrganism.Organism.CommonName,
                        ScientificName = projectOrganism.Organism.ScientificName
                    }).ToList();

             return exportDataList;
        }



    }
}

using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Web;
using Ws.Validations;
using WS.Controllers;

namespace WS.Models
{


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectNameValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string projectName;
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                this.ErrorMessage = "Nombre del Proyecto es requerido";
                return false;
            }

            projectName = value.ToString();

            var user = (User)HttpContext.Current.Items["user"];

            List<Group> groups = UserModel.GetUserGroups(user);

            if (!ProjectModel.ValidateProjectName(projectName, groups))
            {
                this.ErrorMessage = "Nombre del Proyecto existente";
                return false;
            }

            HttpContext.Current.Items["projectName"] = projectName;
            HttpContext.Current.Items["groups"] = groups;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LatValidationAttribute : ValidationAttribute
    {
        private decimal minLat = Convert.ToDecimal(ConfigurationManager.AppSettings["LatMinValue"]);
        private decimal maxLat = Convert.ToDecimal(ConfigurationManager.AppSettings["LatMaxValue"]);


        public LatValidationAttribute()
            : base("La localización es incorrecta. No se encuentra entre el territorio Puertorriqueño")
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            decimal lat = (decimal)value;

            if ((lat == 0) || lat == 37.78583526611328m) // Default del iPhone
                return true;

            if (lat < this.minLat || lat > this.maxLat)

                return false;

            return true;

        }

    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LonValidationAttribute : ValidationAttribute
    {
        private decimal minLon = Convert.ToDecimal(ConfigurationManager.AppSettings["LonMinValue"]);
        private decimal maxLon = Convert.ToDecimal(ConfigurationManager.AppSettings["LonMaxValue"]);


        public LonValidationAttribute()
            : base("La localización es incorrecta. No se encuentra entre el territorio Puertorriqueño")
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            decimal lon = (decimal)value;

            if ((lon == 0) || lon == -122.4064178466797m)// Default del iPhone
                return true;

            if ((lon < this.minLon || lon > this.maxLon) || lon == -122.4064178466797m)

                return false;

            return true;

        }

    }


    public class ProjectModel
    {
        [ProjectNameValidation]
        public string ProjectName { get; set; }

        [CityIDValidation]
        public int CityID { get; set; }

        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Zip Code inválido")]
        //[ZipCodeValidation]
        public string ZipCode { get; set; }

        [StringLength(5000, MinimumLength = 1, ErrorMessage = "Su comentario exede el límite de caracteres")]
        public string Comments { get; set; }

        [StringLength(5000, MinimumLength = 1, ErrorMessage = "Su descripción exede el límite de caracteres")]
        public string Description { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Su dirección línea 1 exede el límite de caracteres")]
        public string Address1 { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Su dirección línea 2 exede el límite de caracteres")]
        public string Address2 { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "El estado exede el límite de caracteres")]
        public string State { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre del cliente exede el límite de caracteres")]
        public string Client { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------

        [Required]
        [Range(typeof(decimal), "0", "99999.99", ErrorMessage = "El números de Acres exeden el valor máximo")]
        public decimal Acres { get; set; }

        [Required]
        [Range(typeof(int), "0", "999999999", ErrorMessage = "El números de Estacionamientos exeden el valor máximo")]
        public int Parkings { get; set; }

        [Required]
        [Range(typeof(int), "10", "30", ErrorMessage = "La Distancia entre Árboles exede el valor máximo")]
        public int DistanceBetweenTrees { get; set; }

        [Required]
        [Range(typeof(int), "0", "999999999", ErrorMessage = "La cantidad de Solares exede el valor máximo")]
        public int Lots { get; set; }

        [LatValidationAttribute]
        public decimal? Lat { get; set; }

        [LonValidationAttribute]
        public decimal? Lon { get; set; }

        //-----------------------------------------------------------------------------------------------------------------------

        public static bool IsAuthorized(Project project, List<Group> groups)
        {
            return groups.Contains(project.Group_Projects.First().Group);
        }

        public static bool ValidateProjectName(string projectName, List<Group> groups)
        {
            List<Project> projects = new ProjectBLL().GetProjectByFilter(projectName.Trim());
            if (projects.Count == 0)
                return true;

            foreach (var project in projects)
            {
                IEnumerable<Group> projectGroups = project.Group_Projects.Select(instance => instance.Group);

                foreach (var projectGroup in projectGroups)
                {
                    List<Int32> x = groups.Select(instance => instance.GroupID).ToList();
                    if (x.Contains(projectGroup.GroupID))
                        return false;
                }
            }

            return true;
        }

        public static object GetProjectObject(Project project)
        {
            ProjectInfoTreeLocation projectInfoTreeLocation = project.ProjectInfoTreeLocations.FirstOrDefault();
            ProjectInfo projectInfo = project.ProjectInfoes.FirstOrDefault();

            if (projectInfoTreeLocation == null)
            {
                projectInfoTreeLocation = new ProjectInfoTreeLocation();

                projectInfoTreeLocation.X = 0;
                projectInfoTreeLocation.Y = 0;
                projectInfoTreeLocation.Lat = 0;
                projectInfoTreeLocation.Lon = 0;
                projectInfoTreeLocation.Acres = 0;
                projectInfoTreeLocation.Lots1 = 0;
                projectInfoTreeLocation.Parkings = 0;
                projectInfoTreeLocation.DistanceBetweenTrees = 10;
                
                projectInfoTreeLocation.ProjectID = project.ProjectID;
                projectInfoTreeLocation.ProjectReference.EntityKey = project.EntityKey;

                new ProjectInfoTreeLocationBLL().CreateNewProjectInfoTreeLocation(projectInfoTreeLocation);
            }

            string zipCode = null;
            if (string.IsNullOrWhiteSpace(projectInfo.ZipCode))
            {

            }
            else if (projectInfo.ZipCode.Length == 5)
            {
                zipCode = projectInfo.ZipCode;
            }
            else if (projectInfo.ZipCode.Length == 9)
            {
                zipCode = projectInfo.ZipCode.Substring(0, 5) + "-" + projectInfo.ZipCode.Substring(5, 4);
            }

            return new
            {
                projectInfo.ProjectName,
                projectInfoTreeLocation.ProjectID,
                Acres = projectInfoTreeLocation.Acres.ToString(),
                DistanceBetweenTrees = projectInfoTreeLocation.DistanceBetweenTrees.ToString(),
                projectInfoTreeLocation.Lat,
                projectInfoTreeLocation.Lon,
                Lots = projectInfoTreeLocation.Lots1.ToString(),
                Parkings = projectInfoTreeLocation.Parkings.ToString(),
                projectInfoTreeLocation.X,
                projectInfoTreeLocation.Y,
                projectInfo.City.CityName,
                projectInfo.City.CityID,
                projectInfo.Client,
                CityX = projectInfo.City.X,
                CityY = projectInfo.City.Y,
                CityLat = projectInfo.City.Lat,
                CityLon = projectInfo.City.Lon,
                ZipCode = zipCode,
                projectInfo.Comments,
                projectInfo.Description,
                projectInfo.Address1,
                projectInfo.Address2,
                projectInfo.State,
                Perimeters = project.Perimeters.Select(i => new
                {
                    PerimeterID = i.PerimeterID,
                    ColorID = i.ColorID,
                    Code = i.Color.Code,
                    ColorDesc = i.Color.ColorDesc,
                    PerimeterName = i.PerimeterName,
                    PerimeterPoints = i.PerimeterPoints.Select(e => new
                    {
                        PerimeterPointID = e.PerimeterPointID,
                        X = e.X,
                        Y = e.Y
                    })
                }),
                Trees = project.Project_Organisms.Select(i =>
                    ProjectOrganismModel.GetProjectOrganismObject(i.TreeDetails.First())
               //     i.TreeDetails.First().ActionProposed.ActionProposedDesc,
               //     i.TreeDetails.First().ActionProposed.ActionProposedID,
               //     i.TreeDetails.First().ActionProposed.Color.ColorDesc,
               //     i.TreeDetails.First().ActionProposed.Color.ColorID,
               //     i.TreeDetails.First().ActionProposed.Color.Code,
               //     i.TreeDetails.First().Commentary,
               //     i.TreeDetails.First().Condition.ConditionDesc,
               //     i.TreeDetails.First().Condition.ConditionID,
               //     i.TreeDetails.First().CreatedDate,
               //     i.TreeDetails.First().CreatorUserID,
               //     i.TreeDetails.First().Dap,
               //     i.TreeDetails.First().Dap_Counter,
               //// Daps,//jsonSerialiser.Serialize(treeDetail.Daps.ToList()),
               //     i.TreeDetails.First().EditedDate,
               //     i.TreeDetails.First().EditorUserID,
               //     i.TreeDetails.First().Height,
               //     i.TreeDetails.First().Lat,
               //     i.TreeDetails.First().Lon,
               //     i.TreeDetails.First().X,
               //     i.TreeDetails.First().Y,
               //     i.TreeDetails.First().Number,
               //     i.TreeDetails.First().ProjectOrganismID,
               //     i.TreeDetails.First().Project_Organisms.ProjectID,
               //     i.TreeDetails.First().Project_Organisms.OrganismID,
               //     i.TreeDetails.First().Project_Organisms.Organism.CommonName.CommonNameDesc,
               //     i.TreeDetails.First().Project_Organisms.Organism.CommonName.CommonNameID,
               //     i.TreeDetails.First().Project_Organisms.Organism.ScientificName.ScientificNameDesc,
               //     i.TreeDetails.First().Project_Organisms.Organism.ScientificName.ScientificNameID,
               //     i.TreeDetails.First().TreeDetailsID,
               //     i.TreeDetails.First().Varas
               // }
               )
            };
        }

        public static bool IsNull(Project requestedProject, out Project Project)
        {
            bool isNull = (requestedProject == null || requestedProject == new Project());
            Project = isNull ? null : requestedProject;
            return isNull;
        }

    }
}
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using regex = System.Text.RegularExpressions;
using System.Web;
using Ws.Validations;
using WS.Controllers;

namespace WS.Models
{


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectNamUpdateValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string projectName;
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                this.ErrorMessage = "ProjectName es requerido";
                return false;
            }

            projectName = value.ToString();

            var user = (User)HttpContext.Current.Items["user"];

            List<Group> groups = UserModel.GetUserGroups(user);

            //if (!ProjectModel.ValidateProjectName(projectName, groups))
            //{
            //    this.ErrorMessage = "ProjectName existente";
            //    return false;
            //}

            HttpContext.Current.Items["projectName"] = projectName;
            HttpContext.Current.Items["groups"] = groups;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ZipCodeValidation : ValidationAttribute
    {
        public override bool IsValid(object zipCode)
        {
            if (zipCode == null || string.IsNullOrEmpty(zipCode.ToString()))
                return true;

            string pattern = @"^\\d{5}(\\-\\d{4})?$";
            regex.Regex regex = new regex.Regex(pattern);
            if (!regex.IsMatch(zipCode as String))
            {
                this.ErrorMessage = "Zip Code inválido";
                return false;
            }

            HttpContext.Current.Items["zipCode"] = zipCode;
            return true;
        }
    }

    public class ProjectModel_Update
    {
        [ProjectNamUpdateValidation]
        public string ProjectName { get; set; }

        [CityIDValidation]
        public int CityID { get; set; }

        [ProjectIDValidationAttribute]
        public int ProjectID { get; set; }

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
                    if (groups.Select(instance => instance.GroupID).Contains(projectGroup.GroupID))
                        return false;
                }
            }

            return true;
        }

        public static bool IsNull(Project requestedProject, out Project Project)
        {
            bool isNull = (requestedProject == null || requestedProject == new Project());
            Project = isNull ? null : requestedProject;
            return isNull;
        }

    }
}
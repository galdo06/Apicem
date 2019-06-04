using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Eisk.BusinessEntities;
using WS.Models;
using Eisk.BusinessLogicLayer;
using System.Net.Http;
using OAuth2.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ws.Validations
{
    public static class ProjectOrganismValidationConstants
    {
        public static readonly decimal LatMinValue = Convert.ToDecimal(ConfigurationManager.AppSettings["LatMinValue"]);
        public static readonly decimal LatMaxValue = Convert.ToDecimal(ConfigurationManager.AppSettings["LatMaxValue"]);

        public static readonly decimal LonMinValue = Convert.ToDecimal(ConfigurationManager.AppSettings["LonMinValue"]);
        public static readonly decimal LonMaxValue = Convert.ToDecimal(ConfigurationManager.AppSettings["LonMaxValue"]);

        public static readonly decimal HeightMinValue = Convert.ToDecimal(ConfigurationManager.AppSettings["HeightMinValue"]);
        public static readonly decimal HeightMaxValue = Convert.ToDecimal(ConfigurationManager.AppSettings["HeightMaxValue"]);

        public static readonly decimal CommentaryMaxLength = Convert.ToDecimal(ConfigurationManager.AppSettings["CommentaryMaxLength"]);

        public static readonly decimal VarasMaxCount = Convert.ToDecimal(ConfigurationManager.AppSettings["VarasMaxCount"]);

        public static readonly decimal DapMinValue = Convert.ToDecimal(ConfigurationManager.AppSettings["DapMinValue"]);
        public static readonly decimal DapMaxValue = Convert.ToDecimal(ConfigurationManager.AppSettings["DapMaxValue"]);
    }

    static class DecimalIsValid
    {
        public static bool IsValid(object value, decimal min, decimal max)
        {
            decimal lat = Convert.ToDecimal(value);
            if (lat < min || lat > max)
                return false;
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OrganismIDValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int organismID;
            Organism organism;
            if (value == null)
            {
                this.ErrorMessage = "OrganismID es requerido";
                return false;
            }

            if (!int.TryParse(value.ToString(), out organismID))
            {
                this.ErrorMessage = "OrganismID contiene formato inválido";
                return false;
            }

            if (organismID == 0 || OrganismModel.IsNull(new OrganismBLL().GetOrganismByOrganismID((int)value), out organism))
            {
                this.ErrorMessage = "OrganismID no existente";
                return false;
            }

            HttpContext.Current.Items["organism"] = organism;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectIDValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int projectID;
            Project project;
            if (value == null)
            {
                this.ErrorMessage = "ProjectID es requerido";
                return false;
            }

            if (!int.TryParse(value.ToString(), out projectID))
            {
                this.ErrorMessage = "ProjectID contiene formato inválido";
                return false;
            }

            if (projectID == 0 || ProjectModel.IsNull(new ProjectBLL().GetProjectByProjectID((int)value), out project))
            {
                this.ErrorMessage = "ProjectID no existente";
                return false;
            }

            User user = UserModel.GetUserFromAuthorizationHeader(HttpContext.Current.Request);

            List<Group> groups = UserModel.GetUserGroups(user);
            if (ProjectModel.IsAuthorized(project, groups))
                this.ErrorMessage = "ProjectID no autorizado";

            HttpContext.Current.Items["project"] = project;
            HttpContext.Current.Items["groups"] = groups;
            HttpContext.Current.Items["user"] = user;

            return true;

        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LatRangeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "Latitud fuera del rango permitido";
            return DecimalIsValid.IsValid(value, ProjectOrganismValidationConstants.LatMinValue, ProjectOrganismValidationConstants.LatMaxValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LonRangeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "Longitud fuera del rango permitido";
            return DecimalIsValid.IsValid(value, ProjectOrganismValidationConstants.LonMinValue, ProjectOrganismValidationConstants.LonMaxValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HeightRangeValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "Alto del arbol fuera del rango permitido";
            return DecimalIsValid.IsValid(value, ProjectOrganismValidationConstants.HeightMinValue, ProjectOrganismValidationConstants.HeightMaxValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class VarasCountValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = string.Format("La cantidad de varas exede el máximo de {0}", ConfigurationManager.AppSettings["VarasMaxCount"]);

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                HttpContext.Current.Items["varas"] = 0;
                return true;
            }

            bool isValid = DecimalIsValid.IsValid((int)value, 0M, ProjectOrganismValidationConstants.VarasMaxCount);

            if (isValid)
                HttpContext.Current.Items["varas"] = (int)value;

            return isValid;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DapsValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var jArray = JArray.Parse(value.ToString());
            int[] daps = jArray.ToObject<List<DapModel>>().Select(i=> i.DapValue).ToArray();

            HttpContext.Current.Items["daps"] = daps;

            int varas = (int)HttpContext.Current.Items["varas"];

            if ((daps.Length <= 0 && varas <= 0) || (daps.Length > 0 && varas > 0))
            {
                this.ErrorMessage = "Varas y/o Daps inválidos";
                return false;
            }

            for (int i = 0; i < daps.Length; i++)
            {
                if (!DecimalIsValid.IsValid(daps[i], ProjectOrganismValidationConstants.DapMinValue, ProjectOrganismValidationConstants.DapMaxValue))
                {
                    this.ErrorMessage = string.Format("DAP numero {0} fuera del rango permitido", i);
                    return false;
                }
            }

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConditionIDValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int conditionID;
            Condition condition;
            if (value == null)
            {
                this.ErrorMessage = "ConditionID es requerido";
                return false;
            }

            if (!int.TryParse(value.ToString(), out conditionID))
            {
                this.ErrorMessage = "ConditionID contiene formato inválido";
                return false;
            }

            if (conditionID == 0 || ConditionModel.IsNull(new ConditionBLL().GetConditionByConditionID((int)value), out condition))
            {
                this.ErrorMessage = "ConditionID no existente";
                return false;
            }

            HttpContext.Current.Items["condition"] = condition;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ActionProposedIDValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int actionProposedID;
            ActionProposed actionProposed;
            if (value == null)
            {
                this.ErrorMessage = "ActionProposedID es requerido";
                return false;
            }

            if (!int.TryParse(value.ToString(), out actionProposedID))
            {
                this.ErrorMessage = "ActionProposedID contiene formato inválido";
                return false;
            }

            if (actionProposedID == 0 || ActionProposedModel.IsNull(new ActionProposedBLL().GetActionProposedByActionProposedID((int)value), out actionProposed))
            {
                this.ErrorMessage = "ActionProposedID no existente";
                return false;
            }

            HttpContext.Current.Items["actionProposed"] = actionProposed;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CommentaryLengthValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            this.ErrorMessage = string.Format("La cantidad de caracteres para el comentario exede el máximo de {0}", ConfigurationManager.AppSettings["CommentaryMaxLength"]);
            return DecimalIsValid.IsValid(((string)value).Length, 0M, ProjectOrganismValidationConstants.CommentaryMaxLength);
        }
    }

    ///////////////////////////////////////////////////////////////////////////


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectOrganismIDValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int projectOrganismID;
            Project_Organisms projectOrganism;
            if (value == null)
            {
                this.ErrorMessage = "ProjectOrganismID es requerido";
                return false;
            }

            if (!int.TryParse(value.ToString(), out projectOrganismID))
            {
                this.ErrorMessage = "ProjectOrganismID contiene formato inválido";
                return false;
            }

            if (projectOrganismID == 0 || ProjectOrganismModel.IsNull(new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID((int)value), out projectOrganism))
            {
                this.ErrorMessage = "ProjectOrganismID no existente";
                return false;
            }

            User user = UserModel.GetUserFromAuthorizationHeader(HttpContext.Current.Request);

            List<Group> groups = UserModel.GetUserGroups(user);
            if (ProjectOrganismModel.IsAuthorized(projectOrganism, groups))
                this.ErrorMessage = "ProjectID no autorizado";

            HttpContext.Current.Items["projectOrganism"] = projectOrganism;
            HttpContext.Current.Items["groups"] = groups;
            HttpContext.Current.Items["user"] = user;

            return true;

        }
    }

}

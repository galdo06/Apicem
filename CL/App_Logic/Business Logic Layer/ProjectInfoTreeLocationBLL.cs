
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;
using System.Web;

namespace Eisk.BusinessLogicLayer
{
    public partial class ProjectInfoTreeLocationBLL
    {
        public static string GetTipoDesarrollo(int? _mitigation)
        {
            switch (_mitigation)
            {
                case 0:
                    return "Segregaciones";
                case 1:
                    return "Residenciales Unifamiliares";
                case 2:
                    return "Residenciales Multifamiliares";
                case 3:
                    return "Desarrollo Comercial, Industrial o Institucional";
                default:
                    return "";
            }
        }

        public static string GetMitigacion(ProjectInfoTreeLocation _projectInfoTreeLocation)
        {
            if (_projectInfoTreeLocation == null)
                return "";

            switch (_projectInfoTreeLocation.Mitigation)
            {
                case 0:
                case 1:
                    return "Mitigación por solares";
                case 2:
                case 3:
                    if (_projectInfoTreeLocation.PreviouslyImpacted.HasValue)
                    {
                        if ((bool)_projectInfoTreeLocation.PreviouslyImpacted)
                            return "No requerirá mitigación (Previamente Impactado)";
                    }
                    if (_projectInfoTreeLocation.Acres.HasValue)
                    {
                        if ((decimal)_projectInfoTreeLocation.Acres < 1)
                            return "No requerirá mitigación (Total de Cuerdas en el Proyecto menor de uno)";
                    }
                    return "Mitigación por perímetro";
                default:
                    return "";
            }
        }
    }
}
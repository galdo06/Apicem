
using CsvHelper.Configuration;
using Eisk.DataAccessLayer;
using Eisk.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Web;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using Eisk.Web.App_Logic.Helpers;

namespace Eisk.Web
{
    public class LatLngCSV : CsvClassMap<LatLng>
    {
        public override void CreateMap()
        {
            Map(i => i.Number).Index(0);
            Map(i => i.Lat).Index(1);
            Map(i => i.Lon).Index(2);
        }
    }

    public class LatLng
    {
        public int Number { get; set; }
        public Decimal Lat { get; set; }
        public Decimal Lon { get; set; }
    }

    public class StatePlaneCSV : CsvClassMap<StatePlane>
    {
        public override void CreateMap()
        {
            Map(i => i.Number).Index(0);
            Map(i => i.Y).Index(1);
            Map(i => i.X).Index(2);
        }
    }

    public class StatePlane
    {
        public int Number { get; set; }
        public Decimal Y { get; set; }
        public Decimal X { get; set; }
    }

    public partial class Tl_Upload_CSV : System.Web.UI.Page
    {
        protected string webPath = System.Configuration.ConfigurationManager.AppSettings["webPath"];

        private decimal minX = Convert.ToDecimal(ConfigurationManager.AppSettings["XMinValue"]);
        private decimal maxX = Convert.ToDecimal(ConfigurationManager.AppSettings["XMaxValue"]);

        private decimal minY = Convert.ToDecimal(ConfigurationManager.AppSettings["YMinValue"]);
        private decimal maxY = Convert.ToDecimal(ConfigurationManager.AppSettings["YMaxValue"]);

        private decimal minLon = Convert.ToDecimal(ConfigurationManager.AppSettings["LonMinValue"]);
        private decimal maxLon = Convert.ToDecimal(ConfigurationManager.AppSettings["LonMaxValue"]);

        private decimal minLat = Convert.ToDecimal(ConfigurationManager.AppSettings["LatMinValue"]);
        private decimal maxLat = Convert.ToDecimal(ConfigurationManager.AppSettings["LatMaxValue"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            imgStatePlanes.ImageUrl = webPath + "/App_Resources/images/StatePlanes.png";
            imgLatLon.ImageUrl = webPath + "/App_Resources/images/LatLon.png";
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Session["positions"] != null)
            {
                using (var _DatabaseContext = new DatabaseContext())
                {
                    var projectID = Convert.ToInt32(Page.RouteData.Values["project_id"] as string);
                    Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
                    List<Eisk.BusinessEntities.Group> groups = user.Group_Users.Select(instance => instance.Group).ToList();
                    int groupID = groups[0].GroupID;
                    Eisk.BusinessEntities.Project project = _DatabaseContext.Projects.Where(proj => proj.ProjectID == projectID).First();

                    int newPositions = 0;
                    int count = 0;
                    var positionsObject = (object)Session["positions"];

                    if (positionsObject is List<LatLng>)
                    {
                        List<LatLng> positions = (List<LatLng>)positionsObject;
                        count = positions.Count;

                        for (int i = 0; i < positions.Count; i++)
                        {
                            LatLng position = positions[i];
                            Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(position.Lon.ToString(), position.Lat.ToString(), @"~/App_Resources/client-scripts/tl/");

                            // If exists - EDIT
                            var tree = _DatabaseContext.TreeDetails.FirstOrDefault(thisTree => thisTree.Number == position.Number && thisTree.Project_Organisms.ProjectID == project.ProjectID);
                            if (tree != null)
                            {
                                tree.X = Convert.ToDecimal(anewpointObj["x"]);
                                tree.Y = Convert.ToDecimal(anewpointObj["y"]);
                                tree.Lat = position.Lat;
                                tree.Lon = position.Lon;
                                tree.EditedDate = DateTime.Now;
                                tree.EditorUserID = user.UserID;
                            }
                            // Else - create
                            else
                            {
                                new TreeDetailBLL().CreateProject_Organism(
                                   0,
                                   0,
                                   position.Lat,
                                   position.Lon,
                                   new int[1] { 1 },
                                   _DatabaseContext.ActionProposeds.Where(act => act.ActionProposedID == 5).First(), // Det Luego
                                   _DatabaseContext.Conditions.Where(cond => cond.ConditionID == 1).First(),  //  Excelente
                                   "",
                                   _DatabaseContext.Organisms.Where(org => org.ScientificNameID == 546 && org.GroupID == groupID).First(), //Desconocido
                                   project,
                                   user);
                                newPositions++;
                            }
                        }
                    }

                    if (positionsObject is List<StatePlane>)
                    {
                        List<StatePlane> positions = (List<StatePlane>)positionsObject;
                        count = positions.Count;

                        for (int i = 0; i < positions.Count; i++)
                        {
                            StatePlane position = positions[i];
                            Dictionary<string, object> anewpointObj = Utility.ConvertToLatLng(position.X.ToString(), position.Y.ToString(), @"~/App_Resources/client-scripts/tl/");

                            // If exists - EDIT
                            var tree = _DatabaseContext.TreeDetails.FirstOrDefault(thisTree => thisTree.Number == position.Number && thisTree.Project_Organisms.ProjectID == project.ProjectID);
                            if (tree != null)
                            {
                                tree.X = position.X;
                                tree.Y = position.Y;
                                tree.Lat = Convert.ToDecimal(anewpointObj["y"]);
                                tree.Lon = Convert.ToDecimal(anewpointObj["x"]);
                                tree.EditedDate = DateTime.Now;
                                tree.EditorUserID = user.UserID;
                            }
                            // Else - create
                            else
                            {
                                new TreeDetailBLL().CreateProject_Organism(
                                   0,
                                   0,
                                    Convert.ToDecimal(anewpointObj["y"]),
                                    Convert.ToDecimal(anewpointObj["x"]),
                                   new int[1] { 1 },
                                   _DatabaseContext.ActionProposeds.Where(act => act.ActionProposedID == 5).First(), // Det Luego
                                   _DatabaseContext.Conditions.Where(cond => cond.ConditionID == 1).First(),  //  Excelente
                                   "",
                                   _DatabaseContext.Organisms.Where(org => org.ScientificNameID == 546 && org.GroupID == groupID).First(), //Desconocido
                                   project,
                                   user);
                                newPositions++;
                            }
                        }
                    }

                    _DatabaseContext.SaveChanges();

                    string message = "El importe fué satisfactorio.<br />El total de posiciones de árboles existentes procesadas fue de " + (count - newPositions) + ".";
                    if (newPositions > 0)
                        message += "<br />El total de árboles nuevos añadidos fue de " + newPositions + ".";

                    ltlMessage.Text = MessageFormatter.GetFormattedSuccessMessage(message);
                }
            }

        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {

            if (!FileUploadControl.HasFile)   //Upload file here
            {
                ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Importe Cancelado. Favor de seleccionar un documento.");
                return;
            }

            string fileExt = System.IO.Path.GetExtension(FileUploadControl.FileName);  //Get extension
            if (fileExt != ".csv")   //check to see if its a .csv file
            {
                ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Importe Cancelado. Solo documentos .csv permitidos.");
                return;
            }

            int newPositions = 0;
            string errors = string.Empty;
            List<StatePlane> positionsStatePlane = new List<StatePlane>();
            List<LatLng> positionsLatLng = new List<LatLng>();

            using (var _DatabaseContext = new DatabaseContext())
            {

                var projectID = Convert.ToInt32(Page.RouteData.Values["project_id"] as string);
                Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
                List<Eisk.BusinessEntities.Group> groups = user.Group_Users.Select(instance => instance.Group).ToList();
                int groupID = groups[0].GroupID;
                Eisk.BusinessEntities.Project project = _DatabaseContext.Projects.Where(proj => proj.ProjectID == projectID).First();

                var numbers = _DatabaseContext.TreeDetails.Where(i => i.Project_Organisms.ProjectID == project.ProjectID).Select(i => i.Number.Value).ToList();
                int max = numbers.Count > 0 ? numbers.Max() : 0;

                var csvConfiguration = new CsvConfiguration();
                csvConfiguration.HasHeaderRecord = true;

                using (var stream = new StreamReader(FileUploadControl.PostedFile.InputStream))
                using (var csvReader = new CsvHelper.CsvReader(stream, csvConfiguration))
                {
                    int line = 1;
                    while (csvReader.Read())
                    {
                        if (rblPosition.SelectedValue == "0")
                        {
                            try
                            {
                                StatePlane position = csvReader.GetRecord<StatePlane>();
                                if (position == null)
                                    errors += "<br />Línea " + line + " Inválida. Formato incorrecto.";
                                else if (position.X > maxX || position.X < minX)
                                    errors += "<br />Línea " + line + " Inválida. X Fuera del Rango Permitido.";
                                else if (position.Y > maxY || position.Y < minY)
                                    errors += "<br />Línea " + line + " Inválida. Y Fuera del Rango Permitido.";
                                else
                                {
                                    // If exists - EDIT
                                    var tree = _DatabaseContext.TreeDetails.FirstOrDefault(thisTree => thisTree.Number == position.Number && thisTree.Project_Organisms.ProjectID == project.ProjectID);
                                    if (tree == null) // Nuevo
                                    {
                                        if (position.Number <= 0)
                                            errors += "<br />Línea " + line + " Inválida. Number menor o igual a 0.";
                                        else if (position.Number > (max + 1))
                                            errors += "<br />Línea " + line + " Inválida. El Number " + position.Number + " es inválido. Este interrupe la secuencia de números puesto a que el árbol con número " + (position.Number - 1) + " no existe.";
                                        else
                                        {
                                            newPositions++;
                                            max++;
                                            positionsStatePlane.Add(position);
                                        }
                                    }
                                    else
                                        positionsStatePlane.Add(position);
                                }
                            }
                            catch
                            {
                                errors += "<br />Línea " + line + " Inválida. Formato incorrecto.";
                            }
                        }
                        else
                        {
                            try
                            {
                                LatLng position = csvReader.GetRecord<LatLng>();
                                if (position == null)
                                    errors += "<br />Línea " + line + " Inválida. Formato incorrecto.";
                                else if (position.Lon > maxLon || position.Lon < minLon)
                                    errors += "<br />Línea " + line + " Inválida. Longitud Fuera del Rango Permitido";
                                else if (position.Lat > maxLat || position.Lat < minLat)
                                    errors += "<br />Línea " + line + " Inválida. Latitud Fuera del Rango Permitido";
                                else
                                {
                                    // If exists - EDIT
                                    var tree = _DatabaseContext.TreeDetails.FirstOrDefault(thisTree => thisTree.Number == position.Number && thisTree.Project_Organisms.ProjectID == project.ProjectID);
                                    if (tree == null)
                                        newPositions++;

                                    positionsLatLng.Add(position);
                                }
                            }
                            catch
                            {
                                errors += "<br />Línea " + line + " Inválida. Formato incorrecto.";
                            }
                        }
                        line++;
                    }

                    if (errors != string.Empty)
                    {
                        ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Importe Cancelado. Distintos errores encontrados:<br />" + errors);
                        return;
                    }
                    if (positionsStatePlane.Count == 0 && positionsLatLng.Count == 0)
                    {
                        ltlMessage.Text = MessageFormatter.GetFormattedNoticeMessage("Importe Cancelado:<br /><br />Documento vacío");
                        return;
                    }

                    if ((positionsStatePlane.Count + positionsLatLng.Count) != _DatabaseContext.Project_Organisms.Where(i => i.ProjectID == projectID).Count())
                    {

                    }
                }

                if (errors == string.Empty)
                {
                    int count = 0;

                    if (rblPosition.SelectedValue == "0")
                    {
                        Session["positions"] = positionsStatePlane;
                        count = positionsStatePlane.Count;
                    }
                    else
                    {
                        Session["positions"] = positionsLatLng;
                        count = positionsLatLng.Count;
                    }

                    string message = "getConfirm('El total de posiciones de árboles existentes a procesarse es " + (count - newPositions) + ". ###');";
                    if (newPositions > 0)
                        message = message.Replace("###", "###El total de árboles nuevos a añadirse es " + newPositions + ".");
                    else
                        message = message.Replace("###", "");

                    Page.ClientScript.RegisterStartupScript(this.GetType(), null, message, true);
                }
                else
                {
                    Session["positions"] = null;
                }
            }

        }


        protected void rblPosition_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected bool ValidateNumber(int number)
        {
            bool isValid = false;
            if (number <= 0)
                return false;


            return isValid;
        }


    }
}
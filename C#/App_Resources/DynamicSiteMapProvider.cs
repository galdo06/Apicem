using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace Eisk.Web.App_Resources
{
    public class DynamicSiteMapProvider : StaticSiteMapProvider
    {
        public DynamicSiteMapProvider()
            : base()
        {

        }

        private String _siteMapFileName;
        private SiteMapNode _rootNode = null;

        // Return the root node of the current site map.
        public override SiteMapNode RootNode
        {
            get
            {
                return BuildSiteMap();
            }
        }

        /// <summary>
        /// Pull out the filename of the site map xml.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection attributes)
        {
            base.Initialize(name, attributes);
            _siteMapFileName = attributes["siteMapFile"];
        }

        private const String SiteMapNodeName = "siteMapNode";

        public override SiteMapNode BuildSiteMap()
        {
            lock (this)
            {
                //if (null == _rootNode)
                //{
                Clear();

                // Load the sitemap's xml from the file.
                XmlDocument siteMapXml = LoadSiteMapXml();

                // Create the first site map item from the top node in the xml.
                XmlElement rootElement = (XmlElement)siteMapXml.GetElementsByTagName(SiteMapNodeName)[0];

                System.Security.Principal.IIdentity userId = HttpContext.Current.User.Identity;
                if (!string.IsNullOrEmpty(userId.Name))
                {
                    AddUserAreaNodes(rootElement, userId.Name);
                }

                string projectID = (new Page().RouteData.Values["project_id"] as string);
                if (!string.IsNullOrEmpty(projectID) && projectID != "0")
                {
                    // This is the key method - add the dynamic nodes to the xml
                    AddProjectNodes(rootElement, projectID);
                    AddTreeLocationNodes(rootElement, projectID);
                }

                AddAboutNodes(rootElement);

                // Now build up the site map structure from the xml
                GenerateSiteMapNodes(rootElement);
                //}
            }
            return _rootNode;
        }

        protected bool IsAdmin()
        {
            System.Security.Principal.IIdentity userId = HttpContext.Current.User.Identity;
            if (string.IsNullOrEmpty(userId.Name))
                return false;

            return new UserBLL().GetUserByUserName(userId.Name).Role_Users.First().Role.RoleName == "Administrator";
        }

        /// <summary>
        /// Open the site map file as an xml document.
        /// </summary>
        /// <returns>The contents of the site map file.</returns>
        private XmlDocument LoadSiteMapXml()
        {
            XmlDocument siteMapXml = new XmlDocument();
            siteMapXml.Load(AppDomain.CurrentDomain.BaseDirectory + _siteMapFileName);
            return siteMapXml;
        }

        /// <summary>
        /// Creates the site map nodes from the root of 
        /// the xml document.
        /// </summary>
        /// <param name="rootElement">The top-level sitemap element from the XmlDocument loaded with the site map xml.</param>
        private void GenerateSiteMapNodes(XmlElement rootElement)
        {
            _rootNode = GetSiteMapNodeFromElement(rootElement);
            AddNode(_rootNode);
            CreateChildNodes(rootElement, _rootNode);
        }

        /// <summary>
        /// Recursive method! This finds all the site map elements
        /// under the current element, and creates a SiteMapNode for 
        /// them.  On each of these, it calls this method again to 
        /// create it's new children, and so on.
        /// </summary>
        /// <param name="parentElement">The xml element to iterate through.</param>
        /// <param name="parentNode">The site map node to add the new children to.</param>
        private void CreateChildNodes(XmlElement parentElement, SiteMapNode parentNode)
        {
            foreach (XmlNode xmlElement in parentElement.ChildNodes)
            {
                if (xmlElement.Name == SiteMapNodeName)
                {
                    SiteMapNode childNode = GetSiteMapNodeFromElement((XmlElement)xmlElement);
                    AddNode(childNode, parentNode);
                    CreateChildNodes((XmlElement)xmlElement, childNode);
                }
            }
        }


        /// <summary>
        /// The key method. You can add your own code in here
        /// to add xml nodes to the structure, from a 
        /// database, file on disk, or just from code.
        /// To keep the example short, I'm just adding from code.
        /// </summary>
        /// <param name="rootElement"></param>     

        private void AddUserAreaNodes(XmlElement rootElement, string userIdName)
        {
            Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName(userIdName);

            string userInfoPath = new Page().GetRouteUrl("userinfo-details", new { edit_mode = "view", userinfo_id = user.UserID });
            string changePasswordPath = new Page().GetRouteUrl("change-password", new { });
            string notifications = new Page().GetRouteUrl("notifications", new { });

            XmlElement teams = AddDynamicChildElement(rootElement, "~/secured/member/member_default.aspx?", "Área de Usuario", "Área de Usuario");
            XmlElement projects = AddDynamicChildElement(rootElement, "~/secured/member/projects.aspx", "Proyectos", "Proyectos");
            AddDynamicChildElement(teams, userInfoPath, "Detalles de Usuario", "Detalles de Usuario");
            AddDynamicChildElement(teams, notifications, "Notificaciones", "Notificaciones");
            AddDynamicChildElement(teams, changePasswordPath, "Cambiar Contraseña", "Cambiar Contraseña");
        }

        private void AddProjectNodes(XmlElement rootElement, string projectID)
        {
            string project_details = new Page().GetRouteUrl("project-details", new { edit_mode = "view", Project_id = projectID });
            var projectInfo = new ProjectBLL().GetProjectByProjectID(Convert.ToInt32(projectID)).ProjectInfoes.First();

            XmlElement teams = AddDynamicChildElement(rootElement, "~/secured/member/tl/project_dafault.aspx?", "Proyecto: " + projectInfo.ProjectName, "Proyecto: " + projectInfo.ProjectName);
           //XmlElement teams = AddDynamicChildElement(rootElement, "~/secured/member/tl/project_dafault.aspx?", "Proyecto", "Proyecto");
            AddDynamicChildElement(teams, project_details, "Detalles Generales", "Detalles Generales");
        }

        private void AddTreeLocationNodes(XmlElement rootElement, string projectID)
        {
            string treelocation = new Page().GetRouteUrl("tl", new { Project_id = projectID });
            string tl_treeinventory = new Page().GetRouteUrl("tl-treeinventory", new { Project_id = projectID });
            string tl_reports = new Page().GetRouteUrl("tl-reports", new { Project_id = projectID });
            string tl_project_details = new Page().GetRouteUrl("tl-project-details", new { edit_mode = "edit", Project_id = projectID });
            string tl_upload_csv = new Page().GetRouteUrl("tl-upload-csv", new { Project_id = projectID });
            string commonnames = new Page().GetRouteUrl("commonnames", new { Project_id = projectID });

            XmlElement teams = AddDynamicChildElement(rootElement, "~/secured/member/project_dafault.aspx?", "Corte y Poda", "Corte y Poda");
            AddDynamicChildElement(teams, tl_project_details, "Detalles de Corte y Poda", "Detalles de Corte y Poda");
            AddDynamicChildElement(teams, treelocation, "Plano", "Plano");
            AddDynamicChildElement(teams, tl_treeinventory, "Inventario de Árboles", "Inventario de Árboles");
            AddDynamicChildElement(teams, tl_upload_csv, "Importar Árboles", "Importar Árboles");
            //AddDynamicChildElement(teams, "~/secured/member/commonnames.aspx", "Nom. Comunes Árboles", "Nom. Comunes Árboles");
            AddDynamicChildElement(teams, commonnames, "Nom. Comunes Árboles", "Nom. Comunes Árboles");
            AddDynamicChildElement(teams, tl_reports, "Reportes", "Reportes");
        }

        private void AddAboutNodes(XmlElement rootElement)
        {
            XmlElement teams = AddDynamicChildElement(rootElement, "~/about.aspx?", "Información General", "Información General");
            AddDynamicChildElement(teams, "~/about.aspx", "Información sobre N@TURA", "Información sobre N@TURA");
        }

        private static XmlElement AddDynamicChildElement(XmlElement parentElement, String url, String title, String description)
        {
            // Create new element from the parameters
            XmlElement childElement = parentElement.OwnerDocument.CreateElement(SiteMapNodeName);
            childElement.SetAttribute("url", url);
            childElement.SetAttribute("title", title);
            childElement.SetAttribute("description", description);

            // Add it to the parent
            parentElement.AppendChild(childElement);
            return childElement;
        }

        private SiteMapNode GetSiteMapNodeFromElement(XmlElement rootElement)
        {
            SiteMapNode newSiteMapNode;
            String url = rootElement.GetAttribute("url");
            String title = rootElement.GetAttribute("title");
            String description = rootElement.GetAttribute("description");

            // The key needs to be unique, so hash the url and title.
            newSiteMapNode = new SiteMapNode(this,
                (url + title).GetHashCode().ToString(), url, title, description);

            return newSiteMapNode;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }

        // Empty out the existing items.
        protected override void Clear()
        {
            lock (this)
            {
                _rootNode = null;
                base.Clear();
            }
        }
    }
}

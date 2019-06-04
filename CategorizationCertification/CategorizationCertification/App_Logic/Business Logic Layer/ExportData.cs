using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eisk.Web.App_Logic.Business_Logic_Layer
{
    public class ExportData
    {
        public ExportData(){}

        public ExportData(global::System.Int32 projectID, global::System.String projectName, global::System.Int32 organismTypeID, global::System.String organismTypeName, global::System.Int32 organismID, global::System.String commonName, global::System.String scientificName)
        {
            ProjectID = projectID;
            ProjectName = projectName;
            OrganismTypeID = organismTypeID;
            OrganismTypeName = organismTypeName;
            OrganismID = organismID;
            CommonName = commonName;
            ScientificName = scientificName;
        }
        
        private global::System.Int32 _ProjectID;
        public global::System.Int32 ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }

        private global::System.String _ProjectName;
        public global::System.String ProjectName
        {
            get { return _ProjectName; }
            set { _ProjectName = value; }
        }

        private global::System.Int32 _OrganismTypeID;
        public global::System.Int32 OrganismTypeID
        {
            get { return _OrganismTypeID; }
            set { _OrganismTypeID = value; }
        }

        private global::System.String _OrganismTypeName;
        public global::System.String OrganismTypeName
        {
            get { return _OrganismTypeName; }
            set { _OrganismTypeName = value; }
        }

        private global::System.Int32 _OrganismID;
        public global::System.Int32 OrganismID
        {
            get { return _OrganismID; }
            set { _OrganismID = value; }
        }

        private global::System.String _CommonName;
        public global::System.String CommonName
        {
            get { return _CommonName; }
            set { _CommonName = value; }
        }

        private global::System.String _ScientificName;
        public global::System.String ScientificName
        {
            get { return _ScientificName; }
            set { _ScientificName = value; }
        }
    }
}